using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Common;
using LiveScore.Common.Extensions;
using LiveScore.Common.LangResources;
using LiveScore.Core.Converters;
using LiveScore.Core.Extensions;
using LiveScore.Core.Models.Matches;
using LiveScore.Core.PubSubEvents.Matches;
using LiveScore.Core.PubSubEvents.Teams;
using LiveScore.Core.Services;
using LiveScore.Core.Views;
using MethodTimer;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Rg.Plugins.Popup.Contracts;
using Device = Xamarin.Forms.Device;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("LiveScore.Tests")]

namespace LiveScore.Core.ViewModels
{
    public abstract class MatchesViewModel : ViewModelBase
    {
        private static readonly string MatchLimitationMessage = string.Format(AppResources.FavoriteMatchLimitation, 99);

        protected readonly IMatchDisplayStatusBuilder matchStatusBuilder;
        protected readonly IMatchMinuteBuilder matchMinuteBuilder;
        protected readonly IMatchService matchService;
        protected readonly Func<string, string> buildFlagUrlFunc;
        protected readonly IFavoriteService<IMatch> favoriteService;
        private readonly IPopupNavigation popupNavigation;

        [Time]
        protected MatchesViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator)
            : base(navigationService, dependencyResolver, eventAggregator)
        {
            IsBusy = true;
            matchService = DependencyResolver.Resolve<IMatchService>(CurrentSportId.ToString());
            matchStatusBuilder = DependencyResolver.Resolve<IMatchDisplayStatusBuilder>(CurrentSportId.ToString());
            matchMinuteBuilder = DependencyResolver.Resolve<IMatchMinuteBuilder>(CurrentSportId.ToString());
            buildFlagUrlFunc = DependencyResolver.Resolve<Func<string, string>>(FuncNameConstants.BuildFlagUrlFuncName);
            popupNavigation = DependencyResolver.Resolve<IPopupNavigation>();
            favoriteService = DependencyResolver.Resolve<IFavoriteService<IMatch>>(CurrentSportId.ToString());
            favoriteService.OnAddedFunc = OnAddedFavorite;
            favoriteService.OnRemovedFunc = OnRemovedFavorite;
            favoriteService.OnReachedLimit = OnReachedLimitation;

            RefreshCommand = new DelegateAsyncCommand(OnRefreshAsync);
            TappedMatchCommand = new DelegateAsyncCommand<MatchViewModel>(OnTapMatchAsync);
        }

        public bool FirstLoad { get; protected set; } = true;

        public bool IsRefreshing { get; set; }

        public object HeaderViewModel { get; protected set; }

        public ObservableCollection<IGrouping<MatchGroupViewModel, MatchViewModel>> MatchItemsSource { get; protected set; }

        public DelegateAsyncCommand RefreshCommand { get; protected set; }

        public DelegateAsyncCommand<MatchViewModel> TappedMatchCommand { get; protected set; }

        public DelegateCommand<IGrouping<MatchGroupViewModel, MatchViewModel>> ScrollToCommand { get; set; }

        public DelegateCommand ScrollToHeaderCommand { get; set; }

        public bool EnableTapLeague { get; protected set; } = true;

        public bool IsHeaderVisible { get; set; }

        public override Task OnNetworkReconnectedAsync()
            => Task.Run(() => LoadDataAsync(LoadMatchesAsync));

        public override void OnSleep() => UnsubscribeAllEvents();

        public override void OnDisappearing()
        {
            base.OnDisappearing();

            UnsubscribeAllEvents();
        }

        protected abstract Task<IEnumerable<IMatch>> LoadMatchesFromServiceAsync();

        protected virtual void SubscribeEvents()
        {
            EventAggregator
                .GetEvent<MatchEventPubSubEvent>()
                .Subscribe(OnReceivedMatchEvent);

            EventAggregator
                .GetEvent<TeamStatisticPubSubEvent>()
                .Subscribe(OnReceivedTeamStatistic);
        }

        protected virtual void UnsubscribeAllEvents()
        {
            EventAggregator
                .GetEvent<MatchEventPubSubEvent>()
                .Unsubscribe(OnReceivedMatchEvent);

            EventAggregator
                .GetEvent<TeamStatisticPubSubEvent>()
                .Unsubscribe(OnReceivedTeamStatistic);
        }

        protected virtual void OnReceivedMatchEvent(IMatchEventMessage payload)
        {
            if (payload?.SportId != CurrentSportId)
            {
                return;
            }

            MatchItemsSource?.UpdateMatchItemEvent(payload.MatchEvent);
        }

        protected virtual void OnReceivedTeamStatistic(ITeamStatisticsMessage payload)
        {
            if (payload.SportId != CurrentSportId)
            {
                return;
            }

            MatchItemsSource?.UpdateMatchItemStatistics(
                payload.MatchId,
                payload.IsHome,
                payload.TeamStatistic);
        }

        protected virtual async Task OnRefreshAsync()
        {
            if (networkConnectionManager.IsFailureConnection())
            {
                IsRefreshing = false;
                networkConnectionManager.PublishNetworkConnectionEvent();
                return;
            }

            await Task.Run(
                () => LoadDataAsync(UpdateMatchesAsync, false))
                .ConfigureAwait(false);
        }

        protected virtual async Task OnTapMatchAsync(MatchViewModel matchItem)
        {
            var parameters = new NavigationParameters
            {
                { "Match", matchItem.Match }
            };

            var navigated = await NavigationService
                .NavigateAsync("MatchDetailView" + CurrentSportId, parameters)
                .ConfigureAwait(false);

            if (!navigated.Success)
            {
                await LoggingService.LogExceptionAsync(navigated.Exception).ConfigureAwait(false);
            }
        }

        protected virtual async Task LoadMatchesAsync()
        {
            var matches = (await LoadMatchesFromServiceAsync())?.ToList();

            if (matches?.Any() != true)
            {
                HasData = false;
                Device.BeginInvokeOnMainThread(() => MatchItemsSource?.Clear());

                return;
            }

            HasData = true;
            InitializeMatchItems(matches);
        }

        protected virtual async Task UpdateMatchesAsync()
        {
            var matches = (await LoadMatchesFromServiceAsync())?.ToList();

            if (matches?.Any() != true)
            {
                HasData = false;
                IsRefreshing = false;

                Device.BeginInvokeOnMainThread(() => MatchItemsSource?.Clear());
                return;
            }

            UpdateMatchItems(matches);
            IsRefreshing = false;
            HasData = true;

            Device.BeginInvokeOnMainThread(RecheckFavoriteMatchItems);
        }

        protected virtual void InitializeMatchItems(IEnumerable<IMatch> matches)
        {
            var matchItemViewModels = matches.Select(match => new MatchViewModel(
                match,
                matchStatusBuilder,
                matchMinuteBuilder,
                EventAggregator,
                favoriteService));

            var matchItems
                = matchItemViewModels.GroupBy(item => new MatchGroupViewModel(item.Match, buildFlagUrlFunc, NavigationService, CurrentSportId, EnableTapLeague));

            Device.BeginInvokeOnMainThread(()
                => MatchItemsSource = new ObservableCollection<IGrouping<MatchGroupViewModel, MatchViewModel>>(matchItems));
        }

        protected virtual void UpdateMatchItems(IEnumerable<IMatch> matches) => InitializeMatchItems(matches);

        protected virtual void RecheckFavoriteMatchItems()
        {
            var matchItems = MatchItemsSource
                .SelectMany(group => group)
                .Where(item => item.EnableFavorite);

            foreach (var matchItem in matchItems)
            {
                matchItem.RecheckFavorite();
            }
        }

        protected virtual Task OnAddedFavorite()
            => popupNavigation.PushAsync(new FavoritePopupView(AppResources.AddedFavorite));

        protected virtual Task OnRemovedFavorite(IMatch match)
            => popupNavigation.PushAsync(new FavoritePopupView(AppResources.RemovedFavorite));

        protected virtual Task OnReachedLimitation()
            => popupNavigation.PushAsync(new FavoritePopupView(MatchLimitationMessage));
    }
}