using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using LiveScore.Common.Extensions;
using LiveScore.Core;
using LiveScore.Core.Controls.TabStrip;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Models.Odds;
using LiveScore.Core.PubSubEvents.Odds;
using LiveScore.Soccer.Enumerations;
using LiveScore.Soccer.PubSubEvents.Odds;
using LiveScore.Soccer.Services;
using LiveScore.Soccer.ViewModels.MatchDetails.DetailOdds.OddItems;
using MethodTimer;
using Prism.Events;
using Prism.Navigation;
using Xamarin.Forms;

[assembly: InternalsVisibleTo("Soccer.Tests")]

namespace LiveScore.Soccer.ViewModels.MatchDetails.DetailOdds
{
    internal class DetailOddsViewModel : TabItemViewModel
    {
        private readonly string matchId;
        private readonly string oddsFormat;

        private readonly MatchStatus eventStatus;
        private readonly IOddsService oddsService;
        private readonly IEventAggregator eventAggregator;

        public DetailOddsViewModel(
            string matchId,
            MatchStatus eventStatus,
            INavigationService navigationService,
            IDependencyResolver serviceLocator,
            IEventAggregator eventAggregator,
            DataTemplate dataTemplate)
            : base(navigationService, serviceLocator, dataTemplate, eventAggregator)
        {
            Debug.WriteLine($"MatchId {matchId}");

            this.matchId = matchId;
            this.eventStatus = eventStatus;
            this.eventAggregator = eventAggregator;

            oddsFormat = OddsFormat.Decimal.DisplayName;
            SelectedBetType = BetType.AsianHDP;            
            BetTypeOddsItems = new List<BaseItemViewModel>();

            HasData = true;
            IsRefreshing = false;

            oddsService = DependencyResolver.Resolve<IOddsService>(CurrentSportId.ToString());          
            this.eventAggregator.GetEvent<OddsComparisonPubSubEvent>().Subscribe(HandleOddsComparisonMessage, ThreadOption.UIThread, true);
        }

        public bool IsRefreshing { get; set; }        

        public string HeaderTitle => string.Empty;

        public BetType SelectedBetType { get; private set; }

        public bool IsOneXTwoSelected => SelectedBetType == BetType.OneXTwo;

        public bool IsAsianHdpSelected => SelectedBetType == BetType.AsianHDP;

        public bool IsOverUnderSelected => SelectedBetType == BetType.OverUnder;

        public IList<BaseItemViewModel> BetTypeOddsItems { get; private set; }

        public DataTemplate HeaderTemplate { get; private set; }

        public DelegateAsyncCommand RefreshCommand => new DelegateAsyncCommand(() =>
                LoadDataAsync(() => FirstLoadOrRefreshOddsAsync(SelectedBetType, oddsFormat, true)));

        public DelegateAsyncCommand<string> OnOddsTabClicked => new DelegateAsyncCommand<string>((betTypeId) =>
                FirstLoadOrRefreshOddsAsync(Enumeration.FromValue<BetType>(byte.Parse(betTypeId)), oddsFormat));

        public DelegateAsyncCommand<BaseItemViewModel> TappedOddsItemCommand => new DelegateAsyncCommand<BaseItemViewModel>(HandleOddsItemTapCommandAsync);

        private async Task HandleOddsItemTapCommandAsync(BaseItemViewModel item)
        {
            var parameters = new NavigationParameters
            {
                { "MatchId", matchId },
                { "EventStatus", eventStatus },
                { "Bookmaker", item.BetTypeOdds.Bookmaker},
                { "BetType", SelectedBetType },
                { "Format",  oddsFormat}
            };

            var navigated
                = await NavigationService
                    .NavigateAsync("OddsMovementView" + CurrentSportId, parameters)
                    .ConfigureAwait(false);

            if (!navigated.Success)
            {
                await LoggingService.LogErrorAsync(navigated.Exception)
                    .ConfigureAwait(false);
            }
        }

        public override async void OnAppearing()
        {
            Debug.WriteLine("DetailOddsViewModel OnAppearing");

            try
            {
                Debug.WriteLine("DetailOddsViewModel OnInitialized");

                await LoadDataAsync(
                        () => FirstLoadOrRefreshOddsAsync(SelectedBetType, oddsFormat, IsRefreshing))
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await LoggingService.LogErrorAsync(ex).ConfigureAwait(false);
            }
        }

        public override async void OnResumeWhenNetworkOK()
        {
            await LoggingService.LogInfoAsync($"OddsComparison  {DateTime.Now} - OnResume - Selected BetType {SelectedBetType}").ConfigureAwait(false);

            if (eventStatus == MatchStatus.NotStarted || eventStatus == MatchStatus.Live)
            {
                await LoadOddsByBetTypeAsync(oddsFormat, isRefresh: true).ConfigureAwait(false);
            }

            eventAggregator.GetEvent<OddsComparisonPubSubEvent>().Subscribe(HandleOddsComparisonMessage, ThreadOption.UIThread);
        }

        public override void OnSleep()
        {
            Debug.WriteLine("DetailOddsViewModel OnSleep");
            eventAggregator.GetEvent<OddsComparisonPubSubEvent>().Unsubscribe(HandleOddsComparisonMessage);
        }

        public override void Destroy()
        {
            Debug.WriteLine("DetailOddsViewModel Destroy");

            eventAggregator.GetEvent<OddsComparisonPubSubEvent>().Unsubscribe(HandleOddsComparisonMessage);
        }

        [Time]
        private async Task FirstLoadOrRefreshOddsAsync(BetType betType, string formatType, bool isRefresh = false)
        {
            if (CanLoadOdds(betType, isRefresh))
            {
                IsBusy = !isRefresh;

                SelectedBetType = betType;
                await LoadOddsByBetTypeAsync(formatType, isRefresh).ConfigureAwait(false);

                IsRefreshing = false;
                IsBusy = false;
            }
        }

        private async Task LoadOddsByBetTypeAsync(string formatType, bool isRefresh)
        {
            var forceFetchNew = isRefresh || (eventStatus == MatchStatus.NotStarted || eventStatus == MatchStatus.Live);

            var odds = await oddsService.GetOdds(CurrentLanguage.DisplayName, matchId, SelectedBetType.Value, formatType, forceFetchNew).ConfigureAwait(false);

            HasData = odds.BetTypeOddsList?.Any() == true;

            HeaderTemplate = new BaseHeaderViewModel(SelectedBetType, HasData, NavigationService, DependencyResolver).CreateTemplate();

            BetTypeOddsItems = HasData
                ? new List<BaseItemViewModel>((odds.BetTypeOddsList ?? throw new InvalidOperationException())
                    .Select(bettypeOdds => new BaseItemViewModel(SelectedBetType, bettypeOdds, NavigationService, DependencyResolver)
                    .CreateInstance()))
                : new List<BaseItemViewModel>();
        }

        private bool CanLoadOdds(BetType betType, bool isRefresh)
            => isRefresh
            || SelectedBetType != betType
            || BetTypeOddsItems?.Any() != true;

        [Time]
        internal void HandleOddsComparisonMessage(OddsComparisonMessage oddsComparisonMessage)
        {
            LoggingService.LogInfo($"HandleOddsComparisonMessage {DateTime.Now} - Match {oddsComparisonMessage.MatchId} - Selected BetType {SelectedBetType}");

            if (!oddsComparisonMessage.MatchId.Equals(matchId, StringComparison.OrdinalIgnoreCase)
                || oddsComparisonMessage.BetTypeOddsList?.All(x => x.Id != SelectedBetType.Value) != false)
            {
                return;
            }

            var updatedBetTypeOdds = oddsComparisonMessage.BetTypeOddsList.Where(x => x.Id == SelectedBetType.Value);
            {
                var needToReOrder = false;

                LoggingService.LogInfo($"HandleOddsComparisonMessage {DateTime.Now} - Match {oddsComparisonMessage.MatchId} - Selected BetType {SelectedBetType} - Update odds");

                foreach (var updatedOdds in updatedBetTypeOdds)
                {
                    var existingOddsItem = BetTypeOddsItems
                        .FirstOrDefault(x => x.BetTypeOdds.Bookmaker == updatedOdds.Bookmaker);

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
        }

        private void AddBookmakerOdds(IBetTypeOdds updatedOdds)
        {
            HasData = true;
            var newOddsItemViewModel
                = new BaseItemViewModel(SelectedBetType, updatedOdds, NavigationService, DependencyResolver)
                    .CreateInstance();

            if (BetTypeOddsItems.Count == 0)
            {
                HeaderTemplate
                    = new BaseHeaderViewModel(SelectedBetType, HasData, NavigationService, DependencyResolver)
                        .CreateTemplate();
            }

            BetTypeOddsItems.Add(newOddsItemViewModel);
        }
    }
}