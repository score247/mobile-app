namespace Soccer.Tests.ViewModels.DetailOdds.OddsItems
{
    using System;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Odds;
    using LiveScore.Core.Tests.Fixtures;
    using LiveScore.Soccer.Enumerations;
    using LiveScore.Soccer.ViewModels.MatchDetails.Odds.OddItems;
    using LiveScore.Soccer.ViewModels.MatchDetails.Odds.OddItems.AsianHdp;
    using LiveScore.Soccer.ViewModels.MatchDetails.Odds.OddItems.OneXTwo;
    using LiveScore.Soccer.ViewModels.MatchDetails.Odds.OddItems.OverUnder;
    using LiveScore.Soccer.Views.Templates.MatchDetails.Odds.OddsItems.AsianHdp;
    using LiveScore.Soccer.Views.Templates.MatchDetails.Odds.OddsItems.OneXTwo;
    using LiveScore.Soccer.Views.Templates.MatchDetails.Odds.OddsItems.OverUnder;
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
        [InlineData(2, typeof(OverUnderMovementItemViewModel))]
        [InlineData(3, typeof(AsianHdpMovementItemViewModel))]
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
        [InlineData(2, typeof(OverUnderMovementItemTemplate))]
        [InlineData(3, typeof(AsianHdpMovementItemTemplate))]
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
