using System;
using AutoFixture;
using LiveScore.Core.Converters;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Models.Matches;
using LiveScore.Core.Tests.Fixtures;
using LiveScore.Soccer.Models.Matches;
using LiveScore.Soccer.ViewModels.Matches.MatchDetails.HeadToHead;
using NSubstitute;
using Xunit;

namespace Soccer.Tests.ViewModels.DetailH2H
{
    public class H2HMatchViewModelTests : IClassFixture<ViewModelBaseFixture>
    {
        private readonly IMatchDisplayStatusBuilder matchDisplayStatusBuilder;
        private readonly IMatch match;

        private readonly Fixture fixture;

        public H2HMatchViewModelTests(ViewModelBaseFixture baseFixture)
        {
            fixture = baseFixture.CommonFixture.Specimens;

            matchDisplayStatusBuilder = Substitute.For<IMatchDisplayStatusBuilder>();
            match = Substitute.For<IMatch>();
        }

        [Fact]
        public void DisplayMatchStatus_FullTime_CorrectAssignedValue()
        {
            matchDisplayStatusBuilder.BuildDisplayStatus(match).Returns("FT");

            var viewModel = new H2HMatchViewModel(true, string.Empty, match, matchDisplayStatusBuilder);

            Assert.Equal("FT", viewModel.DisplayMatchStatus);
        }

        [Fact]
        public void BuildDisplayEventDate_InCurrentYear_ShouldDisplayLocalShortDayMonth()
        {
            match.EventDate.Returns(new DateTimeOffset(new DateTime(DateTime.Now.Year, 10, 12, 9, 0, 0)));

            var viewModel = new H2HMatchViewModel(true, string.Empty, match, matchDisplayStatusBuilder);

            Assert.Equal("12 Oct", viewModel.DisplayEventDate);
        }

        [Fact]
        public void BuildDisplayEventDate_InPastYear_ShouldDisplayYear()
        {
            match.EventDate.Returns(new DateTimeOffset(new DateTime(2000, 10, 12, 9, 0, 0)));

            var viewModel = new H2HMatchViewModel(true, string.Empty, match, matchDisplayStatusBuilder);

            Assert.Equal("2000", viewModel.DisplayEventDate);
        }

        [Fact]
        public void BuildDisplayEventDate_InFuture_ShouldDisplayYear()
        {
            match.EventDate.Returns(new DateTimeOffset(new DateTime(2099, 10, 12, 9, 0, 0)));

            var viewModel = new H2HMatchViewModel(true, string.Empty, match, matchDisplayStatusBuilder);

            Assert.Equal("2099", viewModel.DisplayEventDate);
        }

        [Fact]
        public void BuildMatch_MatchIsNull_ShouldReturn()
        {
            var viewModel = new H2HMatchViewModel(true, string.Empty, null, matchDisplayStatusBuilder);

            Assert.Null(viewModel.Match);
        }

        [Fact]
        public void BuildMatch_TeamIsWinner_ResultShouldBeWin()
        {
            var matchData = fixture.Create<SoccerMatch>().With(match => match.WinnerId, "sr:team");

            var viewModel = new H2HMatchViewModel(false, "sr:team", matchData, matchDisplayStatusBuilder);

            Assert.Equal(TeamResult.Win.DisplayName, viewModel.Result);
        }

        [Fact]
        public void BuildMatch_TeamIsLose_ResultShouldBeLose()
        {
            var matchData = fixture.Create<SoccerMatch>().With(match => match.WinnerId, "sr:team:1");

            var viewModel = new H2HMatchViewModel(false, "sr:team", matchData, matchDisplayStatusBuilder);

            Assert.Equal(TeamResult.Lose.DisplayName, viewModel.Result);
        }

        [Fact]
        public void BuildMatch_Draw_ResultShouldBeDraw()
        {
            var matchData = fixture.Create<SoccerMatch>().With(match => match.WinnerId, "");

            var viewModel = new H2HMatchViewModel(false, "sr:team", matchData, matchDisplayStatusBuilder);

            Assert.Equal(TeamResult.Draw.DisplayName, viewModel.Result);
        }
    }
}