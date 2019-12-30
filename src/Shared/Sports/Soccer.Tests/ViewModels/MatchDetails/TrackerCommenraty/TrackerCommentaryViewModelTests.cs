using System;
using System.Threading.Tasks;
using AutoFixture;
using LiveScore.Common.Services;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Models.Matches;
using LiveScore.Core.Tests.Fixtures;
using LiveScore.Soccer.Models.Matches;
using LiveScore.Soccer.Services;
using LiveScore.Soccer.ViewModels.Matches.MatchDetails.TrackerCommentary;
using NSubstitute;
using Prism.Events;
using Xunit;

namespace Soccer.Tests.ViewModels.MatchDetails.TrackerCommentary
{
    public class TrackerCommentaryViewModelTests : IClassFixture<ViewModelBaseFixture>, IClassFixture<ResourcesFixture>
    {
        private const string MatchId = "sr:match:1";
        private static readonly DateTime MatchEventDate = new DateTime(2019, 01, 01);
        private readonly ISoccerMatchService matchService;
        private readonly ILoggingService logService;
        private readonly IEventAggregator eventAggregator;
        private readonly Fixture fixture;
        private readonly ViewModelBaseFixture baseFixture;

        public TrackerCommentaryViewModelTests(ViewModelBaseFixture baseFixture)
        {
            this.baseFixture = baseFixture;
            fixture = baseFixture.CommonFixture.Specimens;

            matchService = Substitute.For<ISoccerMatchService>();
            logService = Substitute.For<ILoggingService>();

            eventAggregator = Substitute.For<IEventAggregator>();
            var networkConnectionManager = Substitute.For<INetworkConnection>();
            networkConnectionManager.IsSuccessfulConnection().Returns(true);

            baseFixture.DependencyResolver.Resolve<ISoccerMatchService>().Returns(matchService);
            baseFixture.DependencyResolver.Resolve<ILoggingService>().Returns(logService);
            baseFixture.DependencyResolver.Resolve<INetworkConnection>().Returns(networkConnectionManager);
        }

        [Fact]
        public async Task LoadTrackerAndCommentaries_CoverageIsNull_ReturnHasDataFalse()
        {
            var viewModel = new TrackerCommentaryViewModel(
                MatchId,
               null,
                DateTime.Now,
                baseFixture.NavigationService,
                baseFixture.DependencyResolver,
                eventAggregator,
                null);

            await viewModel.LoadTrackerAndCommentaries();

            Assert.False(viewModel.HasData);
        }

        [Fact]
        public async Task LoadTrackerAndCommentaries_NotCoverageLive_ReturnHasTrackerDataFalse()
        {
            var matchCoverage = fixture.Create<Coverage>()
                .With(c => c.Live, false);

            var viewModel = new TrackerCommentaryViewModel(
                MatchId,
                matchCoverage,
                DateTime.Now,
                baseFixture.NavigationService,
                baseFixture.DependencyResolver,
                eventAggregator,
                null);

            await viewModel.LoadTrackerAndCommentaries();

            Assert.False(viewModel.HasTrackerData);
            Assert.Null(viewModel.WidgetContent);
        }

        [Fact]
        public async Task LoadTrackerAndCommentaries_CoverageLive_ReturnHasTrackerDataTrue()
        {
            var matchCoverage = fixture.Create<Coverage>()
                .With(c => c.Live, true);

            var viewModel = new TrackerCommentaryViewModel(
                MatchId,
                matchCoverage,
                MatchEventDate,
                baseFixture.NavigationService,
                baseFixture.DependencyResolver,
                eventAggregator,
                null);

            await viewModel.LoadTrackerAndCommentaries();

            Assert.True(viewModel.HasTrackerData);
            Assert.NotNull(viewModel.WidgetContent);
        }

        [Fact]
        public async Task LoadTrackerAndCommentaries_NotCoverageCommentary_ReturnHasDataTrue()
        {
            var matchCoverage = fixture.Create<Coverage>()
                .With(c => c.Live, true)
                .With(coverage => coverage.Commentary, false);

            var viewModel = new TrackerCommentaryViewModel(
                MatchId,
                matchCoverage,
                MatchEventDate,
                baseFixture.NavigationService,
                baseFixture.DependencyResolver,
                eventAggregator,
                null);

            await viewModel.LoadTrackerAndCommentaries();

            Assert.True(viewModel.HasData);
        }

        [Fact]
        public async Task LoadTrackerAndCommentaries_CoverageCommentary_EmptyCommentary_ReturnHasCommentariesDataFalse()
        {
            var matchCoverage = fixture.Create<Coverage>()
                .With(coverage => coverage.Commentary, true);

            var viewModel = new TrackerCommentaryViewModel(
                MatchId,
                matchCoverage,
                MatchEventDate,
                baseFixture.NavigationService,
                baseFixture.DependencyResolver,
                eventAggregator,
                null);

            await viewModel.LoadTrackerAndCommentaries();

            Assert.False(viewModel.HasCommentariesData);
        }

        [Fact]
        public async Task LoadTrackerAndCommentaries_CoverageCommentary_ReturnHasCommentariesDataTrue()
        {
            var matchCoverage = fixture.Create<Coverage>()
                .With(coverage => coverage.Commentary, true);

            var commentaries = fixture.CreateMany<MatchCommentary>();
            matchService.GetMatchCommentariesAsync(MatchId, Arg.Any<Language>(), MatchEventDate).Returns(commentaries);

            var viewModel = new TrackerCommentaryViewModel(
                MatchId,
                matchCoverage,
                MatchEventDate,
                baseFixture.NavigationService,
                baseFixture.DependencyResolver,
                eventAggregator,
                null);

            await viewModel.LoadTrackerAndCommentaries();

            Assert.True(viewModel.HasCommentariesData);
        }

        [Fact]
        public async Task LoadTrackerAndCommentaries_CoverageCommentary_ReturnVisibleShowMoreTrue()
        {
            var matchCoverage = fixture.Create<Coverage>()
                .With(coverage => coverage.Commentary, true);

            var commentaries = fixture.CreateMany<MatchCommentary>();
            matchService.GetMatchCommentariesAsync(MatchId, Arg.Any<Language>(), MatchEventDate).Returns(commentaries);

            var viewModel = new TrackerCommentaryViewModel(
                MatchId,
                matchCoverage,
                MatchEventDate,
                baseFixture.NavigationService,
                baseFixture.DependencyResolver,
                eventAggregator,
                null);

            await viewModel.LoadTrackerAndCommentaries();

            Assert.False(viewModel.VisibleShowMore);
        }

        [Fact]
        public async Task ShowMoreCommentaries_IsShowMoreTrue_ReverseIsShowMore()
        {
            var matchCoverage = fixture.Create<Coverage>()
                .With(coverage => coverage.Commentary, true);

            var commentaries = fixture.CreateMany<MatchCommentary>();
            matchService.GetMatchCommentariesAsync(MatchId, Arg.Any<Language>(), MatchEventDate).Returns(commentaries);

            var viewModel = new TrackerCommentaryViewModel(
                MatchId,
                matchCoverage,
                MatchEventDate,
                baseFixture.NavigationService,
                baseFixture.DependencyResolver,
                eventAggregator,
               null);

            await viewModel.LoadTrackerAndCommentaries();

            viewModel.ShowMoreCommentaries();

            Assert.False(viewModel.IsShowMore);
        }

        [Fact]
        public async Task ShowMoreCommentaries_IsShowMoreFalse_ReverseIsShowMore()
        {
            // Arrange
            var matchCoverage = fixture.Create<Coverage>()
                .With(coverage => coverage.Commentary, true);

            var commentaries = fixture.CreateMany<MatchCommentary>();
            matchService.GetMatchCommentariesAsync(MatchId, Arg.Any<Language>(), MatchEventDate).Returns(commentaries);

            var viewModel = new TrackerCommentaryViewModel(
                MatchId,
                matchCoverage,
                MatchEventDate,
                baseFixture.NavigationService,
                baseFixture.DependencyResolver,
                eventAggregator,
                null);

            await viewModel.LoadTrackerAndCommentaries();
            viewModel.ShowMoreCommentaries();

            // Act
            viewModel.ShowMoreCommentaries();

            // Assert
            Assert.True(viewModel.IsShowMore);
        }
    }
}