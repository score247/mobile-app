namespace LiveScore.League.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using Core.Factories;
    using Core.Models.Leagues;
    using Core.Services;
    using Core.ViewModels;
    using LiveScore.Core.Models.Matches;
    using Prism.Commands;
    using Prism.Navigation;

    public class LeagueDetailViewModel : ViewModelBase
    {
        private ObservableCollection<IGrouping<MatchHeaderItemViewModel, IMatch>> groupMatches;

        public LeagueDetailViewModel(
            INavigationService navigationService,
            IGlobalFactoryProvider globalFactory,
            ISettingsService settingsService)
                : base(navigationService, globalFactory, settingsService)
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