[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Soccer.Tests")]

namespace LiveScore.Soccer.ViewModels.DetailOdds
{
    using LiveScore.Common.Extensions;
    using LiveScore.Core;
    using LiveScore.Core.Controls.TabStrip;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Events;
    using LiveScore.Core.Models.Odds;
    using LiveScore.Core.Services;
    using LiveScore.Soccer.Enumerations;
    using LiveScore.Soccer.Models.Odds;
    using LiveScore.Soccer.ViewModels.DetailOdds.OddItems;
    using MethodTimer;
    using Newtonsoft.Json;
    using Prism.Events;
    using Prism.Navigation;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Xamarin.Forms;

    internal class DetailOddsViewModel : TabItemViewModel, IDisposable
    {
        private readonly string matchId;
        private readonly string oddsFormat;
        private readonly IOddsService oddsService;
        private bool disposedValue;

        public DetailOddsViewModel(
            string matchId,
            INavigationService navigationService,
            IDependencyResolver serviceLocator,
            IEventAggregator eventAggregator,
            DataTemplate dataTemplate)
            : base(navigationService, serviceLocator, dataTemplate, eventAggregator)
        {
            this.matchId = matchId;

            oddsFormat = OddsFormat.Decimal.DisplayName;
            SelectedBetType = BetType.AsianHDP;
            IsRefreshing = false;

            oddsService = DependencyResolver.Resolve<IOddsService>(SettingsService.CurrentSportType.Value.ToString());

            RefreshCommand = new DelegateAsyncCommand(async () => await LoadData(() => LoadOdds(SelectedBetType, oddsFormat, true)));

            OnOddsTabClicked = new DelegateAsyncCommand<string>(HandleButtonCommand);

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

        private Task HandleButtonCommand(string betTypeId)
            => LoadData(() => LoadOdds(Enumeration.FromValue<BetType>(Byte.Parse(betTypeId)), oddsFormat));

        private async Task HandleOddsItemTapCommand(BaseItemViewModel item)
        {
            var parameters = new NavigationParameters
            {
                { "MatchId", matchId },
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
        protected override async void OnInitialized()
        {
            try
            {
                await LoadData(() => LoadOdds(SelectedBetType, oddsFormat, IsRefreshing));

                // TODO: need review UIThread here
                EventAggregator.GetEvent<OddsComparisonPubSubEvent>().Subscribe(
                    async (oddsComparisonMessage) =>
                    {
                        if (oddsComparisonMessage == null)
                        {
                            return;
                        }

                        await HandleOddsComparisonMessage(oddsComparisonMessage.OddsComparison);
                    },
                    ThreadOption.UIThread,
                    true);
            }
            catch (Exception ex)
            {
                await LoggingService.LogErrorAsync(ex);
            }
        }

        public override async void OnResume()
        {
            //TODO not re-load odds when match is closed
            await LoadOddsByBetType(oddsFormat, isRefresh: true);
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
            var odds = await oddsService.GetOdds(SettingsService.CurrentLanguage, matchId, SelectedBetType.Value, formatType, isRefresh);

            HasData = odds.BetTypeOddsList?.Any() == true;

            HeaderTemplate = new BaseHeaderViewModel(SelectedBetType, HasData, NavigationService, DependencyResolver).CreateTemplate();

            BetTypeOddsItems = HasData
                ? new List<BaseItemViewModel>(odds.BetTypeOddsList.Select(t =>
                    new BaseItemViewModel(SelectedBetType, t, NavigationService, DependencyResolver)
                    .CreateInstance()))
                : new List<BaseItemViewModel>();
        }

        private bool CanLoadOdds(BetType betType, bool isRefresh)
            => isRefresh || SelectedBetType != betType || BetTypeOddsItems == null || !BetTypeOddsItems.Any();

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

                await oddsService.GetOdds(SettingsService.CurrentLanguage, matchId, SelectedBetType.Value, oddsFormat, forceFetchNewData: true);
            }
        }

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