namespace Score.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Common.ViewModels;
    using Prism.Commands;
    using Prism.Navigation;
    using Score.Views.Templates;
    using Xamarin.Forms;

    public class MatchDetailViewModel : ViewModelBase
    {
        public MatchDetailViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            MatchDetailItems = new List<string>()
            {
                "MatchInfo",
                "MatchStats"
            };

            CurrentMatchDetailIndex = 0;
        }

        private List<string> matchDetailItems;

        public List<string> MatchDetailItems
        {
            get { return matchDetailItems; }
            set { SetProperty(ref matchDetailItems, value); }
        }

        private int currentMatchDetailIndex;

        public int CurrentMatchDetailIndex
        {
            get { return currentMatchDetailIndex; }
            set { SetProperty(ref currentMatchDetailIndex, value); }
        }

        public DelegateCommand SelectMatchDetailTabCommand => new DelegateCommand(OnSelectMatchDetailTabCommandExecuted);

        private void OnSelectMatchDetailTabCommandExecuted()
        {
            CurrentMatchDetailIndex = 1;
        }
    }
}