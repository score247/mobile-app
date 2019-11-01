using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using LiveScore.Common.Extensions;
using LiveScore.Common.LangResources;
using LiveScore.Core;
using LiveScore.Core.Controls.TabStrip;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Models.Odds;
using LiveScore.Soccer.Enumerations;
using LiveScore.Soccer.PubSubEvents.Odds;
using LiveScore.Soccer.Services;
using LiveScore.Soccer.ViewModels.MatchDetails.Odds.OddItems;
using MethodTimer;
using Prism.Events;
using Prism.Navigation;
using Xamarin.Forms;

[assembly: InternalsVisibleTo("Soccer.Tests")]

namespace LiveScore.Soccer.ViewModels.MatchDetails.Odds
{
    public class OddsViewModel : TabItemViewModel
    {
        private readonly string matchId;
        private readonly string oddsFormat;
        private readonly MatchStatus eventStatus;
        private readonly IOddsService oddsService;

        public OddsViewModel(
            string matchId,
            MatchStatus eventStatus,
            INavigationService navigationService,
            IDependencyResolver serviceLocator,
            IEventAggregator eventAggregator,
            DataTemplate dataTemplate)
            : base(navigationService, serviceLocator, dataTemplate, eventAggregator, AppResources.Odds)
        {
            this.matchId = matchId;
            this.eventStatus = eventStatus;

            oddsFormat = OddsFormat.Decimal.DisplayName;
            SelectedBetType = BetType.AsianHDP;
            BetTypeOddsItems = new List<BaseItemViewModel>();

            HasData = true;
            IsRefreshing = false;

            oddsService = DependencyResolver.Resolve<IOddsService>(CurrentSportId.ToString());

            RefreshCommand = new DelegateAsyncCommand(Refresh);

            OnOddsTabClicked = new DelegateAsyncCommand<string>(betTypeId
                => FirstLoadOrRefreshOddsAsync(Enumeration.FromValue<BetType>(byte.Parse(betTypeId)), oddsFormat));

            TappedOddsItemCommand = new DelegateAsyncCommand<BaseItemViewModel>(HandleOddsItemTapCommandAsync);
        }

        public bool IsRefreshing { get; set; }

        public BetType SelectedBetType { get; private set; }

        public bool IsOneXTwoSelected => SelectedBetType == BetType.OneXTwo;

        public bool IsAsianHdpSelected => SelectedBetType == BetType.AsianHDP;

        public bool IsOverUnderSelected => SelectedBetType == BetType.OverUnder;

        public IList<BaseItemViewModel> BetTypeOddsItems { get; private set; }

        public DataTemplate HeaderTemplate { get; private set; }

        public DelegateAsyncCommand RefreshCommand { get; }

        public DelegateAsyncCommand<string> OnOddsTabClicked { get; }

        public DelegateAsyncCommand<BaseItemViewModel> TappedOddsItemCommand { get; }

        [Time]
        public override async void OnResumeWhenNetworkOK()
        {
            await LoggingService
                              .TrackEventAsync(
                              $"Odds - {matchId}",
                              $"{DateTime.Now} Selected BetType {SelectedBetType} - OnResumeWhenNetworkOK").ConfigureAwait(false);

            await Resume();
        }

        internal async Task Resume()
        {
            await UpdateOddsByBetTypeAsync().ConfigureAwait(false);

            SubscribeEvents();
        }

        public override async void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                await LoadDataAsync(() => FirstLoadOrRefreshOddsAsync(SelectedBetType, oddsFormat)).ConfigureAwait(false);

                SubscribeEvents();
            }
            catch (Exception ex)
            {
                await LoggingService.LogExceptionAsync(ex).ConfigureAwait(false);
            }
        }

        public override void OnSleep()
        {
            base.OnSleep();

            LoggingService
                    .TrackEvent(
                    $"Odds - {matchId}",
                    $"{DateTime.Now} Selected BetType {SelectedBetType} - OnSleep");

            UnsubscribeEvents();
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();

            UnsubscribeEvents();
        }

        public override Task OnNetworkReconnectedAsync() => RefreshCommand.ExecuteAsync();

        private void SubscribeEvents()
            => EventAggregator?.GetEvent<OddsComparisonPubSubEvent>().Subscribe(HandleOddsComparisonMessage);

        private void UnsubscribeEvents()
            => EventAggregator?.GetEvent<OddsComparisonPubSubEvent>().Unsubscribe(HandleOddsComparisonMessage);

        private async Task Refresh()
        {
            IsRefreshing = true;

            await FirstLoadOrRefreshOddsAsync(SelectedBetType, oddsFormat);

            IsRefreshing = false;
        }

        [Time]
        internal async Task FirstLoadOrRefreshOddsAsync(BetType betType, string formatType)
        {
            if (CanLoadOdds(betType))
            {
                IsBusy = !IsRefreshing;

                SelectedBetType = betType;
                await LoadOddsByBetTypeAsync(formatType).ConfigureAwait(false);

                IsBusy = false;
            }
        }

        private async Task UpdateOddsByBetTypeAsync()
        {
            if (eventStatus == MatchStatus.NotStarted || eventStatus == MatchStatus.Live)
            {
                await LoadOddsByBetTypeAsync(oddsFormat).ConfigureAwait(false);
            }
        }

        private async Task LoadOddsByBetTypeAsync(string formatType)
        {            
            var odds = await oddsService.GetOddsAsync(
                CurrentLanguage.DisplayName,
                matchId,
                SelectedBetType.Value,
                formatType).ConfigureAwait(false);

            HasData = odds?.BetTypeOddsList?.Any() == true;

            HeaderTemplate = new BaseHeaderViewModel(
                SelectedBetType,
                HasData,
                NavigationService,
                DependencyResolver).CreateTemplate();

            BetTypeOddsItems = HasData
                ? new List<BaseItemViewModel>((odds.BetTypeOddsList ?? throw new InvalidOperationException())
                    .Select(betTypeOdds => new BaseItemViewModel(
                        SelectedBetType,
                        betTypeOdds,
                        NavigationService,
                        DependencyResolver)
                    .CreateInstance()))
                : Enumerable.Empty<BaseItemViewModel>().ToList();
        }

        private bool CanLoadOdds(BetType betType)
            => SelectedBetType != betType
            || BetTypeOddsItems?.Any() != true;

        [Time]
        internal void HandleOddsComparisonMessage(OddsComparisonMessage oddsComparisonMessage)
        {
            if (!oddsComparisonMessage
                .MatchId
                .Equals(matchId, StringComparison.OrdinalIgnoreCase)
                || oddsComparisonMessage
                    .BetTypeOddsList?
                    .All(x => x.Id != SelectedBetType.Value) != false)
            {
                return;
            }

            var updatedBetTypeOdds = oddsComparisonMessage
                .BetTypeOddsList
                .Where(x => x.Id == SelectedBetType.Value);
            {
                var needToReOrder = false;

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
                    BetTypeOddsItems = new ObservableCollection<BaseItemViewModel>(BetTypeOddsItems.OrderBy(x => x
                        .BetTypeOdds
                        .Bookmaker
                        .Name));
                }
            }
        }

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
                await LoggingService.LogExceptionAsync(navigated.Exception)
                    .ConfigureAwait(false);
            }
        }

        private void AddBookmakerOdds(IBetTypeOdds updatedOdds)
        {
            HasData = true;
            var newOddsItemViewModel
                = new BaseItemViewModel(
                    SelectedBetType,
                    updatedOdds,
                    NavigationService,
                    DependencyResolver)
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