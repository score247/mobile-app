using LiveScore.Features.Score;
using LiveScore.Features.Score.ViewModels;
using LiveScore.Features.Score.Views;

namespace LiveScore.Features.Tests
{
    using NSubstitute;
    using Prism.Ioc;
    using Xunit;

    public class ScoreModuleTests
    {
        [Fact]
        public void OnInitialized_DoNothing()
        {
            // Arrange
            var soccerModule = new ScoreModule();
            var container = Substitute.For<IContainerProvider>();

            // Act
            soccerModule.OnInitialized(container);

            // Assert
            Assert.True(true);
        }

        [Fact]
#pragma warning disable S2699 // Tests should include assertions
        public void RegisterTypes_Always_RegisterTypeWithSoccer()
#pragma warning restore S2699 // Tests should include assertions
        {
            // Arrange
            var soccerModule = new ScoreModule();
            var container = Substitute.For<IContainerRegistry>();

            // Act
            soccerModule.RegisterTypes(container);

            // Assert
            container.Received(1).RegisterForNavigation<ScoresView, ScoreMatchesViewModel>();
        }
    }
}