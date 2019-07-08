namespace Basketball.Tests
{
    using LiveScore.Basketball;
    using LiveScore.Basketball.Services;
    using LiveScore.Basketball.Views.Templates;
    using LiveScore.Core.Services;
    using NSubstitute;
    using Prism.Ioc;
    using Xamarin.Forms;
    using Xunit;

    public class BasketballModuleTests
    {
        [Fact]
        public void OnInitialized_DoNothing()
        {
            // Arrange
            var soccerModule = new BasketballModule();
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
            var soccerModule = new BasketballModule();
            var container = Substitute.For<IContainerRegistry>();

            // Act
            soccerModule.RegisterTypes(container);

            // Assert
            container.Received(1).Register<IMatchService, MatchService>("2");
            container.Received(1).Register<DataTemplate, MatchDataTemplate>("2");
        }
    }
}