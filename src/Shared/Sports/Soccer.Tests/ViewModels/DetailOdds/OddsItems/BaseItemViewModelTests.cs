namespace Soccer.Tests.ViewModels.DetailOdds.OddsItems
{
    using System;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Odds;
    using LiveScore.Core.Tests.Fixtures;
    using LiveScore.Soccer.Enumerations;
    using LiveScore.Soccer.ViewModels.MatchDetails.DetailOdds.OddItems;
    using LiveScore.Soccer.Views.Templates.MatchDetails.DetailOdds.OddsItems.AsianHdp;
    using LiveScore.Soccer.Views.Templates.MatchDetails.DetailOdds.OddsItems.OneXTwo;
    using LiveScore.Soccer.Views.Templates.MatchDetails.DetailOdds.OddsItems.OverUnder;
    using NSubstitute;
    using Xunit;

    public class BaseItemViewModelTests : IClassFixture<ViewModelBaseFixture>, IClassFixture<ResourcesFixture>
    {

        private readonly ViewModelBaseFixture baseFixture;
        private readonly IBetTypeOdds betTypeOdds;

        public BaseItemViewModelTests(ViewModelBaseFixture baseFixture)
        {
            betTypeOdds = Substitute.For<IBetTypeOdds>();
            betTypeOdds.Bookmaker.Returns(new Bookmaker("1", "William"));

            this.baseFixture = baseFixture;
        }

        [Theory]
        [InlineData(1, typeof(OneXTwoItemViewModel))]
        [InlineData(2, typeof(OverUnderItemViewModel))]
        [InlineData(3, typeof(AsianHdpItemViewModel))]
        public void CreateInstance_Always_GetExpectedViewModelInstance(byte betTypeId, Type expectedType)
        {
            // Arrange               
            var betType = Enumeration.FromValue<BetType>(betTypeId);
            var viewModel = new BaseItemViewModel(betType, betTypeOdds, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            var actual = viewModel.CreateInstance();

            // Assert
            Assert.Equal(expectedType, actual.GetType());
        }

        [Theory]
        [InlineData(1, typeof(OneXTwoItemTemplate))]      
        [InlineData(2, typeof(OverUnderItemTemplate))]
        [InlineData(3, typeof(AsianHdpItemTemplate))]
        public void CreateTemplate_Always_GetExpectedTemplate(byte betTypeId, Type expectedType)
        {
            // Arrange
            var betType = Enumeration.FromValue<BetType>(betTypeId);
            var viewModel = new BaseItemViewModel(betType, betTypeOdds, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            var actual = viewModel.CreateTemplate();

            // Assert
            Assert.Equal(expectedType, actual.GetType());
        }
    }
}
