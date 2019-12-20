using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Common.Extensions;
using LiveScore.Common.LangResources;
using LiveScore.Core;
using LiveScore.Core.Controls.TabStrip;
using LiveScore.Core.Models.Leagues;
using LiveScore.Core.Services;
using Prism.Navigation;

namespace LiveScore.Features.Favorites.ViewModels
{
    public class FavoriteLeaguesViewModel : TabItemViewModel
    {
        private readonly IFavoriteService favoriteService;

        public FavoriteLeaguesViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IFavoriteService favoriteService)
            : base(navigationService, dependencyResolver, null, null, AppResources.Leagues)
        {
            this.favoriteService = favoriteService;
            TapLeagueCommand = new DelegateAsyncCommand<FavoriteLeague>(OnTapLeagueAsync);
        }

        public ObservableCollection<FavoriteLeague> FavoriteLeagues { get; private set; }

        public DelegateAsyncCommand<FavoriteLeague> TapLeagueCommand { get; }

        public override void OnAppearing()
        {
            base.OnAppearing();

            Debug.WriteLine($"FavoriteLeaguesViewModel OnAppearing");

            FavoriteLeagues = new ObservableCollection<FavoriteLeague>(favoriteService.GetLeagues());

            HasData = FavoriteLeagues.Any();
        }

        private async Task OnTapLeagueAsync(FavoriteLeague league)
        {

            var parameters = new NavigationParameters
            {
                { "LeagueId", league.Id },
                { "LeagueSeasonId", league.LeagueSeasonId },
                { "LeagueRoundGroup", league.LeagueRoundGroup },
                { "LeagueGroupName", league.LeagueGroupName },
                { "CountryFlag", league.CountryFlag }
            };

            await NavigationService
                .NavigateAsync("LeagueDetailView" + CurrentSportId, parameters)
                .ConfigureAwait(false);
        }
    }
}