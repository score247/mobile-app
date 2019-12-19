using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Core;
using LiveScore.Core.Models.Matches;
using LiveScore.Core.ViewModels;
using Prism.Events;
using Prism.Navigation;

namespace LiveScore.Features.Favorites.ViewModels
{
    public class FavoriteMatchItemsViewModel : MatchesViewModel
    {
        public FavoriteMatchItemsViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator)
            : base(navigationService, dependencyResolver, eventAggregator)
        {
            HeaderViewModel = this;
        }

        public override async void OnAppearing()
        {
            base.OnAppearing();

            SubscribeEvents();

            if (FirstLoad)
            {
                await LoadDataAsync(LoadMatchesAsync);
            }
            else
            {
                await Task.Run(() => LoadDataAsync(UpdateMatchesAsync));
            }
        }

        public override async void OnResumeWhenNetworkOK()
        {
            base.OnResumeWhenNetworkOK();

            SubscribeEvents();
            await LoadDataAsync(LoadMatchesAsync);
        }

        protected override Task<IEnumerable<IMatch>> LoadMatchesFromServiceAsync()
        {
            return Task.FromResult(Enumerable.Empty<IMatch>());
        }
    }
}