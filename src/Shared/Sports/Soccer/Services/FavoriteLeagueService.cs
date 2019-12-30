using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Common.Services;
using LiveScore.Core.Events.FavoriteEvents.Leagues;
using LiveScore.Core.Models.Leagues;
using LiveScore.Core.Services;
using LiveScore.Soccer.Models.Leagues;
using Prism.Events;

namespace LiveScore.Soccer.Services
{
    public class FavoriteLeagueService : FavoriteService<ILeague>
    {
        private const int LeagueLimit = 30;

        public FavoriteLeagueService(IUserSettingService userSettingService, IEventAggregator eventAggregator)
                : base(userSettingService, eventAggregator, nameof(FavoriteLeagueService), LeagueLimit)
        {
            Init();

            OnRemovedFunc = PublishRemovedEvent;
            OnAddedFunc = PublishAddedEvent;
            OnReachedLimit = PublishReachLimitEvent;
        }

        private Task PublishRemovedEvent(ILeague league)
            => Task.Run(() => eventAggregator.GetEvent<RemoveFavoriteLeagueEvent>().Publish(league));

        private Task PublishAddedEvent()
            => Task.Run(() => eventAggregator.GetEvent<AddFavoriteLeagueEvent>().Publish());

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