using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Common.Services;
using LiveScore.Core.Events.FavoriteEvents.Matches;
using LiveScore.Core.Extensions;
using LiveScore.Core.Models.Matches;
using LiveScore.Core.Services;
using LiveScore.Soccer.Models.Matches;
using Prism.Events;

namespace LiveScore.Soccer.Services
{
    public class FavoriteMatchService : FavoriteService<IMatch>
    {
        public FavoriteMatchService(IUserSettingService userSettingService, IEventAggregator eventAggregator)
                : base(userSettingService, eventAggregator, nameof(FavoriteMatchService), 99)
        {
            CleanUpAndInit();

            OnAddedFunc = PublishAddEvent;
            OnRemovedFunc = PublishRemoveEvent;
            OnReachedLimit = PublishReachLimitEvent;
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

        public override void UpdateCache()
        {
            userSettingService.AddOrUpdateValue(
                Key,
                Objects.Select(obj => obj as SoccerMatch).ToList());
        }

        public override IList<IMatch> LoadCache()
        {
            return userSettingService.GetValueOrDefault(
                    Key,
                    Enumerable.Empty<SoccerMatch>()).Select(l => l as IMatch).ToList();
        }

        private Task PublishAddEvent()
            => Task.Run(() => eventAggregator.GetEvent<AddFavoriteMatchEvent>().Publish());

        private Task PublishRemoveEvent(IMatch match)
            => Task.Run(() => eventAggregator.GetEvent<RemoveFavoriteMatchEvent>().Publish(match));

        private Task PublishReachLimitEvent()
            => Task.Run(() => eventAggregator.GetEvent<ReachLimitFavoriteMatchesEvent>().Publish());

        private void CleanUpAndInit()
        {
            var matches = LoadCache();
            var disableMatches = matches.Where(match => !match.IsEnableFavorite());
            var enableMatches = matches.Except(disableMatches).ToList();

            userSettingService.AddOrUpdateValue(Key, enableMatches.Select(obj => obj as SoccerMatch).ToList());
            Objects = enableMatches;
        }
    }
}