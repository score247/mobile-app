namespace Scores.Tests
{
    using LiveScore.Score;
    using LiveScore.Score.ViewModels;
    using LiveScore.Score.Views;
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
        public void RegisterTypes_Always_RegisterTypeWithSoccer()
        {
            // Arrange
            var soccerModule = new ScoreModule();
            var container = Substitute.For<IContainerRegistry>();

            // Act
            soccerModule.RegisterTypes(container);

            // Assert
            container.Received(1).RegisterForNavigation<ScoresView, ScoresViewModel>();
            container.Received(1).RegisterForNavigation<LiveView, LiveViewModel>();
        }
    }
}