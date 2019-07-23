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
        [InlineData(BetType.OneXTwo, typeof(OneXTwoMovementItemViewModel))]
        public void CreateInstance_Always_GetExpectedViewModelInstance(BetType betType, Type expectedType)
        {
            // Arrange               
            var viewModel = new BaseMovementItemViewModel(betType, oddsMovement, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            var actual = viewModel.CreateInstance();

            // Assert
            Assert.Equal(expectedType, actual.GetType());
        }

        [Theory]
        [InlineData(BetType.OneXTwo, typeof(OneXTwoMovementItemTemplate))]
        public void CreateTemplate_Always_GetExpectedTemplate(BetType betType, Type expectedType)
        {
            // Arrange
            var viewModel = new BaseMovementItemViewModel(betType, oddsMovement, baseFixture.NavigationService, baseFixture.DependencyResolver);

            // Act
            var actual = viewModel.CreateTemplate();

            // Assert
            Assert.Equal(expectedType, actual.GetType());
        }
    }
}
