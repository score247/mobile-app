using System;
using System.Threading.Tasks;
using LiveScore.Common.LangResources;
using LiveScore.Core;
using LiveScore.Core.Controls.TabStrip;
using LiveScore.Core.NavigationParams;
using Prism.Events;
using Prism.Navigation;

namespace LiveScore.Soccer.ViewModels.Leagues.LeagueDetails.Fixtures
{
    public class FixturesViewModel : TabItemViewModel, IDisposable
    {
        private bool disposed;

        public FixturesViewModel(
             INavigationService navigationService,
             IDependencyResolver serviceLocator,
             IEventAggregator eventAggregator,
             LeagueDetailNavigationParameter league)
             : base(navigationService, serviceLocator, null, eventAggregator, AppResources.Fixtures)
        {
            MatchesViewModel = new FixturesMatchesViewModel(NavigationService, DependencyResolver, EventAggregator, league);
        }

        public FixturesMatchesViewModel MatchesViewModel { get; private set; }

        public override void OnAppearing() => MatchesViewModel.OnAppearing();

        public override void OnDisappearing() => MatchesViewModel.OnDisappearing();

        public override void OnResumeWhenNetworkOK() => MatchesViewModel.OnResumeWhenNetworkOK();

        public override void OnSleep() => MatchesViewModel.OnSleep();

        public override Task OnNetworkReconnectedAsync() => MatchesViewModel.OnNetworkReconnectedAsync();

        public override void Destroy()
        {
            base.Destroy();

            Dispose();
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                MatchesViewModel?.Dispose();
                MatchesViewModel = null;
            }

            disposed = true;
        }
    }
}