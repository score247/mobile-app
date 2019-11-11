using System.Linq;
using System.Threading.Tasks;
using LiveScore.Common.Services;
using LiveScore.Core;
using LiveScore.Core.Enumerations;
using LiveScore.Soccer.Models.Matches;
using LiveScore.Soccer.Models.Teams;
using LiveScore.Soccer.Services;
using LiveScore.Soccer.ViewModels.MatchDetails.Statistics;
using NSubstitute;
using Prism.Events;
using Prism.Navigation;
using Xamarin.Forms;
using Xunit;

namespace Soccer.Tests.ViewModels.DetailStatisitcs
{
    public class StatisticsViewModelTests
    {
        private readonly ISoccerMatchService soccerMatchService;
        private readonly string matchId = "match_id";
        private readonly StatisticsViewModel statisticViewModel;
        private readonly INavigationService navigationService;
        private readonly IDependencyResolver serviceLocator;
        private readonly IEventAggregator eventAggregator;
        private readonly ISettings settings;

        public StatisticsViewModelTests()
        {
            navigationService = Substitute.For<INavigationService>();
            serviceLocator = Substitute.For<IDependencyResolver>();
            eventAggregator = Substitute.For<IEventAggregator>();
            soccerMatchService = Substitute.For<ISoccerMatchService>();
            settings = Substitute.For<ISettings>();
            var networkConnectionManager = Substitute.For<INetworkConnection>();
            networkConnectionManager.IsSuccessfulConnection().Returns(true);

            serviceLocator.Resolve<ISoccerMatchService>().Returns(soccerMatchService);
            serviceLocator.Resolve<INetworkConnection>().Returns(networkConnectionManager);
            serviceLocator.Resolve<ISettings>().Returns(settings);
            settings.CurrentSportType.Returns(SportType.Soccer);

            statisticViewModel = new StatisticsViewModel(matchId, navigationService, serviceLocator, eventAggregator, new DataTemplate());
        }

        [Fact]
        public async Task LoadStatisticsAsync_StatisticIsEmpty_SetHasDataAndIsRefreshingToFalse()
        {
            await statisticViewModel.LoadStatisticsAsync();

            Assert.False(statisticViewModel.HasData);
            Assert.False(statisticViewModel.IsRefreshing);
        }

        [Fact]
        public async Task LoadStatisticsAsync_StatisticHasData_SetHasDataToTrueAndIsRefreshingToFalse()
        {
            var homeStatistic = new TeamStatistic(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);
            var awayStatistic = new TeamStatistic(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);
            var matchStatistic = new MatchStatistic(matchId, homeStatistic, awayStatistic);
            soccerMatchService.GetMatchStatisticAsync(matchId, Language.English).Returns(Task.FromResult(matchStatistic));

            await statisticViewModel.LoadStatisticsAsync();

            Assert.True(statisticViewModel.HasData);
            Assert.False(statisticViewModel.IsRefreshing);
        }

        [Fact]
        public async Task LoadStatisticsAsync_StatisticHasData_SetMainStatisticAndSubStatisticData()
        {
            var homeStatistic = new TeamStatistic(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);
            var awayStatistic = new TeamStatistic(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);
            var matchStatistic = new MatchStatistic(matchId, homeStatistic, awayStatistic);
            soccerMatchService.GetMatchStatisticAsync(matchId, Language.English).Returns(Task.FromResult(matchStatistic));

            await statisticViewModel.LoadStatisticsAsync();

            Assert.Equal(homeStatistic.Possession, statisticViewModel.StatisticItems.FirstOrDefault().HomeValue);
            Assert.Equal(awayStatistic.Possession, statisticViewModel.StatisticItems.FirstOrDefault().AwayValue);
            Assert.Equal(homeStatistic.ShotsOnTarget, statisticViewModel.StatisticItems.ElementAt(1).HomeValue);
            Assert.Equal(awayStatistic.ShotsOnTarget, statisticViewModel.StatisticItems.ElementAt(1).AwayValue);
        }
    }
}