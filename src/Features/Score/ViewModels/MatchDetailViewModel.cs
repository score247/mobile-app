namespace LiveScore.Score.ViewModels
{
    using System.Collections.Generic;
    using Common.Controls.TabStrip;
    using Core.ViewModels;
    using LiveScore.Core.Factories;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Score.Views.Templates;
    using Prism.Navigation;

    public class MatchDetailViewModel : MatchViewModelBase
    {
        public MatchDetailViewModel(INavigationService navigationService, IDepdendencyResolver serviceLocator)
            : base(navigationService, serviceLocator)
        {
            MatchDetailItems = new List<TabModel>();
        }

        public List<TabModel> MatchDetailItems { get; set; }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            if (parameters != null)
            {
                var match = parameters["Match"] as IMatch;
                MatchId = match.Id;
            }

            if (MatchDetailItems.Count == 0)
            {
                InitMatchDetailItems(MatchId);
            }
        }

        private void InitMatchDetailItems(string matchId)
        {
            MatchDetailItems = new List<TabModel>
            {
                new TabModel { Name = "Odds", Template = new MatchOdds1x2Template() },
                new TabModel
                {
                    Name = "Trackers",
                    Template = new MatchTrackerTemplate
                    {
                        BindingContext = new MatchViewModelBase(NavigationService, ServiceLocator) { MatchId = matchId }
                    }
                },
                new TabModel { Name = "Stats", Template = new MatchStatsTemplate() },
                new TabModel { Name = "LineUps", Template = new MatchLineUpsTemplate() },
                new TabModel { Name = "H2H", Template = new MatchH2HAwayTeamTemplate() },
                new TabModel { Name = "Table", Template = new MatchTableTotalFullListTemplate() },
                new TabModel { Name = "Social", Template = new MatchSocialTemplate() },
                new TabModel { Name = "TV Schedule", Template = new MatchInfoTemplate() }
            };
        }
    }
}