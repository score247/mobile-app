﻿using LiveScore.Core;
using LiveScore.Core.Models.Matches;
using LiveScore.Soccer.Enumerations;
using LiveScore.Soccer.Extensions;
using LiveScore.Soccer.Models.Matches;
using Prism.Navigation;

namespace LiveScore.Soccer.ViewModels.MatchDetailInfo
{
    public class SubstitutionItemViewModel : BaseItemViewModel
    {    
        public SubstitutionItemViewModel(
            TimelineEvent timelineEvent,
            MatchInfo matchInfo,
            INavigationService navigationService,
            IDependencyResolver dependencyResolver)
             : base(timelineEvent, matchInfo, navigationService, dependencyResolver)
        {
            PlayerOutImageSource = Images.SubstitutionOut.Value;
            PlayerInImageSource = Images.SubstitutionIn.Value;
        }

        public string HomePlayerOutName { get; private set; }

        public string HomePlayerInName { get; private set; }

        public string AwayPlayerOutName { get; private set; }

        public string AwayPlayerInName { get; private set; }

        public string PlayerOutImageSource { get; private set; }

        public string PlayerInImageSource { get; private set; }

        public override BaseItemViewModel BuildData()
        {
            base.BuildData();            
           
            if (TimelineEvent.OfHomeTeam())
            {
                BuildHomeInfo();
            }
            else
            {
                BuildAwayInfo();
            }

            return this;
        }

        private void BuildHomeInfo()
        {
            HomePlayerOutName = TimelineEvent.PlayerOut?.Name;
            HomePlayerInName = TimelineEvent.PlayerIn?.Name;

            VisibleHomeImage = true;
        }

        private void BuildAwayInfo()
        {
            AwayPlayerOutName = TimelineEvent.PlayerOut?.Name;
            AwayPlayerInName = TimelineEvent.PlayerIn?.Name;

            VisibleAwayImage = true;
        }
    }
}