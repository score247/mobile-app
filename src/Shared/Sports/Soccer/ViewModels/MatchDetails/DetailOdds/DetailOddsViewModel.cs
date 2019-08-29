using System.Runtime.CompilerServices;

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
    using LiveScore.Soccer.PubSubEvents.Odds;
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

    internal class DetailOddsViewModel : TabItemViewModel, IDisposable
    {
        private static readonly TimeSpan HubKeepAliveInterval = TimeSpan.FromSeconds(30);

        private readonly string matchId;
        private readonly string oddsFormat;
        private readonly MatchStatus eventStatus;
        private readonly IOddsService oddsService;

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

            oddsService = DependencyResolver.Resolve<IOddsService>(SettingsService.CurrentSportType.Value.ToString());

            RefreshCommand = new DelegateAsyncCommand(async () => await LoadData(() => FirstLoadOrRefreshOdds(SelectedBetType, oddsFormat, true)));

            OnOddsTabClicked = new DelegateAsyncCommand<string>(async (betTypeId) => await FirstLoadOrRefreshOdds(Enumeration.FromValue<BetType>(Byte.Parse(betTypeId)), oddsFormat));

            TappedOddsItemCommand = new DelegateAsyncCommand<BaseItemViewModel>(HandleOddsItemTapCommand);

            TabHeaderIcon = MatchDetailTabImage.Odds;
            TabHeaderActiveIcon = MatchDetailTabImage.OddsActive;

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

        private Action<byte, string> OddsComparisonHandler()
        {
            return async (sportId, data) =>
            {
                Debug.WriteLine($"OddsComparison received {data}");
                var oddsComparisonMessage = await DeserializeComparisonMessage(data);

                if (oddsComparisonMessage == null)
                {
                    return;
                }

                await HandleOddsComparisonMessage(oddsComparisonMessage);
            };
        }

        [Time]
        protected override async void OnInitialized()
        {
            try
            {
                Debug.WriteLine("DetailOddsViewModel Initialize");

                await LoadData(() => FirstLoadOrRefreshOdds(SelectedBetType, oddsFormat, IsRefreshing));
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
        }

        public override void Destroy()
        {
            Debug.WriteLine("DetailOddsViewModel Destroy");
        }

        [Time]
        private async Task FirstLoadOrRefreshOdds(BetType betType, string formatType, bool isRefresh = false)
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
        internal async Task<OddsComparisonMessage> DeserializeComparisonMessage(string message)
        {
            OddsComparisonMessage oddsComparisonMessage = null;

            try
            {
                oddsComparisonMessage = JsonConvert.DeserializeObject<OddsComparisonMessage>(message);
            }
            catch (Exception ex)
            {
                await LoggingService.LogErrorAsync("Errors while deserialize MatchOddsComparisonMessage", ex);
            }

            return oddsComparisonMessage;
        }

        [Time]
        internal async Task HandleOddsComparisonMessage(OddsComparisonMessage oddsComparisonMessage)
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

        private async Task UpdateOddsCache(OddsComparisonMessage oddsComparisonMessage)
        {
            if (oddsComparisonMessage.BetTypeOddsList != null &&
                oddsComparisonMessage.BetTypeOddsList.Any() &&
                oddsComparisonMessage.MatchId.Equals(matchId, StringComparison.OrdinalIgnoreCase))
            {
                var betTypes = oddsComparisonMessage.BetTypeOddsList.Select(x => x.Id);

                foreach (var betTypeId in betTypes)
                {
                    await oddsService.GetOdds(SettingsService.CurrentLanguage, matchId, betTypeId, oddsFormat, forceFetchNewData: true);
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