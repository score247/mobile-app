﻿using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Soccer.Tests")]

namespace LiveScore.Soccer.ViewModels.DetailOdds
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using LiveScore.Common.Extensions;
    using LiveScore.Core;
    using LiveScore.Core.Controls.TabStrip;
    using LiveScore.Core.Services;
    using LiveScore.Soccer.Enumerations;
    using LiveScore.Soccer.ViewModels.DetailOdds.OddItems;
    using Prism.Navigation;
    using Xamarin.Forms;

    internal class DetailOddsViewModel : TabItemViewModelBase
    {
        private readonly IOddsService oddsService;
        private readonly string matchId;

        public DetailOddsViewModel(
            string matchId,
            INavigationService navigationService,
            IDependencyResolver serviceLocator,
            DataTemplate dataTemplate)
            : base(navigationService, serviceLocator, dataTemplate)
        {
            this.matchId = matchId;
            oddsService = DependencyResolver.Resolve<IOddsService>(SettingsService.CurrentSportType.Value);
            RefreshCommand = new DelegateAsyncCommand(async () => await LoadOdds(BetType.OneXTwo, false, true));
        }

        public ObservableCollection<BaseItemViewModel> BetTypeOdds { get; private set; }

        public DelegateAsyncCommand RefreshCommand { get; }

        public bool IsRefreshing { get; set; }

        public bool IsLoading { get; private set; }

        public bool IsNotLoading { get; private set; }

        public bool HasData { get; private set; }

        public bool NoData { get; private set; }

        protected override async void Initialize()
        {
            try
            {
                await LoadOdds(BetType.OneXTwo);
            }
            catch (Exception ex)
            {
                await LoggingService.LogErrorAsync(ex);
            }
        }

        private async Task LoadOdds(BetType betTypeId, bool showLoadingIndicator = true, bool isRefresh = false)
        {
            IsLoading = showLoadingIndicator;

            var odds = await oddsService.GetOdds(matchId, (int)betTypeId, isRefresh);

            if (odds.BetTypeOddsList?.Any() == true)
            {
                BetTypeOdds = new ObservableCollection<BaseItemViewModel>(odds.BetTypeOddsList.Select(t =>
                   new BaseItemViewModel(BetType.OneXTwo, t, NavigationService, DependencyResolver)
                   .CreateInstance()));

                HasData = true;
                NoData = !HasData;
            }
            else
            {
                HasData = false;
                NoData = !HasData;
            }

            IsLoading = false;
            IsNotLoading = true;
            IsRefreshing = false;
        }
    }
}