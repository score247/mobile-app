using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Common.Services;
using LiveScore.Core;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Events.FavoriteEvents.Leagues;
using LiveScore.Core.Models.Favorites;
using LiveScore.Core.Models.Leagues;
using LiveScore.Soccer.Models.Leagues;
using Prism.Events;

namespace LiveScore.Soccer.Services.Favorites
{
    public class FavoriteLeagueService : SoccerFavoriteService<ILeague>
    {
        private const int LeagueLimit = 30;

        public FavoriteLeagueService(
            IUserSettingService userSettingService,
            IEventAggregator eventAggregator,
            ISettings settings,
            ILoggingService loggingService,
            IApiService apiService,
            IUserService userService,
            INetworkConnection networkConnection,
            SoccerApi.FavoriteApi favoriteApi = null)
                : base(userSettingService, eventAggregator, nameof(FavoriteLeagueService), LeagueLimit, loggingService, apiService, userService, settings, networkConnection, favoriteApi)
        {
            Init();

            Task.Run(() => Sync(
                addedFavorites: Objects.Select(league => new Favorite(league.Id, FavoriteType.LeagueValue, league.Name)).ToList()
            ));

            OnRemovedFunc = PublishRemovedEvent;
            OnAddedFunc = PublishAddedEvent;
            OnReachedLimitFunc = PublishReachLimitEvent;
        }

        private Task PublishRemovedEvent(ILeague league)
        {
            Task.Run(() => Sync(removedFavorites: new List<Favorite> { new Favorite(league.Id, FavoriteType.LeagueValue, league.Name) }));

            return Task.Run(() => eventAggregator.GetEvent<RemoveFavoriteLeagueEvent>().Publish(league));
        }

        private Task PublishAddedEvent(ILeague league)
        {
            Task.Run(() => Sync(addedFavorites: new List<Favorite> { new Favorite(league.Id, FavoriteType.LeagueValue, league.Name) }));

            return Task.Run(() => eventAggregator.GetEvent<AddFavoriteLeagueEvent>().Publish(league));
        }

        private Task PublishReachLimitEvent()
            => Task.Run(() => eventAggregator.GetEvent<ReachLimitFavoriteLeaguesEvent>().Publish());

#pragma warning disable S3215 // "interface" instances should not be cast to concrete types

        public override void UpdateCache()
        {
            userSettingService.AddOrUpdateValue(Key, Objects.Select(obj => obj as League).ToList());
        }

#pragma warning restore S3215 // "interface" instances should not be cast to concrete types

        public override IList<ILeague> LoadCache()
            => userSettingService.GetValueOrDefault(Key, Enumerable.Empty<League>()).Select(l => l as ILeague).ToList();
    }
}