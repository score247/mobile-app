using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Core;
using LiveScore.Core.Extensions;
using LiveScore.Core.Models.Matches;
using LiveScore.Core.ViewModels;
using Prism.Events;
using Prism.Navigation;
using Xamarin.Forms;

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
            favoriteService.OnRemovedFavorite += HandleRemovedFavorite;
        }

        public override async void OnAppearing()
        {
            base.OnAppearing();

            SubscribeEvents();

            if (FirstLoad)
            {
                await LoadDataAsync(LoadMatchesAsync);

                FirstLoad = false;
            }
            else
            {
                await Task.Run(() => LoadDataAsync(UpdateMatchesAsync, false));
            }
        }

        public override async void OnResumeWhenNetworkOK()
        {
            base.OnResumeWhenNetworkOK();

            SubscribeEvents();
            await Task.Run(() => LoadDataAsync(UpdateMatchesAsync, false));
        }

        protected override void InitializeMatchItems(IEnumerable<IMatch> matches)
        {
            base.InitializeMatchItems(matches);

            Device.BeginInvokeOnMainThread(() => HeaderViewModel = null);
        }

        protected override async Task<IEnumerable<IMatch>> LoadMatchesFromServiceAsync()
        {
            var favoriteMatches = favoriteService.GetAll()?.ToList();

            if (favoriteMatches?.Any() != true)
            {
                HeaderViewModel = this;
                HasData = false;
                return Enumerable.Empty<IMatch>();
            }

            var matches = await matchService.GetMatchesByIds(favoriteMatches.Select(match => match.Id).ToArray(), CurrentLanguage);

            return matches?.OrderByDescending(match => match.EventDate);
        }

        private Task HandleRemovedFavorite(IMatch match)
        {
            MatchItemsSource.RemoveMatches(new[] { match.Id }, buildFlagUrlFunc, NavigationService, CurrentSportId);

            if (MatchItemsSource?.Any() != true)
            {
                HeaderViewModel = this;
                HasData = false;
            }

            return Task.CompletedTask;
        }
    }
}