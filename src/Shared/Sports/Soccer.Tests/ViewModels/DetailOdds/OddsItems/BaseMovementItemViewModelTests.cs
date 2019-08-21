namespace Soccer.Tests.ViewModels.DetailOdds.OddsItems
{
    using System;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Odds;
    using LiveScore.Core.Tests.Fixtures;
    using LiveScore.Soccer.Enumerations;
    using LiveScore.Soccer.ViewModels.DetailOdds.OddItems;
    using LiveScore.Soccer.Views.Templates.DetailOdds.OddsItems;
    using NSubstitute;
    using Xunit;

    public class BaseMovementItemViewModelTests : IClassFixture<ViewModelBaseFixture>, IClassFixture<ResourcesFixture>
    {

        private readonly ViewModelBaseFixture baseFixture;
        private readonly IOddsMovement oddsMovement;

        public BaseMovementItemViewModelTests(ViewModelBaseFixture baseFixture)
        {
            oddsMovement = Substitute.For<IOddsMovement>();

            this.baseFixture = baseFixture;
        }

        [Theory]
        [InlineData(1, typeof(OneXTwoMovementItemViewModel))]
        [InlineData(2, typeof(AsianHdpMovementItemViewModel))]
        [InlineData(3, typeof(OverUnderMovementItemViewModel))]
        public void CreateInstance_Always_GetExpectedViewModelInstance(byte betTypeId, Type expectedType)
        {
            // Arrange         
            var betType = Enumeration.FromValue<BetType>(betTypeId);
            var viewModel = new BaseMovementItemViewModel(betType, oddsMovement, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            var actual = viewModel.CreateInstance();

            // Assert
            Assert.Equal(expectedType, actual.GetType());
        }

        [Theory]
        [InlineData(1, typeof(OneXTwoMovementItemTemplate))]
        [InlineData(2, typeof(AsianHdpMovementItemTemplate))]
        [InlineData(3, typeof(OverUnderMovementItemTemplate))]
        public void CreateTemplate_Always_GetExpectedTemplate(byte betTypeId, Type expectedType)
        {
            // Arrange
            var betType = Enumeration.FromValue<BetType>(betTypeId);
            var viewModel = new BaseMovementItemViewModel(betType, oddsMovement, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            var actual = viewModel.CreateTemplate();

            // Assert
            Assert.Equal(expectedType, actual.GetType());
        }
    }
}
