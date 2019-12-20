﻿using System.Collections.Generic;
using System.Threading.Tasks;
using LiveScore.Core;
using LiveScore.Core.Controls.TabStrip;
using LiveScore.Core.Models.Leagues;
using LiveScore.Core.Services;
using LiveScore.Core.ViewModels;
using LiveScore.Soccer.ViewModels.Leagues.LeagueDetails.Fixtures;
using LiveScore.Soccer.ViewModels.Leagues.LeagueDetails.Table;
using PanCardView.EventArgs;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;

namespace LiveScore.Soccer.ViewModels.Leagues
{
    public class LeagueDetailViewModel : ViewModelBase
    {
        private const string InactiveFavoriteImageSource = "images/common/inactive_favorite_header_bar.png";
        private const string ActiveFavoriteImageSource = "images/common/active_favorite_header_bar.png";

        private readonly IFavoriteService favoriteService;

        private string LeagueId;
        private string LeagueGroupName;

        public LeagueDetailViewModel(
         INavigationService navigationService,
         IDependencyResolver dependencyResolver,
         IEventAggregator eventAggregator,
         IFavoriteService favoriteService)
         : base(navigationService, dependencyResolver, eventAggregator)
        {
            this.favoriteService = favoriteService;

            ItemAppearedCommand = new DelegateCommand<ItemAppearedEventArgs>(OnItemAppeared);
            ItemDisappearingCommand = new DelegateCommand<ItemDisappearingEventArgs>(OnItemDisappearing);

            FavoriteCommand = new DelegateCommand(OnFavorite);
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

            LeagueId = parameters["LeagueId"]?.ToString();
            LeagueGroupName = parameters["LeagueGroupName"]?.ToString();

            IsFavorite = favoriteService.IsFavoriteLeague(LeagueId);

            var leagueSeasonId = parameters["LeagueSeasonId"]?.ToString();
            var leagueRoundGroup = parameters["LeagueRoundGroup"]?.ToString();            
            var countryFlag = parameters["CountryFlag"]?.ToString();
            var homeId = parameters["HomeId"]?.ToString();
            var awayId = parameters["AwayId"]?.ToString();

            LeagueDetailItemSources = new List<ViewModelBase> {
                new TableViewModel(LeagueId, leagueSeasonId, leagueRoundGroup, NavigationService, DependencyResolver, null, LeagueGroupName, countryFlag, homeId, awayId, false),
                new FixturesViewModel(LeagueId, LeagueGroupName, NavigationService, DependencyResolver, EventAggregator)
            };
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

        private void OnItemAppeared(ItemAppearedEventArgs args)
        {
            if (LeagueDetailItemSources[SelectedIndex] is TabItemViewModel selectedItem)
            {
                selectedItem.IsActive = true;
                selectedItem.OnAppearing();
            }
        }

        private void OnItemDisappearing(ItemDisappearingEventArgs args)
        {
            if (args.Index >= 0 && LeagueDetailItemSources[args.Index] is TabItemViewModel previousItem)
            {
                previousItem.IsActive = false;
                previousItem.OnDisappearing();
            }
        }

        private void OnFavorite()
        {
            //TODO add CountryCode
            if (IsFavorite)
            {
                favoriteService.RemoveLeague(new FavoriteLeague(LeagueId, LeagueGroupName, string.Empty));
            }
            else
            {
                favoriteService.AddLeague(new FavoriteLeague(LeagueId, LeagueGroupName, string.Empty));
            }

            IsFavorite = !IsFavorite;
        }
    }
}