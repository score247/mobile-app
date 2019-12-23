using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Common;
using LiveScore.Common.Extensions;
using LiveScore.Common.LangResources;
using LiveScore.Core;
using LiveScore.Core.Controls.TabStrip;
using LiveScore.Core.Models.Leagues;
using LiveScore.Core.Services;
using LiveScore.Core.Views;
using Prism.Navigation;
using Rg.Plugins.Popup.Services;

namespace LiveScore.Features.Favorites.ViewModels
{
    public class FavoriteLeaguesViewModel : TabItemViewModel
    {
        private static string LeagueLimitationMessage = string.Format(AppResources.FavoriteLeagueLimitation, 30);
        private readonly IFavoriteService favoriteService;
        private readonly Func<string, string> buildFlagFunction;

        public FavoriteLeaguesViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IFavoriteService favoriteService)
            : base(navigationService, dependencyResolver, null, null, AppResources.Leagues)
        {
            this.favoriteService = favoriteService;
            this.favoriteService.OnAddedFunc = OnAddedFavorite;
            this.favoriteService.OnRemovedFunc = OnRemovedFavorite;
            this.favoriteService.OnReachedLimit = OnReachedLimitation;

            buildFlagFunction = DependencyResolver.Resolve<Func<string, string>>(FuncNameConstants.BuildFlagUrlFuncName);

            TapLeagueCommand = new DelegateAsyncCommand<LeagueItemViewModel>(OnTapLeagueAsync);
        }

        public ObservableCollection<LeagueItemViewModel> FavoriteLeagues { get; private set; }

        public bool? HasHeader { get; private set; }

        public DelegateAsyncCommand<LeagueItemViewModel> TapLeagueCommand { get; }

        public override void OnAppearing()
        {
            base.OnAppearing();

            Debug.WriteLine($"FavoriteLeaguesViewModel OnAppearing");

            FavoriteLeagues = new ObservableCollection<LeagueItemViewModel>(
                favoriteService.GetLeagues()
                .OrderBy(league => league.Order)
                .Select(league => new LeagueItemViewModel(league, buildFlagFunction(league.CountryCode))));

            HasData = FavoriteLeagues.Any();

            HasHeader = HasData ? null : (bool?)true;
        }

        private async Task OnTapLeagueAsync(LeagueItemViewModel item)
        {
            var parameters = new NavigationParameters
            {
                { "LeagueId", item.League.Id },
                { "LeagueSeasonId", item.League.SeasonId },
                { "LeagueRoundGroup", item.League.RoundGroup },
                { "LeagueGroupName", item.League.Name },
                { "CountryFlag", item.CountryFlag},
                { "LeagueOrder", item.League.Order },
                { "CountryCode", item.League.CountryCode },
                { "IsInternational", item.League.IsInternational }
            };

            var isSuccess = await NavigationService
                .NavigateAsync("LeagueDetailView" + CurrentSportId, parameters)
                .ConfigureAwait(false);
        }

        private static Task OnAddedFavorite()
            => PopupNavigation.Instance.PushAsync(new FavoritePopupView(AppResources.AddedFavorite));

        private static Task OnRemovedFavorite()
            => PopupNavigation.Instance.PushAsync(new FavoritePopupView(AppResources.RemovedFavorite));

        private static Task OnReachedLimitation()
            => PopupNavigation.Instance.PushAsync(new FavoritePopupView(LeagueLimitationMessage));
    }
}