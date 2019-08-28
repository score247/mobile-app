using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Soccer.Tests")]

namespace LiveScore.Soccer.ViewModels.DetailOdds.OddItems
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using LiveScore.Common.Extensions;
    using LiveScore.Common.LangResources;
    using LiveScore.Core;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Odds;
    using LiveScore.Core.Services;
    using LiveScore.Core.ViewModels;
    using LiveScore.Soccer.Enumerations;
    using LiveScore.Soccer.Models.Odds;
    using MethodTimer;
    using Microsoft.AspNetCore.SignalR.Client;
    using Newtonsoft.Json;
    using Prism.Events;
    using Prism.Navigation;
    using PropertyChanged;
    using Xamarin.Forms;
    
    public class OddsMovementViewModel : ViewModelBase, IDisposable
    {
        private static readonly TimeSpan HubKeepAliveInterval = TimeSpan.FromSeconds(30);

        private string matchId;
        private string oddsFormat;
        private bool disposedValue;

        private MatchStatus eventStatus;
        private Bookmaker bookmaker;
        private BetType betType;
        private CancellationTokenSource cancellationTokenSource;


        private readonly IOddsService oddsService;
        private readonly HubConnection hubConnection;

        public OddsMovementViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator)
            : base(navigationService, dependencyResolver, eventAggregator)
        {
            hubConnection = DependencyResolver
                .Resolve<IHubService>(CurrentSportId.ToString())
                .BuildOddsEventHubConnection();

            oddsService = DependencyResolver.Resolve<IOddsService>(SettingsService.CurrentSportType.Value.ToString());

            RefreshCommand = new DelegateAsyncCommand(async () => await FirstLoadOrRefreshOddsMovement(true));

            OddsMovementItems = new OddsMovementItemViews(CurrentSportName);
            GroupOddsMovementItems = new ObservableCollection<OddsMovementItemViews> { OddsMovementItems };
        }

        public bool IsRefreshing { get; set; }

        public bool HasData { get; private set; }

        public OddsMovementItemViews OddsMovementItems { get; private set; }

        public ObservableCollection<OddsMovementItemViews> GroupOddsMovementItems { get; private set; }

        public DelegateAsyncCommand RefreshCommand { get; }

        public DataTemplate HeaderTemplate { get; private set; }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            try
            {
                matchId = parameters["MatchId"].ToString();
                eventStatus = parameters["EventStatus"] as MatchStatus;
                bookmaker = parameters["Bookmaker"] as Bookmaker;
                betType = (BetType)parameters["BetType"];
                oddsFormat = parameters["Format"].ToString();

                Title = $"{bookmaker.Name} - {AppResources.ResourceManager.GetString(betType.ToString())} Odds";

                HeaderTemplate = new BaseMovementHeaderViewModel(betType, true, NavigationService, DependencyResolver).CreateTemplate();
            }
            catch (Exception ex)
            {
                LoggingService.LogError(ex);
            }
        }

        protected override async void Initialize()
        {
            try
            {
                Debug.WriteLine("OddsMovementViewModel Initialize");

                await FirstLoadOrRefreshOddsMovement();

                hubConnection.On("OddsMovement", (Action<byte, string>)(async (sportId, data) =>
                {
                    Debug.WriteLine($"OddsMovement received {data}");
                    var oddsMovementMessage = await DeserializeOddsMovementMessage(data);

                    if (oddsMovementMessage == null)
                    {
                        return;
                    }

                    await HandleOddsMovementMessage(oddsMovementMessage);
                }));

                await StartOddsHubConnection();
            }
            catch (Exception ex)
            {
                await LoggingService.LogErrorAsync(ex);
            }
        }

        private async Task StartOddsHubConnection()
        {
            if (cancellationTokenSource == null)
            {
                Debug.WriteLine("OddsMovementViewModel StartOddsHubConnection");

                cancellationTokenSource = new CancellationTokenSource();

                await hubConnection.StartWithKeepAlive(HubKeepAliveInterval, LoggingService, cancellationTokenSource.Token);
            }            
        }

        protected override void Clean()
        {
            Debug.WriteLine("OddsMovementViewModel Clean");

            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
                cancellationTokenSource = null;
            }            

            base.Clean();
        }

        public override async void OnResume()
        {
            Debug.WriteLine("OddsMovementViewModel OnResume");

            if (eventStatus == MatchStatus.Live || eventStatus == MatchStatus.NotStarted)
            {
                await GetOddsMovement(isRefresh: true);
            }

            await StartOddsHubConnection();
        }

        [Time]
        private async Task FirstLoadOrRefreshOddsMovement(bool isRefresh = false)
        {
            IsLoading = !isRefresh;
            await GetOddsMovement(true);

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

            var matchOddsMovement = await oddsService.GetOddsMovement(SettingsService.CurrentLanguage, matchId, betType.Value, oddsFormat, bookmaker.Id, forceFetchNew);

            HasData = matchOddsMovement.OddsMovements?.Any() == true;

            if (HasData)
            {
                foreach (var oddsMovement in matchOddsMovement.OddsMovements)
                {
                    var viewModel = new BaseMovementItemViewModel(betType, oddsMovement, NavigationService, DependencyResolver).CreateInstance();

                    OddsMovementItems.Add(viewModel);
                }
            }
        }

        [Time]
        internal async Task<MatchOddsMovementMessage> DeserializeOddsMovementMessage(string message)
        {
            MatchOddsMovementMessage oddsMovementMessage = null;

            try
            {
                oddsMovementMessage = JsonConvert.DeserializeObject<MatchOddsMovementMessage>(message);
            }
            catch (Exception ex)
            {
                await LoggingService.LogErrorAsync("Errors while deserialize MatchOddsMovementMessage", ex);
            }

            return oddsMovementMessage;
        }

        [Time]
        internal async Task HandleOddsMovementMessage(MatchOddsMovementMessage oddsMovementMessage)
        {
            if (oddsMovementMessage.MatchId.Equals(matchId, StringComparison.OrdinalIgnoreCase))
            {
                //TODO check existing odds movement
                var updatedOddsMovements = oddsMovementMessage.OddsEvents
                    .Where(x => x.Bookmaker == bookmaker && x.BetTypeId == betType.Value)
                    .Select(x => x.OddsMovement)
                    .OrderBy(x => x.UpdateTime)
                    .ToList();

                if (updatedOddsMovements.Count > 0)
                {
                    foreach (var oddsMovement in updatedOddsMovements)
                    {
                        var newOddsMovementView = new BaseMovementItemViewModel(betType, oddsMovement, NavigationService, DependencyResolver)
                            .CreateInstance();

                        OddsMovementItems.Insert(0, newOddsMovementView);
                    }

                    await oddsService.GetOddsMovement(SettingsService.CurrentLanguage, matchId, betType.Value, oddsFormat, bookmaker.Id, forceFetchNewData: true);
                }
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

    public class OddsMovementItemViews : ObservableCollection<BaseMovementItemViewModel>
    {
        public OddsMovementItemViews(string heading)
        {
            Heading = heading;
        }

        public string Heading { get; private set; }

        public ObservableCollection<BaseMovementItemViewModel> ItemViews => this;
    }
}