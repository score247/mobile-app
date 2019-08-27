﻿using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Soccer.Tests")]

namespace LiveScore.Soccer.ViewModels.DetailOdds
{
    using LiveScore.Common.Extensions;
    using LiveScore.Core;
    using LiveScore.Core.Controls.TabStrip;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Odds;
    using LiveScore.Core.Services;
    using LiveScore.Soccer.Enumerations;
    using LiveScore.Soccer.Models.Odds;
    using LiveScore.Soccer.ViewModels.DetailOdds.OddItems;
    using MethodTimer;
    using Microsoft.AspNetCore.SignalR.Client;
    using Newtonsoft.Json;
    using Prism.Navigation;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Xamarin.Forms;

    internal class DetailOddsViewModel : TabItemViewModelBase, IDisposable
    {
        private static readonly TimeSpan HubKeepAliveInterval = TimeSpan.FromSeconds(30);

        private readonly string matchId;
        private readonly string oddsFormat;
        private readonly MatchStatus eventStatus;

        private readonly IOddsService oddsService;
        private readonly HubConnection hubConnection;

        private bool disposedValue;
        private CancellationTokenSource cancellationTokenSource;

        public DetailOddsViewModel(
            string matchId,
            MatchStatus eventStatus,
            INavigationService navigationService,
            IDependencyResolver serviceLocator,
            DataTemplate dataTemplate)
            : base(navigationService, serviceLocator, dataTemplate)
        {
            this.matchId = matchId;
            this.eventStatus = eventStatus;

            oddsFormat = OddsFormat.Decimal.DisplayName;
            SelectedBetType = BetType.AsianHDP;
            IsRefreshing = false;

            hubConnection = DependencyResolver
                .Resolve<IHubService>(CurrentSportId.ToString())
                .BuildOddsEventHubConnection();

            oddsService = DependencyResolver.Resolve<IOddsService>(SettingsService.CurrentSportType.Value.ToString());

            RefreshCommand = new DelegateAsyncCommand(async () => await LoadData(() => LoadOdds(SelectedBetType, oddsFormat, true)));

            OnOddsTabClicked = new DelegateAsyncCommand<string>(HandleButtonCommand);

            TappedOddsItemCommand = new DelegateAsyncCommand<BaseItemViewModel>(HandleOddsItemTapCommand);

            TabHeaderIcon = TabDetailImages.Odds;
            TabHeaderActiveIcon = TabDetailImages.OddsActive;

            BetTypeOddsItems = new List<BaseItemViewModel>();
        }

        public IList<BaseItemViewModel> BetTypeOddsItems { get; private set; }

        public DataTemplate HeaderTemplate { get; private set; }

        public DelegateAsyncCommand RefreshCommand { get; }

        public DelegateAsyncCommand<string> OnOddsTabClicked { get; }

        public DelegateAsyncCommand<BaseItemViewModel> TappedOddsItemCommand { get; }

        public bool IsRefreshing { get; set; }

        public bool HasData { get; private set; }

        public BetType SelectedBetType { get; private set; }

        public bool IsOneXTwoSelected => SelectedBetType == BetType.OneXTwo;

        public bool IsAsianHdpSelected => SelectedBetType == BetType.AsianHDP;

        public bool IsOverUnderSelected => SelectedBetType == BetType.OverUnder;

        private Task HandleButtonCommand(string betTypeId)
        => LoadOdds(Enumeration.FromValue<BetType>(Byte.Parse(betTypeId)), oddsFormat);


        private async Task HandleOddsItemTapCommand(BaseItemViewModel item)
        {
            var parameters = new NavigationParameters
            {
                { "MatchId", matchId },
                { "EventStatus", eventStatus },
                { "Bookmaker", item.BetTypeOdds.Bookmaker},
                { "BetType", SelectedBetType },
                { "Format",  oddsFormat}
            };

            var navigated = await NavigationService.NavigateAsync("OddsMovementView" + SettingsService.CurrentSportType.Value, parameters);

            if (!navigated.Success)
            {
                await LoggingService.LogErrorAsync(navigated.Exception);
            }
        }

        [Time]
        protected override async void Initialize()
        {
            try
            {
                Debug.WriteLine("DetailOddsViewModel Initialize");

                await LoadData(() => LoadOdds(SelectedBetType, oddsFormat, IsRefreshing));

                if (cancellationTokenSource == null)
                {
                    hubConnection.On("OddsComparison", (Action<byte, string>)(async (sportId, data) =>
                    {
                        Debug.WriteLine($"OddsComparison received {data}");
                        var oddsComparisonMessage = await DeserializeComparisonMessage(data);

                        if (oddsComparisonMessage == null)
                        {
                            return;
                        }

                        await HandleOddsComparisonMessage(oddsComparisonMessage);
                    }));

                    await StartOddsHubConnection();
                }                
            }
            catch (Exception ex)
            {
                await LoggingService.LogErrorAsync(ex);
            }
        }

        public override void OnDisappearing()
        {
            Debug.WriteLine("DetailOddsViewModel OnDisappearing");
        }

        public override async void OnResume()
        {
            Debug.WriteLine("DetailOddsViewModel OnResume");
            
            if (eventStatus == MatchStatus.NotStarted || eventStatus == MatchStatus.Live)
            {
                await LoadOddsByBetType(oddsFormat, isRefresh: true);
            }            
        }

        public override void OnSleep()
        {
            Debug.WriteLine("DetailOddsViewModel OnSleep");
            cancellationTokenSource?.Cancel();
            cancellationTokenSource = null;
        }

        public override void Destroy()
        {
            Debug.WriteLine("DetailOddsViewModel Destroy");
            cancellationTokenSource?.Cancel();
            cancellationTokenSource = null;
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            Debug.WriteLine("DetailOddsViewModel OnNavigatedFrom");
            base.OnNavigatedFrom(parameters);
        }

        private async Task StartOddsHubConnection()
        {
            Debug.WriteLine("DetailOddsViewModel StartOddsHubConnection");

            cancellationTokenSource = new CancellationTokenSource();

            await hubConnection.StartWithKeepAlive(HubKeepAliveInterval, LoggingService, cancellationTokenSource.Token);
        }

        [Time]
        private async Task LoadOdds(BetType betType, string formatType, bool isRefresh = false)
        {
            if (CanLoadOdds(betType, isRefresh))
            {
                IsLoading = !isRefresh;

                SelectedBetType = betType;
                await LoadOddsByBetType(formatType, isRefresh);

                IsRefreshing = false;
                IsLoading = false;
            }
        }

        private async Task LoadOddsByBetType(string formatType, bool isRefresh)
        {
            var forceFetchNew = isRefresh || (eventStatus == MatchStatus.NotStarted || eventStatus == MatchStatus.Live);

            var odds = await oddsService.GetOdds(SettingsService.CurrentLanguage, matchId, SelectedBetType.Value, formatType, forceFetchNew);

            HasData = odds.BetTypeOddsList?.Any() == true;

            HeaderTemplate = new BaseHeaderViewModel(SelectedBetType, HasData, NavigationService, DependencyResolver).CreateTemplate();

            BetTypeOddsItems = HasData
                ? new List<BaseItemViewModel>(odds.BetTypeOddsList.Select(t =>
                    new BaseItemViewModel(SelectedBetType, t, NavigationService, DependencyResolver)
                    .CreateInstance()))
                : new List<BaseItemViewModel>();
        }

        private bool CanLoadOdds(BetType betType, bool isRefresh)
            => isRefresh ||
            SelectedBetType != betType ||
            BetTypeOddsItems == null ||
            !BetTypeOddsItems.Any();

        [Time]
        internal async Task<MatchOddsComparisonMessage> DeserializeComparisonMessage(string message)
        {
            MatchOddsComparisonMessage oddsComparisonMessage = null;

            try
            {
                oddsComparisonMessage = JsonConvert.DeserializeObject<MatchOddsComparisonMessage>(message);
            }
            catch (Exception ex)
            {
                await LoggingService.LogErrorAsync("Errors while deserialize MatchOddsComparisonMessage", ex);
            }

            return oddsComparisonMessage;
        }

        [Time]
        internal async Task HandleOddsComparisonMessage(MatchOddsComparisonMessage oddsComparisonMessage)
        {
            if (oddsComparisonMessage.MatchId.Equals(matchId, StringComparison.OrdinalIgnoreCase) &&
                oddsComparisonMessage.BetTypeOddsList != null &&
                oddsComparisonMessage.BetTypeOddsList.Any(x => x.Id == SelectedBetType.Value))
            {
                var needToReOrder = false;
                var updatedBetTypeOdds = oddsComparisonMessage.BetTypeOddsList.Where(x => x.Id == SelectedBetType.Value);

                foreach (var updatedOdds in updatedBetTypeOdds)
                {
                    var existingOddsItem = BetTypeOddsItems.FirstOrDefault(x => x.BetTypeOdds.Bookmaker == updatedOdds.Bookmaker);

                    if (existingOddsItem == null)
                    {
                        AddBookmakerOdds(updatedOdds);

                        needToReOrder = true;
                    }
                    else
                    {
                        existingOddsItem.UpdateOdds(updatedOdds);
                    }
                }

                if (needToReOrder)
                {
                    BetTypeOddsItems = new ObservableCollection<BaseItemViewModel>(BetTypeOddsItems.OrderBy(x => x.BetTypeOdds.Bookmaker.Name));
                }
            }
                             
            await UpdateOddsCache(oddsComparisonMessage);            
        }

        private async Task UpdateOddsCache(MatchOddsComparisonMessage oddsComparisonMessage)
        {            
            if (oddsComparisonMessage.BetTypeOddsList != null &&
                oddsComparisonMessage.BetTypeOddsList.Any())
            {
                if (oddsComparisonMessage.MatchId.Equals(matchId, StringComparison.OrdinalIgnoreCase))
                {
                    var betTypes = oddsComparisonMessage.BetTypeOddsList.Select(x => x.Id);

                    foreach (var betTypeId in betTypes)
                    {                        
                        await oddsService.GetOdds(SettingsService.CurrentLanguage, matchId, betTypeId, oddsFormat, forceFetchNewData: true);
                    }
                }                
            }
        }

        private void AddBookmakerOdds(IBetTypeOdds updatedOdds)
        {
            HasData = true;
            var newOddsItemViewModel = new BaseItemViewModel(SelectedBetType, updatedOdds, NavigationService, DependencyResolver).CreateInstance();

            if (!BetTypeOddsItems.Any())
            {
                HeaderTemplate = new BaseHeaderViewModel(SelectedBetType, HasData, NavigationService, DependencyResolver).CreateTemplate();
            }

            BetTypeOddsItems.Add(newOddsItemViewModel);
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