using System;
using System.Collections.Generic;
using System.Linq;
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

namespace LiveScore.Soccer.ViewModels.Matches.MatchDetails.Statistics
{
    public class StatisticsViewModel : TabItemViewModel, IDisposable
    {
        private readonly string matchId;
        private readonly DateTimeOffset eventDate;
        private ISoccerMatchService soccerMatchService;
        private bool disposed = false;

        public StatisticsViewModel(
            string matchId,
            DateTimeOffset eventDate,
            INavigationService navigationService,
            IDependencyResolver serviceLocator,
            IEventAggregator eventAggregator,
            DataTemplate dataTemplate)
            : base(navigationService, serviceLocator, dataTemplate, eventAggregator, AppResources.Stats)
        {
            this.matchId = matchId;
            this.eventDate = eventDate;
            
        }

        public DelegateAsyncCommand RefreshCommand { get; private set; }

        public List<StatisticsItemViewModel> StatisticItems { get; private set; }

        public bool IsRefreshing { get; set; }

        public override async void OnAppearing()
        {
            base.OnAppearing();

            if (soccerMatchService == null)
            {
                soccerMatchService = DependencyResolver.Resolve<ISoccerMatchService>();
                RefreshCommand = new DelegateAsyncCommand(
                    async () => await LoadDataAsync(LoadStatisticsAsync, false));
            }

            await LoadDataAsync(LoadStatisticsAsync);
        }

        public override async void OnResumeWhenNetworkOK()
            => await LoadDataAsync(LoadStatisticsAsync);

        public override void Destroy()
        {
            base.Destroy();

            Dispose();
        }

        public override Task OnNetworkReconnectedAsync()
            => LoadDataAsync(LoadStatisticsAsync);

        public async Task LoadStatisticsAsync()
        {
            var matchStatistic = await soccerMatchService
                    .GetMatchStatisticAsync(matchId, Language.English, eventDate)
                    .ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(matchStatistic?.MatchId))
            {
                var statisticItems = new List<StatisticsItemViewModel>();
                var mainStatistic = GetMainStatistic(matchStatistic);

                if (mainStatistic.IsVisibled)
                {
                    statisticItems.Add(mainStatistic);
                }

                statisticItems.AddRange(GetSubStatisticItems(matchStatistic));

                HasData = statisticItems.Count > 0;
                StatisticItems = statisticItems;
            }
            else
            {
                HasData = false;
            }

            IsRefreshing = false;
        }

        public StatisticsItemViewModel GetMainStatistic(MatchStatistic statistic)
           => new StatisticsItemViewModel(AppResources.BallPossession, statistic.HomeStatistic?.Possession, statistic.AwayStatistic?.Possession, true);

#pragma warning disable S1541 // Methods and properties should not be too complex

        public IEnumerable<StatisticsItemViewModel> GetSubStatisticItems(MatchStatistic statistic)
#pragma warning restore S1541 // Methods and properties should not be too complex
        {
            var subItems = new List<StatisticsItemViewModel>
            {
                new StatisticsItemViewModel(AppResources.ShotsOnTarget, statistic.HomeStatistic?.ShotsOnTarget, statistic.AwayStatistic?.ShotsOnTarget),
                new StatisticsItemViewModel(AppResources.ShotsOffTarget, statistic.HomeStatistic?.ShotsOffTarget, statistic.AwayStatistic?.ShotsOffTarget),
                new StatisticsItemViewModel(AppResources.BlockedShots, statistic.HomeStatistic?.ShotsBlocked, statistic.AwayStatistic?.ShotsBlocked),
                new StatisticsItemViewModel(AppResources.GoalKicks, statistic.HomeStatistic?.GoalKicks, statistic.AwayStatistic?.GoalKicks),
                new StatisticsItemViewModel(AppResources.TotalShots,
                    (byte?)(statistic.HomeStatistic?.ShotsOnTarget + statistic.HomeStatistic?.ShotsOffTarget),
                    (byte?)(statistic.AwayStatistic?.ShotsOnTarget + statistic.AwayStatistic?.ShotsOffTarget)),
                new StatisticsItemViewModel(AppResources.CornerKicks, statistic.HomeStatistic?.CornerKicks, statistic.AwayStatistic?.CornerKicks),
                new StatisticsItemViewModel(AppResources.Offside, statistic.HomeStatistic?.Offsides, statistic.AwayStatistic?.Offsides),
                new StatisticsItemViewModel(AppResources.YellowCards, statistic.HomeStatistic?.YellowCards, statistic.AwayStatistic?.YellowCards),
                new StatisticsItemViewModel(AppResources.RedCards,
                    (byte?)(statistic.HomeStatistic?.RedCards + statistic.HomeStatistic?.YellowRedCards),
                    (byte?)(statistic.AwayStatistic?.RedCards + statistic.AwayStatistic?.YellowRedCards)),
                new StatisticsItemViewModel(AppResources.ThrowIns, statistic.HomeStatistic?.ThrowIns, statistic.AwayStatistic?.ThrowIns),
                new StatisticsItemViewModel(AppResources.FreeKicks, statistic.HomeStatistic?.FreeKicks, statistic.AwayStatistic?.FreeKicks),
                new StatisticsItemViewModel(AppResources.Fouls, statistic.HomeStatistic?.Fouls, statistic.AwayStatistic?.Fouls),
                new StatisticsItemViewModel(AppResources.Injuries, statistic.HomeStatistic?.Injuries, statistic.AwayStatistic?.Injuries),
            };

            if (subItems.All(item => item.IsHidden))
            {
                return Enumerable.Empty<StatisticsItemViewModel>();
            }

            return subItems.Where(item => item.IsVisibled);
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                StatisticItems = null;
            }

            disposed = true;
        }
    }
}