namespace Score.ViewModels
{
    using System.Collections.Generic;
    using Common.Controls.TabStrip;
    using Common.Models;
    using Common.ViewModels;
    using Prism.Navigation;
    using Score.Views.Templates;

    public class MatchDetailViewModel : MatchViewModelBase
    {
        private List<TabModel> matchDetailItems;

        public List<TabModel> MatchDetailItems
        {
            get { return matchDetailItems; }
            set { SetProperty(ref matchDetailItems, value); }
        }

        public MatchDetailViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            MatchDetailItems = new List<TabModel>();
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            if (parameters != null)
            {
                var match = parameters[nameof(Match)] as Match;
                MatchId = match.Event.Id;
            }

            if(MatchDetailItems.Count == 0)
            {
                InitMatchDetailItems(MatchId);
            }
        }

        private void InitMatchDetailItems(string matchId)
        {
            MatchDetailItems = new List<TabModel>
            {
                new TabModel { Id = 0, Name = "Odds", Template = new MatchOdds1x2Template() },
                new TabModel { Id = 1, Name = "Info", Template = new MatchInfoTemplate() },
                new TabModel { Id = 2, Name = "Trackers", Template = new MatchTrackerTemplate { BindingContext = new MatchViewModelBase(NavigationService) { MatchId = matchId } } },
                new TabModel { Id = 3, Name = "Stats", Template = new MatchStatsTemplate() },
                new TabModel { Id = 4, Name = "LineUps", Template = new MatchLineUpsTemplate() },
                new TabModel { Id = 5, Name = "H2H", Template = new MatchH2HAwayTeamTemplate() },
                new TabModel { Id = 6, Name = "Table", Template = new MatchTableTotalFullListTemplate() },
                new TabModel { Id = 7, Name = "Social", Template = new MatchInfoTemplate() },
                new TabModel { Id = 8, Name = "TV Schedule", Template = new MatchInfoTemplate() }
            };
        }
    }
}