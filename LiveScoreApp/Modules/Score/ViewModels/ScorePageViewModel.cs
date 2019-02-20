namespace Score.ViewModels
{
    using Prism.Commands;
    using Prism.Navigation;
    using Score.Models;
    using Score.Services;
    using Score.Views;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class ScorePageViewModel : ViewModelBase
    {
        private ObservableCollection<IGrouping<string, Match>> groupMatches;
        private IScoreService scoreService;

        public DelegateCommand SelectMatchCommand { get; set; }

        public ObservableCollection<IGrouping<string, Match>> GroupMatches
        {
            get { return groupMatches; }
            set { SetProperty(ref groupMatches, value); }
        }

        public ScorePageViewModel(INavigationService navigationService, IScoreService scoreService)
            : base(navigationService)
        {
            this.scoreService = scoreService;
            var matches = scoreService.GetAll();
            GroupMatches = new ObservableCollection<IGrouping<string, Match>>(matches.GroupBy(x => x.GroupName));
            SelectMatchCommand = new DelegateCommand(OnSelectMatch);
        }

        private async void OnSelectMatch()
        {
            await NavigationService.NavigateAsync(nameof(MatchInfoPage));
        }
    }
}