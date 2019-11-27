using System.Collections.Generic;
using System.Threading.Tasks;
using LiveScore.Core;
using LiveScore.Core.ViewModels;
using LiveScore.Soccer.ViewModels.Leagues.LeagueDetails.Fixtures;
using LiveScore.Soccer.ViewModels.Leagues.LeagueDetails.Table;
using PanCardView.EventArgs;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;

namespace LiveScore.Soccer.ViewModels.Leagues
{
    public class LeagueDetailViewModel : ViewModelBase
    {
        public LeagueDetailViewModel(
         INavigationService navigationService,
         IDependencyResolver dependencyResolver,
         IEventAggregator eventAggregator)
         : base(navigationService, dependencyResolver, eventAggregator)
        {
            ItemAppearedCommand = new DelegateCommand<ItemAppearedEventArgs>(OnItemAppeared);
            ItemDisappearingCommand = new DelegateCommand<ItemDisappearingEventArgs>(OnItemDisappearing);
        }

        public IReadOnlyList<ViewModelBase> LeagueDetailItemSources { get; private set; }

        public byte SelectedIndex { get; set; }

        public DelegateCommand<ItemAppearedEventArgs> ItemAppearedCommand { get; }

        public DelegateCommand<ItemDisappearingEventArgs> ItemDisappearingCommand { get; }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            var leagueId = parameters["LeagueId"]?.ToString();
            var leagueSeasonId = parameters["LeagueSeasonId"]?.ToString();
            var leagueRoundGroup = parameters["LeagueRoundGroup"]?.ToString();
            var leagueName = parameters["LeagueName"]?.ToString();
            var countryFlag = parameters["CountryFlag"]?.ToString();

            LeagueDetailItemSources = new List<ViewModelBase> {
                new TableViewModel(leagueId, leagueSeasonId, leagueRoundGroup, NavigationService, DependencyResolver, null, leagueName, countryFlag),
                new FixturesViewModel(leagueId, NavigationService, DependencyResolver, EventAggregator)
            };

            LeagueDetailItemSources[SelectedIndex]?.OnAppearing();
        }

        public override Task OnNetworkReconnectedAsync() => LeagueDetailItemSources[SelectedIndex]?.OnNetworkReconnectedAsync();

        public override void OnResumeWhenNetworkOK()
        {
            base.OnResumeWhenNetworkOK();

            LeagueDetailItemSources[SelectedIndex]?.OnResumeWhenNetworkOK();
        }

        public override void OnSleep()
        {
            base.OnSleep();

            LeagueDetailItemSources[SelectedIndex]?.OnSleep();
        }

        public override void OnAppearing()
        {
            base.OnAppearing();

            LeagueDetailItemSources[SelectedIndex]?.OnAppearing();
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();

            LeagueDetailItemSources[SelectedIndex]?.OnDisappearing();
        }

        private void OnItemAppeared(ItemAppearedEventArgs args)
        {
            LeagueDetailItemSources[SelectedIndex]?.OnAppearing();
        }

        private void OnItemDisappearing(ItemDisappearingEventArgs args)
        {
            if (args.Index >= 0)
            {
                var previousItem = LeagueDetailItemSources[args.Index];

                previousItem.OnDisappearing();
            }
        }
    }
}