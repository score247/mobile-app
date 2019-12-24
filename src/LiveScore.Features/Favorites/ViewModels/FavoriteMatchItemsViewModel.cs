using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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

        protected override void InitializeMatchItems(IEnumerable<IMatch> matches)
        {
            base.InitializeMatchItems(matches);

            Device.BeginInvokeOnMainThread(() => HeaderViewModel = null);
        }

        protected override async Task<IEnumerable<IMatch>> LoadMatchesFromServiceAsync()
        {
            var favoriteMatches = favoriteService.GetMatches().OrderByDescending(match => match.EventDate);
            var matches = await matchService.GetMatchesByIds(favoriteMatches.Select(match => match.Id).ToArray(), CurrentLanguage);

            return matches;
        }

        protected override async Task OnRemovedFavorite(string matchId)
        {
            await base.OnRemovedFavorite(matchId);

            MatchItemsSource.RemoveMatches(new[] { matchId }, buildFlagUrlFunc, NavigationService, CurrentSportId);
        }
    }
}