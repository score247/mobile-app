namespace Score.ViewModels
{
    using System.Collections.Generic;
    using Common.Controls.TabStrip;
    using Common.ViewModels;
    using Prism.Navigation;
    using Score.Views.Templates;

    public class MatchDetailViewModel : ViewModelBase
    {
        private List<TabModel> matchDetailItems;

        public MatchDetailViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            InitMatchDetailItems();
        }

        public List<TabModel> MatchDetailItems
        {
            get { return matchDetailItems; }
            set { SetProperty(ref matchDetailItems, value); }
        }

        private void InitMatchDetailItems()
        {
            MatchDetailItems = new List<TabModel>
            {
                new TabModel { Id = 0, Name = "Odds", TemplateType = typeof(MatchOdds1x2Template) },
                new TabModel { Id = 1, Name = "Info", TemplateType = typeof(MatchInfoTemplate) },
                new TabModel { Id = 2, Name = "Trackers", TemplateType = typeof(MatchTrackerTemplate) },
                new TabModel { Id = 3, Name = "Stats", TemplateType = typeof(MatchStatsTemplate) },
                new TabModel { Id = 4, Name = "LineUps", TemplateType = typeof(MatchLineUpsTemplate) },
                new TabModel { Id = 5, Name = "H2H", TemplateType = typeof(MatchH2HAwayTeamTemplate) },
                new TabModel { Id = 6, Name = "Table", TemplateType = typeof(MatchTableTotalFullListTemplate) },
                new TabModel { Id = 7, Name = "Social", TemplateType = typeof(MatchInfoTemplate) },
                new TabModel { Id = 8, Name = "TV Schedule", TemplateType = typeof(MatchInfoTemplate) },
            };
        }
    }
}