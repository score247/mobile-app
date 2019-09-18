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
    using Core;
    using Core.Controls.TabStrip;
    using Enumerations;
    using LiveScore.Common.Extensions;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Odds;
    using LiveScore.Core.PubSubEvents.Odds;
    using LiveScore.Soccer.PubSubEvents.Odds;
    using LiveScore.Soccer.Services;
    using MethodTimer;
    using OddItems;
    using Prism.Events;
    using Prism.Navigation;
    using Xamarin.Forms;

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
            : base(navigationService, serviceLocator, dataTemplate)
        {
            Debug.WriteLine($"MatchId {matchId}");
            this.matchId = matchId;
            this.eventStatus = eventStatus;

            oddsFormat = OddsFormat.Decimal.DisplayName;
            SelectedBetType = BetType.AsianHDP;
            IsRefreshing = false;
            BetTypeOddsItems = new List<BaseItemViewModel>();

            this.eventAggregator = eventAggregator;
            oddsService = DependencyResolver.Resolve<IOddsService>(CurrentSportId.ToString());

            RefreshCommand = new DelegateAsyncCommand(async () =>
                await LoadData(() => FirstLoadOrRefreshOdds(SelectedBetType, oddsFormat, true)).ConfigureAwait(false));

            OnOddsTabClicked = new DelegateAsyncCommand<string>(async (betTypeId) =>
                await FirstLoadOrRefreshOdds(Enumeration.FromValue<BetType>(byte.Parse(betTypeId)), oddsFormat).ConfigureAwait(false));

            TappedOddsItemCommand = new DelegateAsyncCommand<BaseItemViewModel>(HandleOddsItemTapCommand);

            //TODO verify keepSubscriberReferenceAlive
            this.eventAggregator.GetEvent<OddsComparisonPubSubEvent>().Subscribe(HandleOddsComparisonMessage, ThreadOption.UIThread);
        }

        public bool IsRefreshing { get; set; }

        public bool HasData { get; private set; }

        public string HeaderTitle => string.Empty;

        public BetType SelectedBetType { get; private set; }

        public bool IsOneXTwoSelected => SelectedBetType == BetType.OneXTwo;

        public bool IsAsianHdpSelected => SelectedBetType == BetType.AsianHDP;

        public bool IsOverUnderSelected => SelectedBetType == BetType.OverUnder;

        public IList<BaseItemViewModel> BetTypeOddsItems { get; private set; }        

        public DataTemplate HeaderTemplate { get; private set; }

        public DelegateAsyncCommand RefreshCommand { get; }

        public DelegateAsyncCommand<string> OnOddsTabClicked { get; }

        public DelegateAsyncCommand<BaseItemViewModel> TappedOddsItemCommand { get; }
        
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

            var navigated = await NavigationService.NavigateAsync("OddsMovementView" + CurrentSportId, parameters);

            if (!navigated.Success)
            {
                await LoggingService.LogErrorAsync(navigated.Exception).ConfigureAwait(false);
            }
        }

      
        public override async void OnAppearing()
        {
            Debug.WriteLine("DetailOddsViewModel OnAppearing");

            try
            {
                Debug.WriteLine("DetailOddsViewModel OnInitialized");

                await LoadData(() => FirstLoadOrRefreshOdds(SelectedBetType, oddsFormat, IsRefreshing));
            }
            catch (Exception ex)
            {
                await LoggingService.LogErrorAsync(ex);
            }
        }

        public override async void OnResume()
        {
            Debug.WriteLine("DetailOddsViewModel OnResume");

            if (eventStatus == MatchStatus.NotStarted || eventStatus == MatchStatus.Live)
            {
                await LoadOddsByBetType(oddsFormat, isRefresh: true);
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
        private async Task FirstLoadOrRefreshOdds(BetType betType, string formatType, bool isRefresh = false)
        {
            if (CanLoadOdds(betType, isRefresh))
            {
                IsBusy = !isRefresh;

                SelectedBetType = betType;
                await LoadOddsByBetType(formatType, isRefresh).ConfigureAwait(false);

                IsRefreshing = false;
                IsBusy = false;
            }
        }

        private async Task LoadOddsByBetType(string formatType, bool isRefresh)
        {
            var forceFetchNew = isRefresh || (eventStatus == MatchStatus.NotStarted || eventStatus == MatchStatus.Live);

            var odds = await oddsService.GetOdds(CurrentLanguage.DisplayName, matchId, SelectedBetType.Value, formatType, forceFetchNew).ConfigureAwait(false);

            HasData = odds.BetTypeOddsList?.Any() == true;

            HeaderTemplate = new BaseHeaderViewModel(SelectedBetType, HasData, NavigationService, DependencyResolver).CreateTemplate();

            BetTypeOddsItems = HasData
                ? new List<BaseItemViewModel>((odds.BetTypeOddsList ?? throw new InvalidOperationException()).Select(t =>
                    new BaseItemViewModel(SelectedBetType, t, NavigationService, DependencyResolver)
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
            Debug.WriteLine($"Selected BetType {SelectedBetType}");

            if (!oddsComparisonMessage.MatchId.Equals(matchId, StringComparison.OrdinalIgnoreCase)
                || oddsComparisonMessage.BetTypeOddsList?.All(x => x.Id != SelectedBetType.Value) != false)
            {
                return;
            }

            var updatedBetTypeOdds = oddsComparisonMessage.BetTypeOddsList.Where(x => x.Id == SelectedBetType.Value);
            {
                var needToReOrder = false;

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
    }
}