using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LiveScore.Core;
using LiveScore.Core.Controls.TabStrip;
using LiveScore.Core.Models.Leagues;
using LiveScore.Core.NavigationParams;
using LiveScore.Core.Services;
using LiveScore.Core.ViewModels;
using LiveScore.Soccer.Models.Leagues;
using LiveScore.Soccer.ViewModels.Leagues.LeagueDetails.Fixtures;
using LiveScore.Soccer.ViewModels.Leagues.LeagueDetails.Table;
using PanCardView.EventArgs;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Xamarin.Forms;

namespace LiveScore.Soccer.ViewModels.Leagues
{
    public class LeagueDetailViewModel : ViewModelBase, IDisposable
    {
        private const string InactiveFavoriteImageSource = "images/common/inactive_favorite_header_bar.png";
        private const string ActiveFavoriteImageSource = "images/common/active_favorite_header_bar.png";
        private readonly IFavoriteService<ILeague> favoriteService;
        private League currentLeague;
        private bool disposed;
        private bool firstLoad = true;

        public LeagueDetailViewModel(
             INavigationService navigationService,
             IDependencyResolver dependencyResolver,
             IEventAggregator eventAggregator) : base(navigationService, dependencyResolver, eventAggregator)
        {
            favoriteService = DependencyResolver.Resolve<IFavoriteService<ILeague>>(CurrentSportId.ToString());
            ItemAppearedCommand = new DelegateCommand<ItemAppearedEventArgs>(OnItemAppeared);
            ItemDisappearingCommand = new DelegateCommand<ItemDisappearingEventArgs>(OnItemDisappearing);

            FavoriteCommand = new DelegateCommand(OnFavorite);
            favoriteService.OnReachLimitFavoriteItems += HandleReachedLimitation;
        }

        public byte SelectedIndex { get; set; }

        public bool IsFavorite { get; private set; }

        public string FavoriteImageSource => IsFavorite ? ActiveFavoriteImageSource : InactiveFavoriteImageSource;

        public IReadOnlyList<ViewModelBase> LeagueDetailItemSources { get; private set; }

        public DelegateCommand<ItemAppearedEventArgs> ItemAppearedCommand { get; }

        public DelegateCommand<ItemDisappearingEventArgs> ItemDisappearingCommand { get; }

        public DelegateCommand FavoriteCommand { get; }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            try
            {
                if (!(parameters["League"] is LeagueDetailNavigationParameter leagueParameter))
                {
                    return;
                }

                Task.Run(async () =>
                {
                    BuildLeagueDetailTabs(parameters, leagueParameter);

                    await Task.Delay(200).ContinueWith(_ => LoadSelectedTab());
                });
            }
            catch (Exception ex)
            {
                LoggingService.LogException(ex);
            }
        }

        private void BuildLeagueDetailTabs(INavigationParameters parameters, LeagueDetailNavigationParameter leagueParameter)
        {
            var countryFlag = parameters["CountryFlag"]?.ToString();
            var homeId = parameters["HomeId"]?.ToString();
            var awayId = parameters["AwayId"]?.ToString();

            currentLeague = new League(
                leagueParameter.Id,
                leagueParameter.LeagueGroupName,
                leagueParameter.Order,
                null,
                null,
                leagueParameter.CountryCode,
                leagueParameter.IsInternational,
                null,
                leagueParameter.RoundGroup,
                leagueParameter.SeasonId,
                leagueParameter.HasGroups,
                leagueParameter.HasStandings);

            var fixtureTab = new FixturesViewModel(NavigationService, DependencyResolver, EventAggregator, leagueParameter);

            Device.BeginInvokeOnMainThread(() => LeagueDetailItemSources = leagueParameter.HasStandings
                ? new List<ViewModelBase> { new TableViewModel(
                    NavigationService,
                    DependencyResolver,
                    leagueParameter,
                    null,
                    countryFlag,
                    homeId,
                    awayId), fixtureTab }
                : new List<ViewModelBase> { fixtureTab });
        }

        public override Task OnNetworkReconnectedAsync() => LeagueDetailItemSources[SelectedIndex]?.OnNetworkReconnectedAsync();

        public override void OnResumeWhenNetworkOK()
        {
            base.OnResumeWhenNetworkOK();

            LeagueDetailItemSources[SelectedIndex]?.OnResumeWhenNetworkOK();
        }

        public override void OnSleep()
        {
            base.OnSleep();

            LeagueDetailItemSources[SelectedIndex]?.OnSleep();
        }

        public override void OnAppearing()
        {
            base.OnAppearing();

            if (!firstLoad)
            {
                LoadSelectedTab();
            }

            firstLoad = false;

            IsFavorite = favoriteService.IsFavorite(currentLeague);
        }

        private void LoadSelectedTab()
        {
            if (LeagueDetailItemSources[SelectedIndex] is TabItemViewModel selectedItem)
            {
                selectedItem.IsActive = true;
                selectedItem.OnAppearing();
            }
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();

            LeagueDetailItemSources[SelectedIndex]?.OnDisappearing();
        }

        public override void Destroy()
        {
            base.Destroy();

            foreach (var item in LeagueDetailItemSources)
            {
                item.Destroy();
            }

            Dispose();
        }

        private void OnItemAppeared(ItemAppearedEventArgs args)
        {
            if (LeagueDetailItemSources?[SelectedIndex] is TabItemViewModel selectedItem)
            {
                selectedItem.IsActive = true;
                selectedItem.OnAppearing();
            }
        }

        private void OnItemDisappearing(ItemDisappearingEventArgs args)
        {
            if (args?.Index >= 0 && LeagueDetailItemSources?[args.Index] is TabItemViewModel previousItem)
            {
                previousItem.IsActive = false;
                previousItem.OnDisappearing();
            }
        }

        private void OnFavorite()
        {
            if (IsFavorite)
            {
                favoriteService.Remove(currentLeague);
            }
            else
            {
                favoriteService.Add(currentLeague);
            }

            IsFavorite = !IsFavorite;
        }

        private Task HandleReachedLimitation()
        {
            IsFavorite = false;
            return Task.CompletedTask;
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
                LeagueDetailItemSources = null;
            }

            disposed = true;
        }
    }
}