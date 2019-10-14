using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Soccer.Tests")]

namespace LiveScore.Soccer.ViewModels.MatchDetails.DetailOdds
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.LangResources;
    using Core;
    using Enumerations;
    using LiveScore.Common.Extensions;
    using LiveScore.Common.Services;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Odds;
    using LiveScore.Core.ViewModels;
    using LiveScore.Soccer.Models.Odds;
    using LiveScore.Soccer.PubSubEvents.Odds;
    using LiveScore.Soccer.Services;
    using MethodTimer;
    using OddItems;
    using Prism.Events;
    using Prism.Navigation;
    using Xamarin.Forms;

    public class OddsMovementViewModel : ViewModelBase
    {
        private string matchId;
        private string oddsFormat;
        private bool isFirstLoad = true;

        private MatchStatus eventStatus;
        private Bookmaker bookmaker;
        private BetType betType;

        private readonly IOddsService oddsService;

        [Time]
        public OddsMovementViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator)
            : base(navigationService, dependencyResolver, eventAggregator)
        {      
            oddsService = DependencyResolver.Resolve<IOddsService>(CurrentSportId.ToString());
            
            OddsMovementItems = new OddsMovementObservableCollection(CurrentSportName);
            GroupOddsMovementItems = new ObservableCollection<OddsMovementObservableCollection> { OddsMovementItems };
        }

        public bool IsRefreshing { get; set; }

        public OddsMovementObservableCollection OddsMovementItems { get; private set; }

        public ObservableCollection<OddsMovementObservableCollection> GroupOddsMovementItems { get; private set; }

        public DelegateAsyncCommand RefreshCommand
            => new DelegateAsyncCommand(async () => await FirstLoadOrRefreshOddsMovement(true).ConfigureAwait(false));

        public DataTemplate HeaderTemplate { get; private set; }

        [Time]
        public override void Initialize(INavigationParameters parameters)
        {
            try
            {
                Debug.WriteLine("OddsMovementViewModel Initialize");

                matchId = parameters["MatchId"].ToString();
                eventStatus = parameters["EventStatus"] as MatchStatus;
                bookmaker = parameters["Bookmaker"] as Bookmaker;
                betType = (BetType)parameters["BetType"];
                oddsFormat = parameters["Format"].ToString();

                Title = $"{bookmaker?.Name} - {AppResources.ResourceManager.GetString(betType.ToString())} Odds".ToUpperInvariant();

                HeaderTemplate = new BaseMovementHeaderViewModel(betType, true, NavigationService, DependencyResolver).CreateTemplate();
            }
            catch (Exception ex)
            {
                LoggingService.LogException(ex);
            }
        }

        public override async void OnAppearing()
        {
            try
            {
                Debug.WriteLine("OddsMovementViewModel OnAppearing");

                if (isFirstLoad)
                {
                    await LoadDataAsync(async () => await FirstLoadOrRefreshOddsMovement());
                    isFirstLoad = false;
                }

                SubscribeEvents();
            }
            catch (Exception ex)
            {
                await LoggingService.LogExceptionAsync(ex).ConfigureAwait(false);
            }
        }

        public override async void OnResumeWhenNetworkOK()
        {
            await ReloadPage();

            SubscribeEvents();
        }

        private async Task ReloadPage()
        {
            Debug.WriteLine("OddsMovementViewModel OnResumeWhenNetworkOK");

            if (eventStatus == MatchStatus.Live || eventStatus == MatchStatus.NotStarted)
            {
                await GetOddsMovement(isRefresh: true).ConfigureAwait(false);
            }
        }

        public override void OnSleep()
        {
            Debug.WriteLine("OddsMovementViewModel OnSleep");

            UnsubscribeEvents();
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();

            Debug.WriteLine("OddsMovementViewModel OnDisappearing");

            UnsubscribeEvents();
        }

        private void SubscribeEvents()
        {
            if (EventAggregator != null)
            {
                return;
            }

            EventAggregator
                .GetEvent<OddsMovementPubSubEvent>()
                .Subscribe(HandleOddsMovementMessage);

            EventAggregator
                .GetEvent<ConnectionChangePubSubEvent>()
                .Subscribe(OnConnectionChangedBase);
        }

        private void UnsubscribeEvents()
        {
            if (EventAggregator != null)
            {
                return;
            }

            EventAggregator
                .GetEvent<ConnectionChangePubSubEvent>()
                .Unsubscribe(OnConnectionChangedBase);

            EventAggregator
                .GetEvent<OddsMovementPubSubEvent>()
                .Unsubscribe(HandleOddsMovementMessage);
        }

        [Time]
        private async Task FirstLoadOrRefreshOddsMovement(bool isRefresh = false)
        {
            IsBusy = !isRefresh;
            await GetOddsMovement(isRefresh).ConfigureAwait(false);

            IsRefreshing = false;
            IsBusy = false;
        }

        [Time]
        private async Task GetOddsMovement(bool isRefresh)
        {
            var forceFetchNew = isRefresh || (eventStatus == MatchStatus.NotStarted || eventStatus == MatchStatus.Live);

            var matchOddsMovement = await oddsService
                .GetOddsMovement(CurrentLanguage.DisplayName, matchId, betType.Value, oddsFormat, bookmaker.Id, forceFetchNew)
                .ConfigureAwait(false);

            if (matchOddsMovement.OddsMovements != null && matchOddsMovement.OddsMovements?.Any() == true)
            {
                var views = matchOddsMovement.OddsMovements
                    .Select(x => new BaseMovementItemViewModel(betType, x, NavigationService, DependencyResolver)
                    .CreateInstance());

                if (isRefresh)
                {
                    OddsMovementItems.Clear();
                }

                foreach (var view in views)
                {
                    OddsMovementItems.Add(view);
                }
            }

            HasData = true;
        }

        [Time]
        internal void HandleOddsMovementMessage(OddsMovementMessage oddsMovementMessage)
        {
            Debug.WriteLine($"HandleOddsMovementMessage {oddsMovementMessage.MatchId}");

            if (!oddsMovementMessage.MatchId.Equals(matchId, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            InsertOddsMovementItems(oddsMovementMessage.OddsEvents
                .Where(x => x.Bookmaker == bookmaker && x.BetTypeId == betType.Value)
                .Select(x => x.OddsMovement));
        }

        private void InsertOddsMovementItems(IEnumerable<IOddsMovement> updatedOddsMovements)
        {
            var exsitingOddsMovements = OddsMovementItems.Select(x => x.OddsMovement);

            var distinctOddsMovements = updatedOddsMovements
                .Except(exsitingOddsMovements)
                .OrderBy(x => x.UpdateTime)
                .ToList();

            foreach (var oddsMovement in distinctOddsMovements)
            {
                var newOddsMovementView = new BaseMovementItemViewModel(betType, oddsMovement, NavigationService, DependencyResolver)
                        .CreateInstance();

                OddsMovementItems.Insert(0, newOddsMovementView);
            }
        }

#pragma warning disable S2325 // Methods and properties that don't access instance data should be static

        private async void OnConnectionChangedBase(bool isConnected)
#pragma warning restore S2325 // Methods and properties that don't access instance data should be static
        {
            if (isConnected)
            {
                Debug.WriteLine("OddsMovementViewModel OnConnectionChangedBase");

                await RefreshCommand.ExecuteAsync();
            }
        }
    }
}