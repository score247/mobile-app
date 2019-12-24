using System.Threading.Tasks;
using LiveScore.Common.LangResources;
using LiveScore.Core;
using LiveScore.Core.Controls.TabStrip;
using Prism.Events;
using Prism.Navigation;

namespace LiveScore.Features.Favorites.ViewModels
{
    public class FavoriteMatchesViewModel : TabItemViewModel
    {
        public FavoriteMatchesViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator)
            : base(navigationService, dependencyResolver, null, eventAggregator, AppResources.Matches)
        {
            IsActive = true;
            MatchesViewModel = new FavoriteMatchItemsViewModel(NavigationService, DependencyResolver, EventAggregator);
        }

        public FavoriteMatchItemsViewModel MatchesViewModel { get; }

        public override void OnAppearing() => MatchesViewModel.OnAppearing();

        public override void OnDisappearing() => MatchesViewModel.OnDisappearing();

        public override void OnResumeWhenNetworkOK() => MatchesViewModel.OnResumeWhenNetworkOK();

        public override void OnSleep() => MatchesViewModel.OnSleep();

        public override Task OnNetworkReconnectedAsync() => MatchesViewModel.OnNetworkReconnectedAsync();
    }
}