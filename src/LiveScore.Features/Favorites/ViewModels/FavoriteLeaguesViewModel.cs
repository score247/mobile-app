﻿using System;
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
using Prism.Commands;
using Prism.Navigation;
using Rg.Plugins.Popup.Contracts;

namespace LiveScore.Features.Favorites.ViewModels
{
    public class FavoriteLeaguesViewModel : TabItemViewModel
    {
        private static readonly string LeagueLimitationMessage = string.Format(AppResources.FavoriteLeagueLimitation, 30);
        private readonly IFavoriteService favoriteService;
        private readonly Func<string, string> buildFlagFunction;
        private readonly IPopupNavigation popupNavigation;

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
            popupNavigation = DependencyResolver.Resolve<IPopupNavigation>();

            TapLeagueCommand = new DelegateAsyncCommand<LeagueItemViewModel>(OnTapLeagueAsync);

            UnfavoriteCommand = new DelegateCommand<ILeague>(UnfavoriteLeague);
        }

        public ObservableCollection<LeagueItemViewModel> FavoriteLeagues { get; private set; }

        public bool? HasHeader { get; private set; }

        public DelegateAsyncCommand<LeagueItemViewModel> TapLeagueCommand { get; }

        public DelegateCommand<ILeague> UnfavoriteCommand { get; }

        public override void OnAppearing()
        {
            base.OnAppearing();

            Debug.WriteLine($"FavoriteLeaguesViewModel OnAppearing");

            FavoriteLeagues = new ObservableCollection<LeagueItemViewModel>(
                favoriteService.GetLeagues()
                .OrderBy(league => league.Order)
                .Select(league => new LeagueItemViewModel(league, buildFlagFunction(league.CountryCode))));

            SetHasDataAndHeader();
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

            await NavigationService
                .NavigateAsync("LeagueDetailView" + CurrentSportId, parameters)
                .ConfigureAwait(false);
        }

        private void UnfavoriteLeague(ILeague league)
        => favoriteService.RemoveLeague(league);

        private void SetHasDataAndHeader()
        {
            HasData = FavoriteLeagues.Any();
            HasHeader = HasData ? null : (bool?)true;
        }

        private Task OnAddedFavorite()
            => popupNavigation.PushAsync(new FavoritePopupView(AppResources.AddedFavorite));

        private Task OnRemovedFavorite(string id)
        {
            var viewModel = FavoriteLeagues.FirstOrDefault(league => league.League.Id == id);

            FavoriteLeagues.Remove(viewModel);

            SetHasDataAndHeader();

            return popupNavigation.PushAsync(new FavoritePopupView(AppResources.RemovedFavorite));
        }        

        private Task OnReachedLimitation()
            => popupNavigation.PushAsync(new FavoritePopupView(LeagueLimitationMessage));
    }
}