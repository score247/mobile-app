namespace Soccer.Tests.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using KellermanSoftware.CompareNetObjects;
    using LiveScore.Core.Converters;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Services;
    using LiveScore.Core.Tests.Fixtures;
    using LiveScore.Soccer.Converters;
    using LiveScore.Soccer.ViewModels;
    using LiveScore.Soccer.ViewModels.MatchDetailInfo;
    using NSubstitute;
    using Prism.Navigation;
    using Xunit;

    public class MatchDetailViewModelTests : IClassFixture<ViewModelBaseFixture>
    {
        private readonly MatchDetailViewModel viewModel;
        private readonly IMatchService matchService;
        private readonly CompareLogic comparer;
        private readonly IMatch match;

        public MatchDetailViewModelTests(ViewModelBaseFixture baseFixture)
        {
            comparer = baseFixture.CommonFixture.Comparer;
            baseFixture.DependencyResolver.Resolve<IMatchStatusConverter>(
                baseFixture.AppSettingsFixture.SettingsService.CurrentSportType.Value)
                .Returns(new MatchStatusConverter());
            matchService = Substitute.For<IMatchService>();
            baseFixture.DependencyResolver.Resolve<IMatchService>("1").Returns(matchService);
            viewModel = new MatchDetailViewModel(
                baseFixture.NavigationService,
                baseFixture.DependencyResolver,
                baseFixture.EventAggregator,
                baseFixture.HubService);

            match = CreateMatch();
            var parameters = new NavigationParameters { { "Match", match } };
            viewModel.OnNavigatingTo(parameters);
        }

        private IMatch CreateMatch()
        {
            var matchData = Substitute.For<IMatch>();
            matchData.Id.Returns("1234");
            matchData.League.Name.Returns("Laliga");
            matchData.EventDate.Returns(new DateTime(2019, 01, 01, 18, 00, 00));
            matchData.Attendance.Returns(2034);
            matchData.Venue.Returns(new Venue { Name = "My Dinh" });
            matchData.MatchResult.EventStatus.Returns(new MatchStatus { Value = MatchStatus.Live });
            matchData.MatchResult.MatchStatus.Returns(new MatchStatus { Value = MatchStatus.FirstHaft });

            return matchData;
        }

        [Fact]
        public void OnNavigatingTo_ParametersIsNotNull_BuildMatchStatus()
        {
            // Arrange
            match.LatestTimeline = new Timeline { StoppageTime = "4", InjuryTimeAnnounced = 5 };
            match.MatchResult.MatchTime = 49;
            var parameters = new NavigationParameters { { "Match", match } };

            // Act
            viewModel.OnNavigatingTo(parameters);

            // Assert
            Assert.Equal("45+4'", viewModel.MatchViewModel.DisplayMatchStatus);
        }

        [Fact]
        public void OnNavigatingTo_ParametersIsNotNull_BuildLeagueNameAndEventDate()
        {
            // Arrange
            var parameters = new NavigationParameters { { "Match", match } };

            // Act
            viewModel.OnNavigatingTo(parameters);

            // Assert
            Assert.Equal("01 Jan, 2019 - LALIGA", viewModel.DisplayEventDateAndLeagueName);
        }

        [Fact]
        public void OnNavigatingTo_IsNotPreMatch_ShowScore()
        {
            // Arrange
            match.MatchResult.HomeScore.Returns(5);
            match.MatchResult.AwayScore.Returns(1);
            var parameters = new NavigationParameters { { "Match", match } };

            // Act
            viewModel.OnNavigatingTo(parameters);

            // Assert
            Assert.Equal("5 - 1", viewModel.DisplayScore);
        }

        [Fact]
        public void OnNavigatingTo_IsPreMatch_NotShowScore()
        {
            // Arrange
            match.MatchResult.EventStatus.Returns(new MatchStatus { Value = MatchStatus.NotStarted });
            var parameters = new NavigationParameters { { "Match", match } };

            // Act
            viewModel.OnNavigatingTo(parameters);

            // Assert
            Assert.Equal(" - ", viewModel.DisplayScore);
        }

        [Fact]
        public void OnAppearing_Always_LoadMatchDetail()
        {
            // Arrange
            var returnMatch = CreateMatch();
            var returnTimelines = new List<ITimeline>
            {
                new Timeline { Type = "red_card", Time = new DateTime(2019, 01, 01, 18, 00, 00) },
                new Timeline { Type = "yellow_card", Time = new DateTime(2019, 01, 01, 17, 00, 00 )},
                new Timeline { Type = "corner_kick", Time = new DateTime(2019, 01, 01, 17, 15, 00 )}
            };
            returnMatch.TimeLines.Returns(returnTimelines);
            matchService.GetMatch(viewModel.SettingsService.UserSettings, match.Id, false).Returns(returnMatch);

            // Act
            viewModel.OnAppearing();

            // Assert
            var expectedInfoItemViewModels = new ObservableCollection<BaseInfoItemViewModel>
            {
                new BaseInfoItemViewModel(returnTimelines[1], returnMatch.MatchResult, viewModel.NavigationService, viewModel.DependencyResolver).CreateInstance(),
                new BaseInfoItemViewModel(returnTimelines[0], returnMatch.MatchResult, viewModel.NavigationService, viewModel.DependencyResolver).CreateInstance()
            };
            var actualInfoItemViewModels = viewModel.InfoItemViewModels;
            Assert.True(comparer.Compare(expectedInfoItemViewModels, actualInfoItemViewModels).AreEqual);
        }

        [Fact]
        public void OnAppearing_Always_LoadFooterInfo()
        {
            // Arrange
            matchService.GetMatch(viewModel.SettingsService.UserSettings, match.Id, false).Returns(match);

            // Act
            viewModel.OnAppearing();

            // Assert
            Assert.Equal("18:00 01 Jan, 2019", viewModel.DisplayEventDate);
            Assert.Equal("2,034", viewModel.DisplayAttendance);
            Assert.Equal("My Dinh", viewModel.DisplayVenue);
        }

        [Fact]
        public void OnReceivingMatchEvent_IsCurrentMatch_BuildGeneralInfo()
        {
            // Arrange
            match.TimeLines.Returns(new List<ITimeline>
            {
                new Timeline { Type = "yellow_card", Time = new DateTime(2019, 01, 01, 18, 00, 00) },
            });

            var matchResult = Substitute.For<IMatchResult>();
            matchResult.EventStatus.Returns(new MatchStatus { Value = MatchStatus.Abandoned });
            matchResult.HomeScore.Returns(1);
            matchResult.AwayScore.Returns(2);
            var timelines = new List<ITimeline>
            {
                new Timeline { Type = "red_card", Time = new DateTime(2019, 01, 01, 18, 00, 00) },
            };

            var pushEvents = new Dictionary<string, MatchPushEvent>
            {
                {"1234", new MatchPushEvent { MatchResult = matchResult, TimeLines = timelines } }
            };

            // Act
            viewModel.OnReceivingMatchEvent("1", pushEvents);

            // Assert
            Assert.Equal(matchResult, viewModel.MatchViewModel.Match.MatchResult);
            Assert.Equal(timelines[0], viewModel.MatchViewModel.Match.LatestTimeline);

            var expectedTimelines = match.TimeLines.Concat(timelines).Distinct();
            Assert.Equal(expectedTimelines, viewModel.MatchViewModel.Match.TimeLines);
            Assert.Equal("Abandoned", viewModel.MatchViewModel.DisplayMatchStatus);
            Assert.Equal("1 - 2", viewModel.DisplayScore);
        }

        [Fact]
        public void OnReceivingMatchEvent_IsCurrentMatch_CurrentTimelinesIsNull_BuildGeneralInfo()
        {
            // Arrange
            var parameters = new NavigationParameters { { "Match", new Match { Id = "1234" } } };
            viewModel.OnNavigatingTo(parameters);

            var timelines = new List<ITimeline>
            {
                new Timeline { Type = "red_card", Time = new DateTime(2019, 01, 01, 18, 00, 00) },
            };

            var pushEvents = new Dictionary<string, MatchPushEvent>
            {
                {"1234", new MatchPushEvent {  TimeLines = timelines } }
            };

            // Act
            viewModel.OnReceivingMatchEvent("1", pushEvents);

            // Assert
            Assert.Equal(timelines, viewModel.MatchViewModel.Match.TimeLines);
        }

        [Fact]
        public void OnReceivingMatchEvent_IsNotCurrentMatch_Return()
        {
            // Arrange
            var timelines = new List<ITimeline>
            {
                new Timeline { Type = "red_card", Time = new DateTime(2019, 01, 01, 18, 00, 00) },
            };

            var pushEvents = new Dictionary<string, MatchPushEvent>
            {
                { "1", new MatchPushEvent{ TimeLines = timelines } }
            };

            // Act
            viewModel.OnReceivingMatchEvent("1", pushEvents);

            // Assert
            Assert.Empty(viewModel.MatchViewModel.Match.TimeLines);
        }

        [Fact]
        public async Task RefreshCommand_OnExecuted_LoadGeneralInfo()
        {
            // Arrange
            matchService.GetMatch(viewModel.SettingsService.UserSettings, match.Id, true).Returns(match);

            // Act
            await viewModel.RefreshCommand.ExecuteAsync();

            // Assert
            Assert.Equal("01 Jan, 2019 - LALIGA", viewModel.DisplayEventDateAndLeagueName);
        }

        [Fact]
        public void OnDisappearing_Alway_Clean()
        {
            // Arrange
            viewModel.OnAppearing();

            // Act
            viewModel.OnDisappearing();
        }
    }
}