﻿using System;
using LiveScore.Core.Converters;
using LiveScore.Core.Models.Matches;
using LiveScore.Soccer.ViewModels.MatchDetails.DetailH2H;
using NSubstitute;
using Xunit;

namespace Soccer.Tests.ViewModels.DetailH2H
{
    public class SummaryMatchViewModelTests
    {

        private readonly IMatchDisplayStatusBuilder matchDisplayStatusBuilder;
        private readonly IMatch match;

        public SummaryMatchViewModelTests()
        {
            matchDisplayStatusBuilder = Substitute.For<IMatchDisplayStatusBuilder>();
            match = Substitute.For<IMatch>();
        }

        [Fact]
        public void DisplayMatchStatus_FullTime_CorrectAssignedValue()
        {   
            matchDisplayStatusBuilder.BuildDisplayStatus(match).Returns("FT");

            var viewModel = new SummaryMatchViewModel(match, matchDisplayStatusBuilder);

            Assert.Equal("FT", viewModel.DisplayMatchStatus);
        }

        [Fact]
        public void BuildDisplayEventDate_InCurrentYear_ShouldDisplayLocalShortDayMonth()
        {
            match.EventDate.Returns(new DateTimeOffset(new DateTime(DateTime.Now.Year, 10, 12, 9, 0, 0)));

            var viewModel = new SummaryMatchViewModel(match, matchDisplayStatusBuilder);

            Assert.Equal("12 Oct", viewModel.DisplayEventDate);
        }

        [Fact]
        public void BuildDisplayEventDate_InPastYear_ShouldDisplayYear()
        {
            match.EventDate.Returns(new DateTimeOffset(new DateTime(2000, 10, 12, 9, 0, 0)));

            var viewModel = new SummaryMatchViewModel(match, matchDisplayStatusBuilder);

            Assert.Equal("2000", viewModel.DisplayEventDate);
        }

        [Fact]
        public void BuildDisplayEventDate_InFuture_ShouldDisplayYear()
        {
            match.EventDate.Returns(new DateTimeOffset(new DateTime(2099, 10, 12, 9, 0, 0)));

            var viewModel = new SummaryMatchViewModel(match, matchDisplayStatusBuilder);

            Assert.Equal("2099", viewModel.DisplayEventDate);
        }
    }
}