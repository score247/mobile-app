namespace LiveScore.League.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.ViewModels;
    using Prism.Commands;
    using Prism.Navigation;
    using Core.Factories;
    using Core.Services;
    using Core.Constants;
    using Core.Models.Leagues;
    using LiveScore.Core.Models.Matches;

    public class LeagueDetailViewModel : ViewModelBase
    {
        private bool isRefreshingMatchList;
        private ObservableCollection<IGrouping<MatchHeaderItemViewModel, IMatch>> groupMatches;

        public LeagueDetailViewModel(
            INavigationService navigationService,
            IGlobalFactoryProvider globalFactory,
            ISettingsService settingsService)
                : base(navigationService, globalFactory, settingsService)
        {
            GroupMatches = new ObservableCollection<IGrouping<MatchHeaderItemViewModel, IMatch>>();
        }

        public ObservableCollection<IGrouping<MatchHeaderItemViewModel, IMatch>> GroupMatches
        {
            get { return groupMatches; }
            set { SetProperty(ref groupMatches, value); }
        }

        public bool IsRefreshingMatchList
        {
            get => isRefreshingMatchList;
            set => SetProperty(ref isRefreshingMatchList, value);
        }

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