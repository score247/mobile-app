using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using KellermanSoftware.CompareNetObjects;
using LiveScore.Common;
using LiveScore.Core.Converters;
using LiveScore.Core.Enumerations;
using LiveScore.Core.PubSubEvents.Matches;
using LiveScore.Core.Services;
using LiveScore.Core.Tests.Fixtures;
using LiveScore.Core.ViewModels;
using LiveScore.Soccer.Models.Matches;
using LiveScore.Soccer.PubSubEvents.Matches;
using LiveScore.Soccer.ViewModels.Leagues.LeagueDetails.Fixtures;
using NSubstitute;
using Prism.Commands;
using Xunit;

namespace Soccer.Tests.ViewModels.Leagues.LeagueDetails.Fixtures
{
    public class FixturesMatchesViewModelTests : IClassFixture<ViewModelBaseFixture>, IClassFixture<ResourcesFixture>
    {
        private const string CurrentLeagueId = "league:1";
        private const string CurrentLeagueGroupName = "UEFA Champion League:: Group A";
        private readonly ViewModelBaseFixture baseFixture;
        private readonly FixturesMatchesViewModel matchesViewModel;
        private readonly ILeagueService leagueService;
        private readonly Fixture specimens;
        private readonly CompareLogic comparer;
        private readonly IMatchDisplayStatusBuilder matchDisplayStatusBuilder;
        private readonly IMatchMinuteBuilder matchMinuteBuilder;
        private readonly Func<string, string> buildFlagUrlFunc;

        public FixturesMatchesViewModelTests(ViewModelBaseFixture baseFixture)
        {
            this.baseFixture = baseFixture;
            specimens = new Fixture();
            comparer = new CompareLogic();
            leagueService = Substitute.For<ILeagueService>();
            matchDisplayStatusBuilder = Substitute.For<IMatchDisplayStatusBuilder>();
            matchMinuteBuilder = Substitute.For<IMatchMinuteBuilder>();
            buildFlagUrlFunc = (_) => string.Empty;

            this.baseFixture.DependencyResolver.Resolve<IMatchDisplayStatusBuilder>("1").Returns(matchDisplayStatusBuilder);
            this.baseFixture.DependencyResolver.Resolve<IMatchMinuteBuilder>("1").Returns(matchMinuteBuilder);
            this.baseFixture.DependencyResolver.Resolve<Func<string, string>>(FuncNameConstants.BuildFlagUrlFuncName).Returns(buildFlagUrlFunc);
            this.baseFixture.DependencyResolver.Resolve<ILeagueService>("1").Returns(leagueService);
            this.baseFixture.NetworkConnection.IsSuccessfulConnection().Returns(true);

            matchesViewModel = new FixturesMatchesViewModel(
                CurrentLeagueId,
                CurrentLeagueGroupName,
                this.baseFixture.NavigationService,
                this.baseFixture.DependencyResolver,
                this.baseFixture.EventAggregator);
        }

        [Fact]
        public async Task OnAppearing_Always_SubscribeEvents_AndLoadDataFromService()
        {
            // Arrange
            leagueService
                .GetFixtures(CurrentLeagueId, CurrentLeagueGroupName, baseFixture.AppSettingsFixture.Settings.CurrentLanguage)
                .Returns(new List<SoccerMatch> {
                    specimens.Create<SoccerMatch>()
                        .With(m => m.Id, "match:1")
                        .With(m => m.MatchStatus, MatchStatus.NotStarted)
                });
            matchesViewModel.OnAppearing();
            await Task.Delay(100);

            // Act
            baseFixture.EventAggregator
                .GetEvent<MatchEventPubSubEvent>()
                .Publish(new MatchEventMessage(1, new MatchEvent("match:1",
                    specimens.Create<MatchResult>().With(result => result.MatchStatus, MatchStatus.Live),
                    null)));

            // Assert
            var expectedMatch = matchesViewModel.MatchItemsSource
                    .SelectMany(group => group)
                    .FirstOrDefault(m => m.Match != null && m.Match.Id == "match:1");
            Assert.Equal(MatchStatus.Live, expectedMatch.Match.MatchStatus);
        }

        [Fact]
        public async Task OnResumeWhenNetworkOK_Always_SubscribeEvents_AndLoadDataFromService()
        {
            // Arrange
            leagueService
                .GetFixtures(CurrentLeagueId, CurrentLeagueGroupName, baseFixture.AppSettingsFixture.Settings.CurrentLanguage)
                .Returns(new List<SoccerMatch> {
                    specimens.Create<SoccerMatch>()
                        .With(m => m.Id, "match:1")
                        .With(m => m.MatchStatus, MatchStatus.NotStarted)
                });
            matchesViewModel.OnResumeWhenNetworkOK();
            await Task.Delay(100);

            // Act
            baseFixture.EventAggregator
                .GetEvent<MatchEventPubSubEvent>()
                .Publish(new MatchEventMessage(1, new MatchEvent("match:1",
                    specimens.Create<MatchResult>().With(result => result.MatchStatus, MatchStatus.Live),
                    null)));

            // Assert
            var expectedMatch = matchesViewModel.MatchItemsSource
                    .SelectMany(group => group)
                    .FirstOrDefault(m => m.Match != null && m.Match.Id == "match:1");
            Assert.Equal(MatchStatus.Live, expectedMatch.Match.MatchStatus);
        }

        [Fact]
        public async Task InitializeMatchItems_NoData_ShowNoData()
        {
            // Arrange
            leagueService
                .GetFixtures(CurrentLeagueId, CurrentLeagueGroupName, baseFixture.AppSettingsFixture.Settings.CurrentLanguage)
                .Returns(new List<SoccerMatch>());

            // Act
            matchesViewModel.OnAppearing();
            await Task.Delay(100);

            // Assert
            Assert.False(matchesViewModel.HasData);
        }

        [Fact]
        public async Task InitializeMatchItems_FixturesAndResultHasLessThanTenMatches_LoadExpectedMatchItemSource()
        {
            // Arrange
            var matches = new List<SoccerMatch> {
                    specimens.Create<SoccerMatch>()
                            .With(m => m.Id, "match:6")
                            .With(m => m.EventDate, new DateTime(2020, 01, 02))
                            .With(m => m.MatchStatus, MatchStatus.NotStarted),
                    specimens.Create<SoccerMatch>()
                            .With(m => m.Id, "match:5")
                            .With(m => m.EventDate, new DateTime(2020, 01, 03))
                            .With(m => m.MatchStatus, MatchStatus.Cancelled),
                    specimens.Create<SoccerMatch>()
                            .With(m => m.Id, "match:2")
                            .With(m => m.EventDate, new DateTime(2018, 01, 03))
                            .With(m => m.MatchStatus, MatchStatus.Postponed),
                    specimens.Create<SoccerMatch>()
                            .With(m => m.Id, "match:1")
                            .With(m => m.EventDate, new DateTime(2019, 01, 02))
                            .With(m => m.MatchStatus, MatchStatus.Closed),
                };
            leagueService
                .GetFixtures(CurrentLeagueId, CurrentLeagueGroupName, baseFixture.AppSettingsFixture.Settings.CurrentLanguage)
                .Returns(matches);

            // Act
            matchesViewModel.OnAppearing();
            await Task.Delay(200);

            // Assert
            var expectedMatches = new List<SoccerMatch> { matches[2], matches[3], matches[0], matches[1] };
            var expectedMatchItems = expectedMatches
                    .Select(match => new MatchViewModel(match, matchDisplayStatusBuilder, matchMinuteBuilder, baseFixture.EventAggregator))
                    .GroupBy(item => new MatchGroupViewModel(item.Match, buildFlagUrlFunc, baseFixture.NavigationService, matchesViewModel.CurrentSportId, matchesViewModel.EnableTapLeague));
            var expectedMatchItemSource = new ObservableCollection<IGrouping<MatchGroupViewModel, MatchViewModel>>(expectedMatchItems);

            Assert.True(comparer.Compare(expectedMatchItemSource, matchesViewModel.MatchItemsSource).AreEqual);
            Assert.False(matchesViewModel.ShowLoadFixturesButton);
            Assert.False(matchesViewModel.ShowLoadResultsButton);
            Assert.Null(matchesViewModel.ResultMatchItemSource);
            Assert.Null(matchesViewModel.FixturesMatchItemSource);
        }

        [Fact]
        public async Task InitializeMatchItems_FixturesHasMoreThanTenMatches_LoadExpectedMatchItemSource()
        {
            // Arrange
            var fixtures = BuildFixtures(11, MatchStatus.NotStarted);
            var liveMatches = BuildFixtures(1, MatchStatus.Live);
            var postponeFixtures = BuildFixtures(1, MatchStatus.Postponed);
            var results = BuildResults(3, MatchStatus.Closed);
            leagueService
                .GetFixtures(CurrentLeagueId, CurrentLeagueGroupName, baseFixture.AppSettingsFixture.Settings.CurrentLanguage)
                .Returns(fixtures.Concat(results).Concat(postponeFixtures).Concat(liveMatches));

            // Act
            matchesViewModel.OnAppearing();
            await Task.Delay(200);

            // Assert
            var expectedMatches = fixtures.Concat(postponeFixtures).Concat(liveMatches).OrderBy(m => m.EventDate).Take(10);
            var expectedMatchItems = expectedMatches
                    .Select(match => new MatchViewModel(match, matchDisplayStatusBuilder, matchMinuteBuilder, baseFixture.EventAggregator))
                    .GroupBy(item => new MatchGroupViewModel(item.Match, buildFlagUrlFunc, baseFixture.NavigationService, matchesViewModel.CurrentSportId, matchesViewModel.EnableTapLeague));
            var expectedMatchItemSource = new ObservableCollection<IGrouping<MatchGroupViewModel, MatchViewModel>>(expectedMatchItems);

            Assert.True(comparer.Compare(expectedMatchItemSource, matchesViewModel.MatchItemsSource).AreEqual);
            Assert.True(matchesViewModel.ShowLoadFixturesButton);
            Assert.True(matchesViewModel.ShowLoadResultsButton);

            var expectedFixtures = fixtures.Concat(liveMatches).Concat(postponeFixtures).OrderBy(m => m.EventDate).Skip(10);
            var expectedFixtureItems = new ObservableCollection<IGrouping<MatchGroupViewModel, MatchViewModel>>(expectedFixtures
                    .Select(match => new MatchViewModel(match, matchDisplayStatusBuilder, matchMinuteBuilder, baseFixture.EventAggregator))
                    .GroupBy(item => new MatchGroupViewModel(item.Match, buildFlagUrlFunc, baseFixture.NavigationService, matchesViewModel.CurrentSportId, matchesViewModel.EnableTapLeague)));
            Assert.True(comparer.Compare(expectedFixtureItems, matchesViewModel.FixturesMatchItemSource).AreEqual);
        }

        [Fact]
        public async Task InitializeMatchItems_FixturesHasLessThanTenMatches_ResultHasMoreThanTenMatches_LoadExpectedMatchItemSource_ShowResultButton()
        {
            // Arrange
            var fixtures = BuildFixtures(1, MatchStatus.NotStarted);
            var liveMatches = BuildFixtures(1, MatchStatus.Live);
            var postponeFixtures = BuildFixtures(1, MatchStatus.Postponed);
            var results = BuildResults(10, MatchStatus.Closed);
            leagueService
                .GetFixtures(CurrentLeagueId, CurrentLeagueGroupName, baseFixture.AppSettingsFixture.Settings.CurrentLanguage)
                .Returns(fixtures.Concat(results).Concat(postponeFixtures).Concat(liveMatches));

            // Act
            matchesViewModel.OnAppearing();
            await Task.Delay(200);

            // Assert
            var expectedResult = results.OrderBy(m => m.EventDate);
            var expectedMatches = expectedResult.TakeLast(7).Concat(fixtures).Concat(postponeFixtures).Concat(liveMatches).OrderBy(m => m.EventDate);
            var expectedMatchItems = expectedMatches
                    .Select(match => new MatchViewModel(match, matchDisplayStatusBuilder, matchMinuteBuilder, baseFixture.EventAggregator))
                    .GroupBy(item => new MatchGroupViewModel(item.Match, buildFlagUrlFunc, baseFixture.NavigationService, matchesViewModel.CurrentSportId, matchesViewModel.EnableTapLeague));
            var expectedMatchItemSource = new ObservableCollection<IGrouping<MatchGroupViewModel, MatchViewModel>>(expectedMatchItems);

            Assert.True(comparer.Compare(expectedMatchItemSource, matchesViewModel.MatchItemsSource).AreEqual);
            Assert.False(matchesViewModel.ShowLoadFixturesButton);
            Assert.True(matchesViewModel.ShowLoadResultsButton);

            var expectedResults = expectedResult.SkipLast(7);
            var expectedResultItems = expectedResults
                    .Select(match => new MatchViewModel(match, matchDisplayStatusBuilder, matchMinuteBuilder, baseFixture.EventAggregator))
                    .GroupBy(item => new MatchGroupViewModel(item.Match, buildFlagUrlFunc, baseFixture.NavigationService, matchesViewModel.CurrentSportId, matchesViewModel.EnableTapLeague))
                    .Reverse();
            Assert.True(comparer.Compare(expectedResultItems, matchesViewModel.ResultMatchItemSource).AreEqual);
        }

        [Fact]
        public async Task LoadResultMatches_ResultsMatchItemHasData_LoadMoreResultMatches()
        {
            // Arrange
            var results = BuildResults(15, MatchStatus.Closed);
            leagueService
                .GetFixtures(CurrentLeagueId, CurrentLeagueGroupName, baseFixture.AppSettingsFixture.Settings.CurrentLanguage)
                .Returns(results);
            matchesViewModel.OnAppearing();
            IGrouping<MatchGroupViewModel, MatchViewModel> scrollItem = null;
            Action<IGrouping<MatchGroupViewModel, MatchViewModel>> onScroll = (item) => scrollItem = item;
            matchesViewModel.ScrollToCommand = new DelegateCommand<IGrouping<MatchGroupViewModel, MatchViewModel>>(onScroll);
            await Task.Delay(200);

            // Act
            matchesViewModel.LoadResultMatchesCommand.Execute();
            await Task.Delay(200);

            // Assert
            var expectedMatches = results.OrderBy(m => m.EventDate);
            var expectedMatchItems = expectedMatches
                    .Select(match => new MatchViewModel(match, matchDisplayStatusBuilder, matchMinuteBuilder, baseFixture.EventAggregator))
                    .GroupBy(item => new MatchGroupViewModel(item.Match, buildFlagUrlFunc, baseFixture.NavigationService, matchesViewModel.CurrentSportId, matchesViewModel.EnableTapLeague));
            var expectedMatchItemSource = new ObservableCollection<IGrouping<MatchGroupViewModel, MatchViewModel>>(expectedMatchItems);

            Assert.True(comparer.Compare(expectedMatchItemSource, matchesViewModel.MatchItemsSource).AreEqual);
            Assert.False(matchesViewModel.ShowLoadResultsButton);
            Assert.Null(matchesViewModel.ResultMatchItemSource);
            Assert.Null(matchesViewModel.HeaderViewModel);
            Assert.Equal(scrollItem, matchesViewModel.MatchItemsSource[5]);
        }

        [Fact]
        public async Task LoadFixtureMatches_FixtureMatchItemHasData_LoadMoreFixtureMatches()
        {
            // Arrange
            var matches = BuildFixtures(15, MatchStatus.NotStarted);
            leagueService
                .GetFixtures(CurrentLeagueId, CurrentLeagueGroupName, baseFixture.AppSettingsFixture.Settings.CurrentLanguage)
                .Returns(matches);
            matchesViewModel.OnAppearing();
            await Task.Delay(100);

            // Act
            matchesViewModel.LoadFixtureMatchesCommand.Execute();
            await Task.Delay(100);

            // Assert
            var expectedMatches = matches.OrderBy(m => m.EventDate);
            var expectedMatchItems = expectedMatches
                    .Select(match => new MatchViewModel(match, matchDisplayStatusBuilder, matchMinuteBuilder, baseFixture.EventAggregator))
                    .GroupBy(item => new MatchGroupViewModel(item.Match, buildFlagUrlFunc, baseFixture.NavigationService, matchesViewModel.CurrentSportId, matchesViewModel.EnableTapLeague));
            var expectedMatchItemSource = new ObservableCollection<IGrouping<MatchGroupViewModel, MatchViewModel>>(expectedMatchItems);

            Assert.True(comparer.Compare(expectedMatchItemSource, matchesViewModel.MatchItemsSource).AreEqual);
            Assert.False(matchesViewModel.ShowLoadFixturesButton);
            Assert.Null(matchesViewModel.FixturesMatchItemSource);
        }

        private List<SoccerMatch> BuildResults(int count, MatchStatus matchStatus)
        {
            var matches = new List<SoccerMatch>();

            for (var i = 1; i <= count; i++)
            {
                var match = specimens.Create<SoccerMatch>()
                            .With(m => m.Id, "closed-match:" + i + DateTime.Now.ToString())
                            .With(m => m.EventDate, DateTime.Now.AddDays(-i))
                            .With(m => m.MatchStatus, matchStatus);

                matches.Add(match);
            }

            return matches;
        }

        private List<SoccerMatch> BuildFixtures(int count, MatchStatus matchStatus)
        {
            var matches = new List<SoccerMatch>();

            for (var i = 1; i <= count; i++)
            {
                var match = specimens.Create<SoccerMatch>()
                            .With(m => m.Id, "match:" + i + DateTime.Now.ToString())
                            .With(m => m.EventDate, DateTime.Now.AddDays(i))
                            .With(m => m.MatchStatus, matchStatus);

                matches.Add(match);
            }

            return matches;
        }
    }
}