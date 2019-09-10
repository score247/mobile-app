using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Soccer.Tests")]

namespace LiveScore.Soccer.ViewModels.MatchDetails.DetailOdds
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.LangResources;
    using Core;
    using Enumerations;
    using LiveScore.Common.Extensions;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Odds;
    using LiveScore.Core.PubSubEvents.Odds;
    using LiveScore.Core.Services;
    using LiveScore.Core.ViewModels;
    using MethodTimer;
    using OddItems;
    using Prism.Events;
    using Prism.Navigation;
    using Xamarin.Forms;

    public class OddsMovementViewModel : ViewModelBase, IDisposable
    {
        private string matchId;
        private string oddsFormat;
        private bool disposedValue;

        private MatchStatus eventStatus;
        private Bookmaker bookmaker;
        private BetType betType;

        private readonly IOddsService oddsService;
        private readonly IEventAggregator eventAggregator;

        public OddsMovementViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator)
            : base(navigationService, dependencyResolver, eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            oddsService = DependencyResolver.Resolve<IOddsService>(CurrentSportId.ToString());

            RefreshCommand = new DelegateAsyncCommand(async () => await FirstLoadOrRefreshOddsMovement(true).ConfigureAwait(false));

            OddsMovementItems = new OddsMovementObservableCollection(CurrentSportName);
            GroupOddsMovementItems = new ObservableCollection<OddsMovementObservableCollection> { OddsMovementItems };

            eventAggregator.GetEvent<OddsMovementPubSubEvent>().Subscribe(HandleOddsMovementMessage, ThreadOption.UIThread);
        }

        public bool IsRefreshing { get; set; }

        public bool HasData { get; private set; }

        public OddsMovementObservableCollection OddsMovementItems { get; private set; }

        public ObservableCollection<OddsMovementObservableCollection> GroupOddsMovementItems { get; private set; }

        public DelegateAsyncCommand RefreshCommand { get; }

        public DataTemplate HeaderTemplate { get; private set; }

        public override void Initialize(INavigationParameters parameters)
        {
            try
            {
                matchId = parameters["MatchId"].ToString();
                eventStatus = parameters["EventStatus"] as MatchStatus;
                bookmaker = parameters["Bookmaker"] as Bookmaker;
                betType = (BetType)parameters["BetType"];
                oddsFormat = parameters["Format"].ToString();

                Title = $"{bookmaker?.Name} - {AppResources.ResourceManager.GetString(betType.ToString())} Odds";

                HeaderTemplate = new BaseMovementHeaderViewModel(betType, true, NavigationService, DependencyResolver).CreateTemplate();
            }
            catch (Exception ex)
            {
                LoggingService.LogError(ex);
            }
        }

        protected async Task OnInitialized()
        {
            try
            {
                Debug.WriteLine("OddsMovementViewModel Initialize");

                await FirstLoadOrRefreshOddsMovement().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await LoggingService.LogErrorAsync(ex).ConfigureAwait(false);
            }
        }

        protected void OnDisposed()
        {
            Debug.WriteLine("OddsMovementViewModel Clean");

            eventAggregator.GetEvent<OddsMovementPubSubEvent>().Unsubscribe(HandleOddsMovementMessage);

            if (eventStatus == MatchStatus.Live || eventStatus == MatchStatus.NotStarted)
            {
                OddsMovementItems.Clear();
            }
        }

        public override async void OnResume()
        {
            Debug.WriteLine("OddsMovementViewModel OnResume");

            if (eventStatus == MatchStatus.Live || eventStatus == MatchStatus.NotStarted)
            {
                await GetOddsMovement(isRefresh: true).ConfigureAwait(false);
            }

            eventAggregator.GetEvent<OddsMovementPubSubEvent>().Subscribe(HandleOddsMovementMessage, ThreadOption.UIThread);
        }

        [Time]
        private async Task FirstLoadOrRefreshOddsMovement(bool isRefresh = false)
        {
            IsLoading = !isRefresh;
            await GetOddsMovement(isRefresh).ConfigureAwait(false);

            IsRefreshing = false;
            IsLoading = false;
        }

        private async Task GetOddsMovement(bool isRefresh)
        {
            var forceFetchNew = isRefresh || (eventStatus == MatchStatus.NotStarted || eventStatus == MatchStatus.Live);

            if (forceFetchNew)
            {
                OddsMovementItems.Clear();
            }

            var matchOddsMovement = await oddsService.GetOddsMovement(base.CurrentLanguage.Value.ToString(), matchId, betType.Value, oddsFormat, bookmaker.Id, forceFetchNew).ConfigureAwait(false);

            HasData = matchOddsMovement.OddsMovements?.Any() == true;

            if (HasData && matchOddsMovement.OddsMovements != null)
            {
                foreach (var oddsMovement in matchOddsMovement.OddsMovements)
                {
                    var viewModel =
                        new BaseMovementItemViewModel(betType, oddsMovement, NavigationService, DependencyResolver)
                            .CreateInstance();

                    OddsMovementItems.Add(viewModel);
                }
            }
        }

        [Time]
        internal void HandleOddsMovementMessage(IOddsMovementMessage oddsMovementMessage)
        {
            if (!oddsMovementMessage.MatchId.Equals(matchId, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            //TODO check existing odds movement
            var updatedOddsMovements = oddsMovementMessage.OddsEvents
                .Where(x => x.Bookmaker == bookmaker && x.BetTypeId == betType.Value)
                .Select(x => x.OddsMovement)
                .OrderBy(x => x.UpdateTime)
                .ToList();

            if (updatedOddsMovements.Count == 0)
            {
                return;
            }

            foreach (var oddsMovement in updatedOddsMovements)
            {
                var newOddsMovementView 
                    = new BaseMovementItemViewModel(betType, oddsMovement, NavigationService, DependencyResolver)
                        .CreateInstance();

                OddsMovementItems.Insert(0, newOddsMovementView);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                // Not use dispose method because of keeping long using object, handling object is implemented in Clean()
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}