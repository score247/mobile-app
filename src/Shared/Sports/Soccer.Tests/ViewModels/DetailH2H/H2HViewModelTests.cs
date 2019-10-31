﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using LiveScore.Common;
using LiveScore.Common.Services;
using LiveScore.Core.Converters;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Models.Matches;
using LiveScore.Core.Services;
using LiveScore.Core.Tests.Fixtures;
using LiveScore.Soccer.Models.Matches;
using LiveScore.Soccer.ViewModels.MatchDetails.HeadToHead;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Prism.Events;
using Xunit;

namespace Soccer.Tests.ViewModels.DetailH2H
{
    public class H2HViewModelTests : IClassFixture<ViewModelBaseFixture>
    {   
        private readonly IMatch match;
        private readonly ITeamService teamService;
        private readonly IMatchDisplayStatusBuilder matchStatusBuilder;
        private readonly ILoggingService logService;
        private readonly IEventAggregator eventAggregator;
        private readonly Func<string, string> buildFlagUrlFunc;

        private readonly H2HViewModel viewModel;

        public H2HViewModelTests(ViewModelBaseFixture baseFixture) 
        {
            match = baseFixture.CommonFixture.Specimens.Create<SoccerMatch>();

            teamService = Substitute.For<ITeamService>();
            matchStatusBuilder = Substitute.For<IMatchDisplayStatusBuilder>();
            logService = Substitute.For<ILoggingService>();
            buildFlagUrlFunc = Substitute.For<Func<string, string>>();
            eventAggregator = Substitute.For<IEventAggregator>();

            baseFixture.DependencyResolver.Resolve<ITeamService>("1").Returns(teamService);
            baseFixture.DependencyResolver.Resolve<IMatchDisplayStatusBuilder>("1").Returns(matchStatusBuilder);
            baseFixture.DependencyResolver.Resolve<Func<string, string>>(FuncNameConstants.BuildFlagUrlFuncName).Returns(buildFlagUrlFunc);
            baseFixture.DependencyResolver.Resolve<ILoggingService>().Returns(logService);

            viewModel = new H2HViewModel(
                match, 
                baseFixture.NavigationService, 
                baseFixture.DependencyResolver, 
                eventAggregator, 
                null);
        }

        [Fact]
        public async Task LoadHeadToHeadAsync_Always_InjectGetHeadToHeadsAsync() 
        {
            // Arrange               

            // Act
            await viewModel.LoadHeadToHeadAsync(false);

            // Assert
            await teamService.Received(1).GetHeadToHeadsAsync(match.HomeTeamId, match.AwayTeamId, viewModel.CurrentLanguage.DisplayName, false);
            Assert.True(viewModel.VisibleHeadToHead);
            Assert.False(viewModel.HasData);
        }

        [Fact]
        public async Task LoadHeadToHeadAsync_Exception_ShouldLogError()
        {
            // Arrange               
            teamService.GetHeadToHeadsAsync(match.HomeTeamId, match.AwayTeamId, viewModel.CurrentLanguage.DisplayName, false)
                .Throws(new InvalidOperationException("Error"));

            // Act
            await viewModel.LoadHeadToHeadAsync(false);

            // Assert
            await logService.Received(1).LogExceptionAsync(Arg.Any<InvalidOperationException>());
            Assert.True(viewModel.VisibleHeadToHead);
            Assert.False(viewModel.HasData);
        }

        [Fact]
        public async Task LoadHeadToHeadAsync_NotStartMatches_ShouldHideStats()
        {
            // Arrange               
            teamService.GetHeadToHeadsAsync(match.HomeTeamId, match.AwayTeamId, viewModel.CurrentLanguage.DisplayName, false)
                .Returns(new List<SoccerMatch> 
                { 
                    new SoccerMatch(new MatchResult(MatchStatus.NotStarted, MatchStatus.NotStarted)),
                    new SoccerMatch(new MatchResult(MatchStatus.NotStarted, MatchStatus.NotStarted))
                });

            // Act
            await viewModel.LoadHeadToHeadAsync(false);

            // Assert
            Assert.True(viewModel.VisibleHeadToHead);
            Assert.True(viewModel.HasData);
            Assert.False(viewModel.VisibleStats);
        }

        [Fact]
        public async Task LoadHeadToHeadAsync_ContainsClosedMatches_ShouldVisibleStats()
        {
            // Arrange               
            teamService.GetHeadToHeadsAsync(match.HomeTeamId, match.AwayTeamId, viewModel.CurrentLanguage.DisplayName, false)
                .Returns(new List<SoccerMatch>
                {
                    new SoccerMatch(new MatchResult(MatchStatus.NotStarted, MatchStatus.NotStarted)),
                    new SoccerMatch(new MatchResult(MatchStatus.Closed, MatchStatus.Ended))
                });

            // Act
            await viewModel.LoadHeadToHeadAsync(false);

            // Assert
            Assert.True(viewModel.VisibleHeadToHead);
            Assert.True(viewModel.HasData);
            Assert.True(viewModel.VisibleStats);
        }

        [Fact]
        public async Task RefreshAsync_Always_GetLatest()
        {
            // Arrange       

            // Act
            await viewModel.RefreshAsync();

            // Assert
            await teamService.Received(1).GetHeadToHeadsAsync(match.HomeTeamId, match.AwayTeamId, viewModel.CurrentLanguage.DisplayName, true);
            Assert.False(viewModel.IsRefreshing);
        }

        [Fact]
        public void LoadTeamResult_Home_VisibleHomeTeamResults()
        {
            // Arrange       

            // Act
            viewModel.LoadTeamResult("home");

            // Assert
            Assert.True(viewModel.VisibleHomeResults);
            Assert.False(viewModel.VisibleAwayResults);
            Assert.False(viewModel.VisibleHeadToHead);
        }

        [Fact]
        public void LoadTeamResult_Away_VisibleAwayTeamResults()
        {
            // Arrange       

            // Act
            viewModel.LoadTeamResult("away");

            // Assert
            Assert.False(viewModel.VisibleHomeResults);
            Assert.True(viewModel.VisibleAwayResults);
            Assert.False(viewModel.VisibleHeadToHead);
        }
    }
}
