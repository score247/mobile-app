using System;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Common.LangResources;
using LiveScore.Common.Services;
using LiveScore.Core;
using LiveScore.Core.Enumerations;
using LiveScore.Soccer.Models.Matches;
using LiveScore.Soccer.Models.Teams;
using LiveScore.Soccer.Services;
using LiveScore.Soccer.ViewModels.Matches.MatchDetails.Statistics;
using NSubstitute;
using Prism.Events;
using Prism.Navigation;
using Xamarin.Forms;
using Xunit;

namespace Soccer.Tests.ViewModels.DetailStatisitcs
{
    public class StatisticsViewModelTests
    {
        private static readonly DateTime MatchEventDate = new DateTime(2019, 01, 01);
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

            statisticViewModel = new StatisticsViewModel(matchId, MatchEventDate, navigationService, serviceLocator, eventAggregator, new DataTemplate());
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
            soccerMatchService.GetMatchStatisticAsync(matchId, Language.English, MatchEventDate).Returns(Task.FromResult(matchStatistic));

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
            soccerMatchService.GetMatchStatisticAsync(matchId, Language.English, MatchEventDate).Returns(Task.FromResult(matchStatistic));

            await statisticViewModel.LoadStatisticsAsync();

            Assert.Equal(homeStatistic.Possession, statisticViewModel.StatisticItems.FirstOrDefault().HomeValue);
            Assert.Equal(awayStatistic.Possession, statisticViewModel.StatisticItems.FirstOrDefault().AwayValue);
            Assert.Equal(homeStatistic.ShotsOnTarget, statisticViewModel.StatisticItems.ElementAt(1).HomeValue);
            Assert.Equal(awayStatistic.ShotsOnTarget, statisticViewModel.StatisticItems.ElementAt(1).AwayValue);
            Assert.Equal(homeStatistic.Offsides, statisticViewModel.StatisticItems.ElementAt(7).HomeValue);
            Assert.Equal(awayStatistic.Offsides, statisticViewModel.StatisticItems.ElementAt(7).AwayValue);
        }

        [Fact]
        public void GetMainStatistic_BallPossessionHasData_ReturnBallPossessionStatistic()
        {
            var homeStatistic = new TeamStatistic(2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            var awayStatistic = new TeamStatistic(4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

            var mainStatistic = statisticViewModel.GetMainStatistic(new MatchStatistic("matchId", homeStatistic, awayStatistic));

            Assert.Equal(AppResources.BallPossession.ToUpperInvariant(), mainStatistic.StatisticName);
            Assert.Equal("33%", mainStatistic.HomePercentText);
            Assert.Equal("67%", mainStatistic.AwayPercentText);
            Assert.Equal("0.33", string.Format("{0:0.00}", mainStatistic.HomePercent));
            Assert.Equal("0.67", string.Format("{0:0.00}", mainStatistic.AwayPercent));
        }

        [Fact]
        public void GetMainStatistic_BallPossessionHasNoData_BallPossessionStatisticIsHidden()
        {
            var homeStatistic = new TeamStatistic(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            var awayStatistic = new TeamStatistic(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

            var mainStatistic = statisticViewModel.GetMainStatistic(new MatchStatistic("matchId", homeStatistic, awayStatistic));

            Assert.True(mainStatistic.IsHidden);
        }

        [Fact]
        public void GetSubStatisticItems_StatisticHasNoData_SubStatisticItemsAreEmpty()
        {
            var homeStatistic = new TeamStatistic(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            var awayStatistic = new TeamStatistic(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

            var subStatisticItems = statisticViewModel.GetSubStatisticItems(new MatchStatistic("matchId", homeStatistic, awayStatistic));

            Assert.Empty(subStatisticItems);
        }

        [Fact]
        public void GetSubStatisticItems_SubStatisticHasData_ReturnOrderedSubStatistic()
        {
            var homeStatistic = new TeamStatistic(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 0, 14, 15);
            var awayStatistic = new TeamStatistic(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 0, 14, 15);

            var subStatisticItems = statisticViewModel.GetSubStatisticItems(new MatchStatistic("matchId", homeStatistic, awayStatistic));

            Assert.Equal(12, subStatisticItems.Count());
            Assert.Equal(AppResources.ShotsOnTarget.ToUpperInvariant(), subStatisticItems.ElementAt(0).StatisticName);
            Assert.Equal(6, subStatisticItems.ElementAt(0).HomeValue);
            Assert.Equal(6, subStatisticItems.ElementAt(0).AwayValue);

            Assert.Equal(AppResources.ShotsOffTarget.ToUpperInvariant(), subStatisticItems.ElementAt(1).StatisticName);
            Assert.Equal(7, subStatisticItems.ElementAt(1).HomeValue);
            Assert.Equal(7, subStatisticItems.ElementAt(1).AwayValue);

            Assert.Equal(AppResources.BlockedShots.ToUpperInvariant(), subStatisticItems.ElementAt(2).StatisticName);
            Assert.Equal(5, subStatisticItems.ElementAt(2).HomeValue);
            Assert.Equal(5, subStatisticItems.ElementAt(2).AwayValue);

            Assert.Equal(AppResources.GoalKicks.ToUpperInvariant(), subStatisticItems.ElementAt(3).StatisticName);
            Assert.Equal(4, subStatisticItems.ElementAt(3).HomeValue);
            Assert.Equal(4, subStatisticItems.ElementAt(3).AwayValue);

            Assert.Equal(AppResources.TotalShots.ToUpperInvariant(), subStatisticItems.ElementAt(4).StatisticName);
            Assert.Equal(13, subStatisticItems.ElementAt(4).HomeValue);
            Assert.Equal(13, subStatisticItems.ElementAt(4).AwayValue);

            Assert.Equal(AppResources.CornerKicks.ToUpperInvariant(), subStatisticItems.ElementAt(5).StatisticName);
            Assert.Equal(8, subStatisticItems.ElementAt(5).HomeValue);
            Assert.Equal(8, subStatisticItems.ElementAt(5).AwayValue);

            Assert.Equal(AppResources.Offside.ToUpperInvariant(), subStatisticItems.ElementAt(6).StatisticName);
            Assert.Equal(11, subStatisticItems.ElementAt(6).HomeValue);
            Assert.Equal(11, subStatisticItems.ElementAt(6).AwayValue);

            Assert.Equal(AppResources.YellowCards.ToUpperInvariant(), subStatisticItems.ElementAt(7).StatisticName);
            Assert.Equal(12, subStatisticItems.ElementAt(7).HomeValue);
            Assert.Equal(12, subStatisticItems.ElementAt(7).AwayValue);

            Assert.Equal(AppResources.RedCards.ToUpperInvariant(), subStatisticItems.ElementAt(8).StatisticName);
            Assert.Equal(29, subStatisticItems.ElementAt(8).HomeValue);
            Assert.Equal(29, subStatisticItems.ElementAt(8).AwayValue);

            Assert.Equal(AppResources.ThrowIns.ToUpperInvariant(), subStatisticItems.ElementAt(9).StatisticName);
            Assert.Equal(3, subStatisticItems.ElementAt(9).HomeValue);
            Assert.Equal(3, subStatisticItems.ElementAt(9).AwayValue);

            Assert.Equal(AppResources.FreeKicks.ToUpperInvariant(), subStatisticItems.ElementAt(10).StatisticName);
            Assert.Equal(2, subStatisticItems.ElementAt(10).HomeValue);
            Assert.Equal(2, subStatisticItems.ElementAt(10).AwayValue);

            Assert.Equal(AppResources.Fouls.ToUpperInvariant(), subStatisticItems.ElementAt(11).StatisticName);
            Assert.Equal(9, subStatisticItems.ElementAt(11).HomeValue);
            Assert.Equal(9, subStatisticItems.ElementAt(11).AwayValue);
        }
    }
}