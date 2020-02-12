using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Common.Services;
using LiveScore.Core;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Events.FavoriteEvents.Matches;
using LiveScore.Core.Extensions;
using LiveScore.Core.Models.Favorites;
using LiveScore.Core.Models.Matches;
using LiveScore.Soccer.Models.Matches;
using Prism.Events;

namespace LiveScore.Soccer.Services
{
    public class FavoriteMatchService : SoccerFavoriteService<IMatch>
    {
        public FavoriteMatchService(
            IUserSettingService userSettingService,
            IEventAggregator eventAggregator,
            ISettings settings,
            ILoggingService loggingService,
            IApiService apiService,
            IUserService userService,
            INetworkConnection networkConnection,
            SoccerApi.FavoriteApi favoriteApi = null)
                : base(userSettingService, eventAggregator, nameof(FavoriteMatchService), 99, loggingService, apiService, userService, settings, networkConnection, favoriteApi)
        {
            CleanUpAndInit();

            OnAddedFunc = PublishAddEvent;
            OnRemovedFunc = PublishRemoveEvent;
            OnReachedLimitFunc = PublishReachLimitEvent;
        }

        public override IList<IMatch> GetAll()
            => base.GetAll().Where(match => match.IsEnableFavorite()).ToList();

        public override bool IsFavorite(IMatch obj)
        {
            if (obj.IsEnableFavorite())
            {
                return base.IsFavorite(obj);
            }

            RemoveFromCache(obj);

            return false;
        }

#pragma warning disable S3215 // "interface" instances should not be cast to concrete types

        public override void UpdateCache()
        {
            userSettingService.AddOrUpdateValue(
                Key,
                Objects.Select(obj => obj as SoccerMatch).ToList());
        }

#pragma warning disable S3215 // "interface" instances should not be cast to concrete types

        public override IList<IMatch> LoadCache()
        {
            return userSettingService.GetValueOrDefault(
                    Key,
                    Enumerable.Empty<SoccerMatch>()).Select(l => l as IMatch).ToList();
        }

        private Task PublishAddEvent(IMatch match)
        {
            Task.Run(() => Sync(
               addedFavorites: new List<Favorite> { new Favorite(match.Id, FavoriteType.MatchValue) }
            ));

            return Task.Run(() => eventAggregator.GetEvent<AddFavoriteMatchEvent>().Publish(match));
        }

        private Task PublishRemoveEvent(IMatch match)
        {
            Task.Run(() => Sync(
                removedFavorites: new List<Favorite> { new Favorite(match.Id, FavoriteType.MatchValue) }
            ));

            return Task.Run(() => eventAggregator.GetEvent<RemoveFavoriteMatchEvent>().Publish(match));
        }

        private Task PublishReachLimitEvent()
            => Task.Run(() => eventAggregator.GetEvent<ReachLimitFavoriteMatchesEvent>().Publish());

        private void CleanUpAndInit()
        {
            Init();

            var matches = LoadCache();
            var disableMatches = matches.Where(match => !match.IsEnableFavorite());
            var enableMatches = matches.Except(disableMatches).ToList();

            userSettingService.AddOrUpdateValue(Key, enableMatches.Select(obj => obj as SoccerMatch).ToList());
            Objects = enableMatches;

            Task.Run(() => Sync(
                addedFavorites: enableMatches.Select(match => new Favorite(match.Id, FavoriteType.MatchValue)).ToList(),
                removedFavorites: disableMatches.Select(match => new Favorite(match.Id, FavoriteType.MatchValue)).ToList())
            );
        }
    }
}