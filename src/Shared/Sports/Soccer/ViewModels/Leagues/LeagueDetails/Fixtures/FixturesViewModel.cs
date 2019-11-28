using System.Threading.Tasks;
using LiveScore.Common.LangResources;
using LiveScore.Core;
using LiveScore.Core.Controls.TabStrip;
using Prism.Events;
using Prism.Navigation;

namespace LiveScore.Soccer.ViewModels.Leagues.LeagueDetails.Fixtures
{
    public class FixturesViewModel : TabItemViewModel
    {
        public FixturesViewModel(
            string leagueId,
            string leagueGroupName,
             INavigationService navigationService,
             IDependencyResolver serviceLocator,
             IEventAggregator eventAggregator)
             : base(navigationService, serviceLocator, null, eventAggregator, AppResources.Fixtures)
        {
            MatchesViewModel = new FixturesMatchesViewModel(leagueId, leagueGroupName, NavigationService, DependencyResolver, EventAggregator);
        }

        public FixturesMatchesViewModel MatchesViewModel { get; }

        public override void OnAppearing() => MatchesViewModel.OnAppearing();

        public override void OnDisappearing() => MatchesViewModel.OnDisappearing();

        public override void OnResumeWhenNetworkOK() => MatchesViewModel.OnResumeWhenNetworkOK();

        public override void OnSleep() => MatchesViewModel.OnSleep();

        public override Task OnNetworkReconnectedAsync() => MatchesViewModel.OnNetworkReconnectedAsync();
    }
}