using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Soccer.Tests")]

namespace LiveScore.Soccer.ViewModels.DetailOdds.OddItems
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using LiveScore.Common.Extensions;
    using LiveScore.Common.LangResources;
    using LiveScore.Core;
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
    using Xamarin.Forms;

    public class OddsMovementViewModel : ViewModelBase, IDisposable
    {
        private static readonly TimeSpan HubKeepAliveInterval = TimeSpan.FromSeconds(30);

        private string matchId;      
        private string oddsFormat;
        private bool disposedValue;

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

            OddsMovementItems = new List<BaseMovementItemViewModel>();
            GroupOddsMovementItems = new List<IGrouping<string, BaseMovementItemViewModel>>();
        }

        public bool IsRefreshing { get; set; }

        public bool HasData { get; private set; }

        public List<BaseMovementItemViewModel> OddsMovementItems { get; private set; }

        public IList<IGrouping<string, BaseMovementItemViewModel>> GroupOddsMovementItems { get; private set; }

        public DelegateAsyncCommand RefreshCommand { get; }

        public DataTemplate HeaderTemplate { get; private set; }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            try
            {
                matchId = parameters["MatchId"].ToString();
                bookmaker = parameters["Bookmaker"] as Bookmaker;
                betType = (BetType)parameters["BetType"];
                oddsFormat = parameters["Format"].ToString();

                Title = $"{bookmaker.Name} - {AppResources.ResourceManager.GetString(betType.ToString())} Odds";
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
                await FirstLoadOrRefreshOddsMovement();

                hubConnection.On("OddsMovement", (Action<byte, string>)(async (sportId, data) =>
                {                    
                    var oddsMovementMessage = await DeserializeOddsMovementMessage(data);

                    if (oddsMovementMessage == null)
                    {
                        return;
                    }

                    await HandleOddsMovementMessage(oddsMovementMessage);
                }));

                cancellationTokenSource = new CancellationTokenSource();

                await hubConnection.StartWithKeepAlive(HubKeepAliveInterval, LoggingService, cancellationTokenSource.Token);
            }
            catch (Exception ex)
            {
                await LoggingService.LogErrorAsync(ex);
            }
        }

        protected override void Clean()
        {            
            cancellationTokenSource?.Cancel();

            base.Clean();
        }

        public override async void OnResume()
        {
            //TODO not re-load odds when match is closed
            await GetOddsMovement(isRefresh: true);
        }

        [Time]
        private async Task FirstLoadOrRefreshOddsMovement(bool isRefresh = false)
        {
            if (isRefresh || GroupOddsMovementItems == null || !GroupOddsMovementItems.Any())
            {
                IsLoading = !isRefresh;
                await GetOddsMovement(isRefresh);

                IsRefreshing = false;
                IsLoading = false;
            }
        }

        private async Task GetOddsMovement(bool isRefresh)
        {
            var matchOddsMovement = await oddsService.GetOddsMovement(SettingsService.CurrentLanguage, matchId, betType.Value, oddsFormat, bookmaker.Id, isRefresh);

            HasData = matchOddsMovement.OddsMovements?.Any() == true;

            HeaderTemplate = new BaseMovementHeaderViewModel(betType, HasData, NavigationService, DependencyResolver).CreateTemplate();

            OddsMovementItems = HasData
                ? new List<BaseMovementItemViewModel>(matchOddsMovement.OddsMovements
                    .Select(t => new BaseMovementItemViewModel(betType, t, NavigationService, DependencyResolver).CreateInstance()))
                : new List<BaseMovementItemViewModel>();

            GroupOddsMovementItems = new List<IGrouping<string, BaseMovementItemViewModel>>(OddsMovementItems.GroupBy(item => item.CurrentSportName));
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
                    .Select(x => x.OddsMovement).ToList();

                if (updatedOddsMovements.Count > 0)
                {
                    var newOddsMovementViews = updatedOddsMovements.Select(t =>
                            new BaseMovementItemViewModel(betType, t, NavigationService, DependencyResolver)
                            .CreateInstance());

                    OddsMovementItems.AddRange(newOddsMovementViews);
                    OddsMovementItems = OddsMovementItems.OrderByDescending(x => x.UpdateTime).ToList();

                    GroupOddsMovementItems = new List<IGrouping<string, BaseMovementItemViewModel>>(OddsMovementItems.GroupBy(item => item.CurrentSportName));

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
}