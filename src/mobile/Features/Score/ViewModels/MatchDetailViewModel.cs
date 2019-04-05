namespace Score.ViewModels
{
    using System.Collections.Generic;
    using Common.Controls.TabStrip;
    using Core.Factories;
    using Core.Models.MatchInfo;
    using Core.Services;
    using Core.ViewModels;
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

        public MatchDetailViewModel(
            INavigationService navigationService,
            IGlobalFactory globalFactory,
            ISettingsService settingsService)
                : base(navigationService, globalFactory, settingsService)
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

            if (MatchDetailItems.Count == 0)
            {
                InitMatchDetailItems(MatchId);
            }
        }

        private void InitMatchDetailItems(string matchId)
        {
            MatchDetailItems = new List<TabModel>
            {
                new TabModel { Name = "Odds", Template = new MatchTableTotalFullListTemplate() },
                //new TabModel { Name = "Info", Template = new MatchInfoTemplate() },
                new TabModel
                {
                    Name = "Trackers",
                    Template = new MatchTrackerTemplate
                    {
                        BindingContext = new MatchViewModelBase(NavigationService, GlobalFactory, SettingsService)
                        {
                            MatchId = matchId
                        }
                    }
                },
                //new TabModel { Name = "Stats", Template = new MatchStatsTemplate() },
                //new TabModel { Name = "LineUps", Template = new MatchLineUpsTemplate() },
                //new TabModel { Name = "H2H", Template = new MatchH2HAwayTeamTemplate() },
                //new TabModel { Name = "Table", Template = new MatchTableTotalFullListTemplate() },
                //new TabModel { Name = "Social", Template = new MatchSocialTemplate() },
                //new TabModel { Name = "TV Schedule", Template = new MatchInfoTemplate() }
            };
        }
    }
}