using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Common.Services;
using LiveScore.Core.Events.FavoriteEvents.Matches;
using LiveScore.Core.Models.Matches;
using LiveScore.Core.Services;
using LiveScore.Soccer.Models.Matches;
using Prism.Events;

namespace LiveScore.Soccer.Services
{
    public class FavoriteMatchService : FavoriteService<IMatch>
    {
        public FavoriteMatchService(IUserSettingService userSettingService, IEventAggregator eventAggrerator)
                : base(userSettingService, eventAggrerator)
        {
            Init(nameof(FavoriteMatchService), 99);
            OnAddedFunc = PublishAddEvent;
            OnRemovedFunc = PublishRemoveEvent;
            OnReachedLimit = PublishReachLimitEvent;
        }

        public override void UpdateCache()
        {
            userSettingService.AddOrUpdateValue(Key, Objects.Select(obj => obj as SoccerMatch).ToList());
        }

        public override IList<IMatch> LoadCache()
            => userSettingService.GetValueOrDefault(Key, Enumerable.Empty<SoccerMatch>()).Select(l => l as IMatch).ToList();

        private Task PublishAddEvent()
            => Task.Run(() => eventAggrerator.GetEvent<AddFavoriteMatchEvent>().Publish());

        private Task PublishRemoveEvent(IMatch match)
            => Task.Run(() => eventAggrerator.GetEvent<RemoveFavoriteMatchEvent>().Publish(match));

        private Task PublishReachLimitEvent()
            => Task.Run(() => eventAggrerator.GetEvent<ReachLimitFavoriteMatchesEvent>().Publish());
    }
}