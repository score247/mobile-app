using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LiveScore.Common.Extensions;
using LiveScore.Common.LangResources;
using LiveScore.Core;
using LiveScore.Core.Controls.TabStrip;
using LiveScore.Core.Enumerations;
using LiveScore.Soccer.Models.Matches;
using LiveScore.Soccer.Services;
using Prism.Events;
using Prism.Navigation;
using Xamarin.Forms;

namespace LiveScore.Soccer.ViewModels.MatchDetails.Statistics
{
    public class StatisticsViewModel : TabItemViewModel
    {
        private readonly string matchId;
        private readonly DateTime eventDate;

        private readonly ISoccerMatchService soccerMatchService;        

        public StatisticsViewModel(
            string matchId,
            DateTime eventDate,
            INavigationService navigationService,
            IDependencyResolver serviceLocator,
            IEventAggregator eventAggregator,
            DataTemplate dataTemplate)
            : base(navigationService, serviceLocator, dataTemplate, eventAggregator, AppResources.Stats)
        {
            this.matchId = matchId;
            this.eventDate = eventDate;
            soccerMatchService = DependencyResolver.Resolve<ISoccerMatchService>();
            RefreshCommand = new DelegateAsyncCommand(
                async () => await LoadDataAsync(LoadStatisticsAsync, false));
        }

        public DelegateAsyncCommand RefreshCommand { get; }

        public List<MatchStatisticItem> StatisticItems { get; private set; }

        public bool IsRefreshing { get; set; }

        public override async void OnAppearing()
        {
            base.OnAppearing();

            await LoadDataAsync(LoadStatisticsAsync);
        }

        public override async void OnResumeWhenNetworkOK()
            => await LoadDataAsync(LoadStatisticsAsync);

        public override Task OnNetworkReconnectedAsync()
            => LoadDataAsync(LoadStatisticsAsync);

        public async Task LoadStatisticsAsync()
        {
            var matchStatistic = await soccerMatchService
                    .GetMatchStatisticAsync(matchId, Language.English, eventDate)
                    .ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(matchStatistic?.MatchId))
            {
                var statisticItems = new List<MatchStatisticItem>();
                var mainStatistic = matchStatistic.GetMainStatistic();

                if (mainStatistic.IsVisibled)
                {
                    statisticItems.Add(mainStatistic);
                }

                statisticItems.AddRange(matchStatistic.GetSubStatisticItems());

                HasData = statisticItems.Count > 0;
                StatisticItems = statisticItems;
            }
            else
            {
                HasData = false;
            }

            IsRefreshing = false;
        }
    }
}