namespace Soccer.Tests.ViewModels.DetailOdds.OddsItems
{
    using System;
    using LiveScore.Core.Models.Odds;
    using LiveScore.Core.Tests.Fixtures;
    using LiveScore.Soccer.Enumerations;
    using LiveScore.Soccer.ViewModels.DetailOdds.OddItems;
    using LiveScore.Soccer.Views.Templates.DetailOdds.OddsItems;
    using NSubstitute;
    using Xunit;

    public class BaseItemViewModelTests : IClassFixture<ViewModelBaseFixture>, IClassFixture<ResourcesFixture>
    {

        private readonly ViewModelBaseFixture baseFixture;
        private readonly IBetTypeOdds betTypeOdds;

        public BaseItemViewModelTests(ViewModelBaseFixture baseFixture)
        {
            betTypeOdds = Substitute.For<IBetTypeOdds>();
            betTypeOdds.Bookmaker.Returns(new Bookmaker { Id = "book1", Name = "book1" });

            this.baseFixture = baseFixture;
        }

        [Theory]
        [InlineData(BetType.OneXTwo, typeof(OneXTwoItemViewModel))]
        [InlineData(BetType.AsianHDP, typeof(AsianHdpItemViewModel))]
        [InlineData(BetType.OverUnder, typeof(OverUnderItemViewModel))]
        public void CreateInstance_Always_GetExpectedViewModelInstance(BetType betType, Type expectedType)
        {
            // Arrange               
            var viewModel = new BaseItemViewModel(betType, betTypeOdds, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            var actual = viewModel.CreateInstance();

            // Assert
            Assert.Equal(expectedType, actual.GetType());
        }

        [Theory]
        [InlineData(BetType.OneXTwo, typeof(OneXTwoItemTemplate))]
        [InlineData(BetType.AsianHDP, typeof(AsianHdpItemTemplate))]
        [InlineData(BetType.OverUnder, typeof(OverUnderItemTemplate))]
        public void CreateTemplate_Always_GetExpectedTemplate(BetType betType, Type expectedType)
        {
            // Arrange
            var viewModel = new BaseItemViewModel(betType, betTypeOdds, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            var actual = viewModel.CreateTemplate();

            // Assert
            Assert.Equal(expectedType, actual.GetType());
        }
    }
}
