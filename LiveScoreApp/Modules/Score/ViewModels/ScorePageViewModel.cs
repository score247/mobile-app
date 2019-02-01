namespace Score.ViewModels
{
    using Prism.Navigation;
    using Score.Models;
    using Score.Services;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class ScorePageViewModel : ViewModelBase
    {
        private ObservableCollection<IGrouping<string, Match>> groupMatches;
        private IScoreService scoreService;

        public ObservableCollection<IGrouping<string, Match>> GroupMatches
        {
            get { return groupMatches; }
            set { SetProperty(ref groupMatches, value); }
        }

        public ScorePageViewModel(INavigationService navigationService, IScoreService scoreService)
            : base(navigationService)
        {
            this.scoreService = scoreService;
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            var matches = scoreService.GetAll();

            GroupMatches = new ObservableCollection<IGrouping<string, Match>>(matches.GroupBy(x => x.GroupName));
        }
    }
}