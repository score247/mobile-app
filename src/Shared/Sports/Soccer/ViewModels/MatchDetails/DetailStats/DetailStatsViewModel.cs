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

namespace LiveScore.Soccer.ViewModels.DetailStats
{
    internal class DetailStatsViewModel : TabItemViewModel
    {
        private readonly ISoccerMatchService soccerMatchService;
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
            soccerMatchService = DependencyResolver.Resolve<ISoccerMatchService>();
            RefreshCommand = new DelegateAsyncCommand(
                async () => await LoadDataAsync(() => LoadMatchStatisticDataAsync(true), false));
        }

        public DelegateAsyncCommand RefreshCommand { get; }

        public MatchStatisticItem MainStatistic { get; private set; }

        public IEnumerable<MatchStatisticItem> SubStatisticItems { get; private set; }

        public bool IsRefreshing { get; set; }
        public bool IsNoData { get; private set; }

        public bool IsVisible => !IsNoData;

        private async Task LoadMatchStatisticDataAsync(bool isRefresh = false)
        {
            var matchStatisticData 
                = await soccerMatchService
                .GetMatchStatistic(this.matchId, Language.English, isRefresh).ConfigureAwait(false);

            MainStatistic = matchStatisticData.GetMainStatistic();
            SubStatisticItems = matchStatisticData.GetSubStatisticItems();

            IsNoData = MainStatistic.IsHidden && !SubStatisticItems.Any();
            IsRefreshing = false;
        }

        public override async void OnAppearing()
        {
            base.OnAppearing();

            await LoadMatchStatistic();
        }
    }
}