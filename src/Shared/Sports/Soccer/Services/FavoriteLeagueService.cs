﻿using System.Collections.Generic;
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
       
        public FavoriteLeagueService(IUserSettingService userSettingService, IEventAggregator eventAggrerator)
                : base(userSettingService, eventAggrerator)
        {
            Init(nameof(FavoriteLeagueService), LeagueLimit);
                        
            OnRemovedFunc = PublishRemovedEvent;
            OnAddedFunc = PublishAddedEvent;
            OnReachedLimit = PublishReachLimitEvent;
        }

        private Task PublishRemovedEvent(ILeague league)
        => Task.Run(() => eventAggrerator.GetEvent<RemoveFavoriteLeagueEvent>().Publish(league));

        private Task PublishAddedEvent()
        => Task.Run(() => eventAggrerator.GetEvent<AddFavoriteLeagueEvent>().Publish());

        private Task PublishReachLimitEvent()
        => Task.Run(() => eventAggrerator.GetEvent<ReachLimitFavoriteLeaguesEvent>().Publish());

        public override void UpdateCache()
        {
            userSettingService.AddOrUpdateValue(Key, Objects.Select(obj => obj as League).ToList());
        }

        public override IList<ILeague> LoadCache()
        => userSettingService.GetValueOrDefault(Key, Enumerable.Empty<League>()).Select(l => l as ILeague).ToList();


    }
}