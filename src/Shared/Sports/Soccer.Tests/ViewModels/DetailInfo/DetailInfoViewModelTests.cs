namespace Soccer.Tests.ViewModels.MatchDetailInfo
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using KellermanSoftware.CompareNetObjects;
    using LiveScore.Common.Services;
    using LiveScore.Core.Converters;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Leagues;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Models.Teams;
    using LiveScore.Core.Services;
    using LiveScore.Core.Tests.Fixtures;
    using LiveScore.Soccer.Converters;
    using LiveScore.Soccer.Models.Matches;
    using LiveScore.Soccer.ViewModels.MatchDetailInfo;
    using NSubstitute;
    using Prism.Navigation;
    using Xunit;

    public class DetailInfoViewModelTests : IClassFixture<ViewModelBaseFixture>
    {
        private readonly DetailInfoViewModel viewModel;
        private readonly IMatchService matchService;
        private readonly CompareLogic comparer;
        private readonly Match match;
        private readonly ILocalStorage localStorage;

        public DetailInfoViewModelTests(ViewModelBaseFixture baseFixture)
        {
            comparer = baseFixture.CommonFixture.Comparer;
            localStorage = Substitute.For<ILocalStorage>();
            baseFixture.DependencyResolver.Resolve<IMatchStatusConverter>("1")
                .Returns(new MatchStatusConverter(localStorage));
            matchService = Substitute.For<IMatchService>();
            baseFixture.DependencyResolver.Resolve<IMatchService>("1").Returns(matchService);
            match = CreateMatch();

            viewModel = new DetailInfoViewModel(
                match.Id,
                baseFixture.NavigationService,
                baseFixture.DependencyResolver,
                baseFixture.HubService.BuildMatchEventHubConnection(),
                null);

            var parameters = new NavigationParameters { { "Match", match } };
            viewModel.OnNavigatingTo(parameters);
        }

        private Match CreateMatch()
        {
            var matchData = new Match
            {
                Id = "1234",
                League = new League { Name = "Laliga" },
                EventDate = new DateTime(2019, 01, 01, 18, 00, 00),
                Attendance = 2034,
                Venue = new Venue { Name = "My Dinh" },
                MatchResult = new MatchResult
                {
                    HomeScore = 5,
                    AwayScore = 1,
                    EventStatus = MatchStatus.Live,
                    MatchStatus = MatchStatus.FirstHalf
                },
                Teams = new List<Team>
            {
                new Team { Id = "home", Name = "Barcelona" },
                new Team { Id = "away", Name = "Real Marid"}
            }
            };

            return matchData;
        }

        [Fact]
        public void OnAppearing_Always_LoadMatchDetail()
        {
            // Arrange
            var returnMatch = CreateMatch();
            var returnTimelines = new List<ITimelineEvent>
            {
                new TimelineEvent { Id = "1", Type = EventTypes.RedCard, Time = new DateTime(2019, 01, 01, 18, 00, 00) },
                new TimelineEvent { Id = "2", Type = EventTypes.YellowRedCard, Time = new DateTime(2019, 01, 01, 17, 00, 00 )},
                new TimelineEvent { Id = "3", Type = EventTypes.Substitution, Time = new DateTime(2019, 01, 01, 17, 15, 00 )},
                new TimelineEvent { Id = "4", Type = EventTypes.RedCard, Time = new DateTime(2019, 01, 01, 17, 45, 00 )},
                new TimelineEvent { Id = "5", Type = EventTypes.BreakStart, PeriodType = PeriodTypes.Pause, Time = new DateTime(2019, 01, 01, 17, 50, 00 )},
                new TimelineEvent { Id = "6", Type = EventTypes.PenaltyMissed, Time = new DateTime(2019, 01, 01, 18, 30, 00 )},
                new TimelineEvent { Id = "7", Type = EventTypes.ScoreChange, Time = new DateTime(2019, 01, 01, 17, 55, 00 )},
                new TimelineEvent { Id = "8", Type = EventTypes.BreakStart, PeriodType = PeriodTypes.ExtraTimeHalfTime, Time = new DateTime(2019, 01, 01, 18, 40, 00 )},
                new TimelineEvent { Id = "10", Type = EventTypes.PenaltyShootout, Time = new DateTime(2019, 01, 01, 19, 00, 00 )},
                new TimelineEvent { Id = "11", Type = EventTypes.MatchEnded, Time = new DateTime(2019, 01, 01, 19, 50, 00 )},
            };
            returnMatch.TimeLines = returnTimelines;
            returnMatch.MatchResult = new MatchResult
            {
                MatchStatus = MatchStatus.EndedAfterPenalties,
                EventStatus = MatchStatus.Live
            };
            matchService.GetMatch(viewModel.SettingsService.UserSettings, match.Id, false).Returns(returnMatch);

            // Act
            viewModel.OnAppearing();

            // Assert
            // Expected not include substition and match_ended
            var expectedInfoItemViewModels = new ObservableCollection<BaseItemViewModel>
            {
                new BaseItemViewModel(returnTimelines[9], returnMatch.MatchResult, viewModel.NavigationService, viewModel.DependencyResolver).CreateInstance(),
                new BaseItemViewModel(returnTimelines[8], returnMatch.MatchResult, viewModel.NavigationService, viewModel.DependencyResolver).CreateInstance(),
                new BaseItemViewModel(returnTimelines[7], returnMatch.MatchResult, viewModel.NavigationService, viewModel.DependencyResolver).CreateInstance(),
                new BaseItemViewModel(returnTimelines[5], returnMatch.MatchResult, viewModel.NavigationService, viewModel.DependencyResolver).CreateInstance(),
                new BaseItemViewModel(returnTimelines[0], returnMatch.MatchResult, viewModel.NavigationService, viewModel.DependencyResolver).CreateInstance(),
                new BaseItemViewModel(returnTimelines[6], returnMatch.MatchResult, viewModel.NavigationService, viewModel.DependencyResolver).CreateInstance(),
                new BaseItemViewModel(returnTimelines[4], returnMatch.MatchResult, viewModel.NavigationService, viewModel.DependencyResolver).CreateInstance(),
                new BaseItemViewModel(returnTimelines[3], returnMatch.MatchResult, viewModel.NavigationService, viewModel.DependencyResolver).CreateInstance(),
                new BaseItemViewModel(returnTimelines[1], returnMatch.MatchResult, viewModel.NavigationService, viewModel.DependencyResolver).CreateInstance(),
            };
            var actualInfoItemViewModels = viewModel.InfoItemViewModels;
            Assert.True(comparer.Compare(expectedInfoItemViewModels, actualInfoItemViewModels).AreEqual);
        }

        [Fact]
        public void OnAppearing_MatchNotPenalty_LoadMatchDetail()
        {
            // Arrange
            var returnMatch = CreateMatch();
            var returnTimelines = new List<ITimelineEvent>
            {
                new TimelineEvent { Id = "1", Type = EventTypes.MatchEnded, Time = new DateTime(2019, 01, 01, 19, 50, 00 )},
            };
            returnMatch.TimeLines = returnTimelines;
            returnMatch.MatchResult = new MatchResult
            {
                MatchStatus = MatchStatus.EndedExtraTime,
                EventStatus = MatchStatus.Closed
            };
            matchService.GetMatch(viewModel.SettingsService.UserSettings, match.Id, false).Returns(returnMatch);

            // Act
            viewModel.OnAppearing();

            // Assert
            var expectedInfoItemViewModels = new ObservableCollection<BaseItemViewModel>
            {
                new BaseItemViewModel(returnTimelines[0], returnMatch.MatchResult, viewModel.NavigationService, viewModel.DependencyResolver).CreateInstance()
            };

            var actualInfoItemViewModels = viewModel.InfoItemViewModels;
            Assert.True(comparer.Compare(expectedInfoItemViewModels, actualInfoItemViewModels).AreEqual);
        }

        [Fact]
        public void OnAppearing_PostMatch_IgnorePenaltyShootOutFirstShoot()
        {
            // Arrange
            var returnMatch = CreateMatch();
            returnMatch.TimeLines = new List<ITimelineEvent>
            {
                new TimelineEvent { Type = EventTypes.PenaltyShootout, IsFirstShoot = true, Time = new DateTime(2019, 01, 01, 19, 50, 00 )},
            };
            returnMatch.MatchResult = new MatchResult
            {
                MatchStatus = MatchStatus.EndedAfterPenalties,
                EventStatus = MatchStatus.Closed
            };
            matchService.GetMatch(viewModel.SettingsService.UserSettings, match.Id, false).Returns(returnMatch);

            // Act
            viewModel.OnAppearing();

            // Assert
            var expectedInfoItemViewModels = new ObservableCollection<BaseItemViewModel>();
            var actualInfoItemViewModels = viewModel.InfoItemViewModels;
            Assert.True(comparer.Compare(expectedInfoItemViewModels, actualInfoItemViewModels).AreEqual);
        }

        [Fact]
        public void OnAppearing_LiveMatch_InPenalties_RemoveFirstShootIfHavingNewEvent()
        {
            // Arrange
            var returnMatch = CreateMatch();
            var timelines = new List<ITimelineEvent>
            {
                new TimelineEvent { Id = "1", Type = EventTypes.PenaltyShootout, IsFirstShoot = true, Time = new DateTime(2019, 01, 01, 19, 50, 00 )},
                new TimelineEvent { Id = "2", Type = EventTypes.PenaltyShootout, IsFirstShoot = false, Time = new DateTime(2019, 01, 01, 19, 50, 00 )},
            };
            returnMatch.TimeLines = timelines;
            returnMatch.MatchResult = new MatchResult
            {
                MatchStatus = MatchStatus.Penalties,
                EventStatus = MatchStatus.Live
            };
            matchService.GetMatch(viewModel.SettingsService.UserSettings, match.Id, false).Returns(returnMatch);

            // Act
            viewModel.OnAppearing();

            // Assert
            var expectedInfoItemViewModels = new ObservableCollection<BaseItemViewModel>
            {
                new BaseItemViewModel(timelines[1], returnMatch.MatchResult, viewModel.NavigationService, viewModel.DependencyResolver).CreateInstance()
            };
            var actualInfoItemViewModels = viewModel.InfoItemViewModels;
            Assert.True(comparer.Compare(expectedInfoItemViewModels, actualInfoItemViewModels).AreEqual);
        }

        [Fact]
        public void OnAppearing_LiveMatch_InPenalties_KeepFirstShoolEventInLast()
        {
            // Arrange
            var returnMatch = CreateMatch();
            var timelines = new List<ITimelineEvent>
            {
                new TimelineEvent { Id = "1", Type = EventTypes.PenaltyShootout, IsFirstShoot = true, Time = new DateTime(2019, 01, 01, 19, 50, 00 )},
                new TimelineEvent { Id = "2", Type = EventTypes.PenaltyShootout, IsFirstShoot = false, Time = new DateTime(2019, 01, 01, 19, 50, 00 )},
                new TimelineEvent { Id = "3", Type = EventTypes.PenaltyShootout, IsFirstShoot = true, Time = new DateTime(2019, 01, 01, 19, 50, 00 )},
            };
            returnMatch.TimeLines = timelines;
            returnMatch.MatchResult = new MatchResult
            {
                MatchStatus = MatchStatus.Penalties,
                EventStatus = MatchStatus.Live
            };
            matchService.GetMatch(viewModel.SettingsService.UserSettings, match.Id, false).Returns(returnMatch);

            // Act
            viewModel.OnAppearing();

            // Assert
            var expectedInfoItemViewModels = new ObservableCollection<BaseItemViewModel>
            {
                new BaseItemViewModel(timelines[1], returnMatch.MatchResult, viewModel.NavigationService, viewModel.DependencyResolver).CreateInstance(),
                new BaseItemViewModel(timelines[2], returnMatch.MatchResult, viewModel.NavigationService, viewModel.DependencyResolver).CreateInstance()
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
        public void OnReceivingMatchEvent_IsCurrentMatch_BuildDetailInfo()
        {
            // Arrange
            matchService.GetMatch(viewModel.SettingsService.UserSettings, match.Id, false).Returns(match);
            viewModel.OnAppearing();

            match.TimeLines = new List<ITimelineEvent>
            {
                new TimelineEvent { Id = "1", Type = EventTypes.YellowCard, Time = new DateTime(2019, 01, 01, 18, 00, 00) },
            };

            var matchResult = new MatchResult
            {
                EventStatus = MatchStatus.Abandoned,
                HomeScore = 1,
                AwayScore = 2
            };

            var timeline = new TimelineEvent { Id = "2", Type = EventTypes.RedCard, Time = new DateTime(2019, 01, 01, 18, 00, 00) };

            var matchEvent = new MatchEvent("1234", matchResult, timeline);

            // Act
            viewModel.OnReceivingMatchEvent(1, matchEvent);

            // Assert
            Assert.Equal(matchResult, viewModel.Match.MatchResult);
            match.TimeLines.ToList().Add(timeline);
            var expectedTimelines = match.TimeLines.Distinct();
            Assert.Equal(expectedTimelines, viewModel.Match.TimeLines);
        }

        [Fact]
        public void OnReceivingMatchEvent_IsCurrentMatch_CurrentTimelinesIsNull_BuildGeneralInfo()
        {
            // Arrange
            matchService.GetMatch(viewModel.SettingsService.UserSettings, match.Id, false).Returns(match);
            viewModel.OnAppearing();

            var timeline = new TimelineEvent { Id = "1", Type = EventTypes.RedCard, Time = new DateTime(2019, 01, 01, 18, 00, 00) };

            var matchEvent = new MatchEvent("1234", null, timeline);

            viewModel.OnAppearing();

            // Act
            viewModel.OnReceivingMatchEvent(1, matchEvent);

            // Assert
            Assert.Contains(timeline, viewModel.Match.TimeLines);
        }

        [Fact]
        public void OnReceivingMatchEvent_IsNotCurrentMatch_Return()
        {
            // Arrange
            matchService.GetMatch(viewModel.SettingsService.UserSettings, match.Id, false).Returns(match);
            viewModel.OnAppearing();
            var timeline = new TimelineEvent { Type = EventTypes.RedCard, Time = new DateTime(2019, 01, 01, 18, 00, 00) };
            var matchEvent = new MatchEvent("1", null, timeline);

            viewModel.OnAppearing();

            // Act
            viewModel.OnReceivingMatchEvent(1, matchEvent);

            // Assert
            Assert.Null(viewModel.Match.TimeLines);
        }
    }
}