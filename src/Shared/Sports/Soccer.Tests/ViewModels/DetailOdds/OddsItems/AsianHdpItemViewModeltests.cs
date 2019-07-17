﻿namespace Soccer.Tests.ViewModels.DetailOdds.OddsItems
{
    using System.Collections.Generic;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Odds;
    using LiveScore.Core.Tests.Fixtures;
    using LiveScore.Soccer.Enumerations;
    using LiveScore.Soccer.ViewModels.DetailOdds.OddItems;
    using NSubstitute;
    using Xunit;

    public class AsianHdpItemViewModeltests : IClassFixture<ViewModelBaseFixture>, IClassFixture<ResourcesFixture>
    {

        private readonly ViewModelBaseFixture baseFixture;
        private readonly IBetTypeOdds betTypeOdds;

        public AsianHdpItemViewModeltests(ViewModelBaseFixture baseFixture)
        {
            betTypeOdds = Substitute.For<IBetTypeOdds>();
            betTypeOdds.Bookmaker.Returns(new Bookmaker { Id = "book1", Name = "book1" });
            betTypeOdds.BetOptions.Returns(new List<BetOptionOdds>
            {
                new BetOptionOdds{ Type = "home", LiveOdds = 5.000m, OpeningOdds = 4.900m, OddsTrend = OddsTrend.Up, OpeningOptionValue= "1.25", OptionValue = "1.5" },               
                new BetOptionOdds{ Type = "away", LiveOdds = 2.500m, OpeningOdds = 2.800m, OddsTrend = OddsTrend.Down, OpeningOptionValue= "2", OptionValue = "2.5" }
            });

            this.baseFixture = baseFixture;
        }

        [Fact]
        public void HomeLiveOdds_Always_GetExpectedFormat()
        {
            // Arrange               
            var viewModel = new AsianHdpItemViewModel(BetType.AsianHDP, betTypeOdds, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            viewModel.CreateInstance();

            // Assert
            Assert.Equal("5.00", viewModel.HomeLiveOdds);
        }

        [Fact]
        public void HdpValue_Always_GetExpectedFormat()
        {
            // Arrange               
            var viewModel = new AsianHdpItemViewModel(BetType.AsianHDP, betTypeOdds, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            viewModel.CreateInstance();

            // Assert
            Assert.Equal("1.25", viewModel.OpeningHdp);
            Assert.Equal("1.5", viewModel.LiveHdp);
        }

        [Fact]
        public void AwayLiveOdds_Always_GetExpectedFormat()
        {
            // Arrange               
            var viewModel = new AsianHdpItemViewModel(BetType.AsianHDP, betTypeOdds, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            viewModel.CreateInstance();

            // Assert
            Assert.Equal("2.50", viewModel.AwayLiveOdds);
        }
    }
}