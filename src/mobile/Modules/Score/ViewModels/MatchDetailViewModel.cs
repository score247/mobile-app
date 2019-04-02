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
                new TabModel {
                    Id = 0,
                    Name = "Trackers",
                    Template = new MatchTrackerTemplate { BindingContext = new MatchViewModelBase(NavigationService) { MatchId = "1" } }
                },
            };
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters != null)
            {
                var match = parameters[nameof(Match)] as Match;
                MatchId = match.Event.Id;
            }
        }
    }
}