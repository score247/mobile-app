namespace LiveScore.Soccer.ViewModels.MatchDetailInfo
{
    using LiveScore.Core;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Matches;
    using Prism.Navigation;

    public class CardItemViewModel : BaseInfoItemViewModel
    {
        public CardItemViewModel(
            ITimeline timelineEvent,
            IMatchResult matchResult,
            INavigationService navigationService,
            IDependencyResolver depdendencyResolver)
             : base(timelineEvent, matchResult, navigationService, depdendencyResolver)
        {
        }

        public bool VisibleHomeYellowCard { get; set; }

        public bool VisibleHomeRedCard { get; set; }

        public bool VisibleHomeRedYellowCard { get; set; }

        public bool VisibleAwayYellowCard { get; set; }

        public bool VisibleAwayRedCard { get; set; }

        public bool VisibleAwayRedYellowCard { get; set; }

        protected override void BuildInfo()
        {
            base.BuildInfo();

            if (TimelineEvent.Team == "home")
            {
                HomePlayerName = TimelineEvent.Player.Name;
            }
            else
            {
                AwayPlayerName = TimelineEvent.Player.Name;
            }

            BuildYellowCard();
            BuildRedCard();
            BuildRedYellowCard();
        }

        private void BuildRedYellowCard()
        {
            if (TimelineEvent.Type == EventTypes.YellowRedCard)
            {
                if (TimelineEvent.Team == "home")
                {
                    VisibleHomeRedYellowCard = true;
                }
                else
                {
                    VisibleAwayRedYellowCard = true;
                }
            }
        }

        private void BuildRedCard()
        {
            if (TimelineEvent.Type == EventTypes.RedCard)
            {
                if (TimelineEvent.Team == "home")
                {
                    VisibleHomeRedCard = true;
                }
                else
                {
                    VisibleAwayRedCard = true;
                }
            }
        }

        private void BuildYellowCard()
        {
            if (TimelineEvent.Type == EventTypes.YellowCard)
            {
                if (TimelineEvent.Team == "home")
                {
                    VisibleHomeYellowCard = true;
                }
                else
                {
                    VisibleAwayYellowCard = true;
                }
            }
        }
    }
}
