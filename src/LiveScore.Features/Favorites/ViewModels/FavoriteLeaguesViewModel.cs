using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Common;
using LiveScore.Common.Extensions;
using LiveScore.Common.LangResources;
using LiveScore.Core;
using LiveScore.Core.Controls.TabStrip;
using LiveScore.Core.Models.Leagues;
using LiveScore.Core.NavigationParams;
using LiveScore.Core.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;

namespace LiveScore.Features.Favorites.ViewModels
{
    public class FavoriteLeaguesViewModel : TabItemViewModel
    {
        private readonly IFavoriteService<ILeague> favoriteService;
        private readonly Func<string, string> buildFlagFunction;

        public FavoriteLeaguesViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator)
                : base(navigationService, dependencyResolver, null, eventAggregator, AppResources.Leagues)
        {
            favoriteService = DependencyResolver.Resolve<IFavoriteService<ILeague>>(CurrentSportId.ToString());
            buildFlagFunction = DependencyResolver.Resolve<Func<string, string>>(FuncNameConstants.BuildFlagUrlFuncName);

            TapLeagueCommand = new DelegateAsyncCommand<LeagueItemViewModel>(OnTapLeagueAsync);
            UnFavoriteCommand = new DelegateCommand<ILeague>(UnFavoriteLeague);

            favoriteService.OnRemovedFavorite += HandleRemovedFavorite;
        }

        public ObservableCollection<LeagueItemViewModel> FavoriteLeagues { get; private set; }

        public bool? HasHeader { get; private set; }

        public DelegateAsyncCommand<LeagueItemViewModel> TapLeagueCommand { get; }

        public DelegateCommand<ILeague> UnFavoriteCommand { get; }

        public override void OnAppearing()
        {
            base.OnAppearing();

            FavoriteLeagues = new ObservableCollection<LeagueItemViewModel>(
                favoriteService.GetAll()
                    .OrderBy(league => league.Order)
                    .ThenBy(league => league.Name)
                    .Select(league => new LeagueItemViewModel(league, buildFlagFunction(league.CountryCode))));

            SetHasDataAndHeader();
        }

        private async Task OnTapLeagueAsync(LeagueItemViewModel item)
        {
            var leagueDetailNavigationParameter = new LeagueDetailNavigationParameter(
                    item.League.Id,
                    item.League.Name,
                    item.League.Order,
                    item.League.CountryCode,
                    item.League.IsInternational,
                    item.League.RoundGroup,
                    " ", // SeasonId empty to get latest season
                    item.League.HasStandings,
                    item.League.HasGroups);

            var parameters = new NavigationParameters
            {
                { "League", leagueDetailNavigationParameter },
                { "CountryFlag", item.CountryFlag }
            };

            await NavigationService
                .NavigateAsync("LeagueDetailView" + CurrentSportId, parameters)
                .ConfigureAwait(false);
        }

        private void UnFavoriteLeague(ILeague league)
            => favoriteService.Remove(league);

        private void SetHasDataAndHeader()
        {
            HasData = FavoriteLeagues.Any();
            HasHeader = HasData ? null : (bool?)true;
        }

        private Task HandleRemovedFavorite(ILeague league)
        {
            var viewModel = FavoriteLeagues?.FirstOrDefault(view => view.League.Id == league.Id);

            if (viewModel != null)
            {
                FavoriteLeagues.Remove(viewModel);

                SetHasDataAndHeader();
            }

            return Task.CompletedTask;
        }
    }
}