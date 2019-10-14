namespace LiveScore.Soccer.ViewModels.DetailStats
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using LiveScore.Common.Extensions;
    using LiveScore.Core;
    using LiveScore.Core.Controls.TabStrip;
    using LiveScore.Soccer.Models.Matches;
    using LiveScore.Soccer.Services;
    using Prism.Events;
    using Prism.Navigation;
    using Xamarin.Forms;

    internal class DetailStatsViewModel : TabItemViewModel
    {
        private readonly IMatchInfoService matchInfoService;
        private readonly string matchId;

        public DetailStatsViewModel(
            string matchId,
            INavigationService navigationService,
            IDependencyResolver serviceLocator,
            IEventAggregator eventAggregator,
            DataTemplate dataTemplate)
            : base(navigationService, serviceLocator, dataTemplate, eventAggregator)
        {
            this.matchId = matchId;
            matchInfoService = DependencyResolver.Resolve<IMatchInfoService>();
            RefreshCommand = new DelegateAsyncCommand(async () => await LoadDataAsync(() => LoadMatchStatistic(true), false));
        }

        public DelegateAsyncCommand RefreshCommand { get; }

        public MatchStatisticItem MainStatistic { get; private set; }

        public IEnumerable<MatchStatisticItem> SubStatisticItems { get; private set; }

        public bool IsNoData { get; private set; }

        public bool IsVisible => !IsNoData;

        private async Task LoadMatchStatistic(bool isRefresh = false)
        {
            var matchStatistic = await matchInfoService.GetMatchStatistic(matchId, isRefresh).ConfigureAwait(false);

            MainStatistic = matchStatistic.GetMainStatistic();
            SubStatisticItems = matchStatistic.GetSubStatisticItems();

            IsNoData = MainStatistic.IsHidden && !SubStatisticItems.Any();
        }

        public override async void OnAppearing() => await LoadMatchStatistic();
    }
}