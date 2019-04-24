namespace LiveScore.League.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using Core.Models.Leagues;
    using Core.ViewModels;
    using LiveScore.Core;
    using LiveScore.Core.Models.Matches;
    using Prism.Commands;
    using Prism.Navigation;

    public class LeagueDetailViewModel : ViewModelBase
    {
        public LeagueDetailViewModel(INavigationService navigationService, IDepdendencyResolver serviceLocator) : base(navigationService, serviceLocator)
        {
            GroupMatches = new ObservableCollection<IGrouping<MatchHeaderItemViewModel, IMatch>>();
        }

        public ObservableCollection<IGrouping<MatchHeaderItemViewModel, IMatch>> GroupMatches { get; set; }

        public bool IsRefreshingMatchList { get; set; }

        public DelegateCommand RefreshCommand
            => new DelegateCommand(() =>
            {
                IsRefreshingMatchList = true;
                IsRefreshingMatchList = false;
            });

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters != null)
            {
                var selectedLeagueName = parameters[nameof(League)] as ILeague;
                Title = selectedLeagueName?.Name ?? string.Empty;
            }
        }

    }
}