namespace Soccer.Tests
{
    using LiveScore.Core.Converters;
    using LiveScore.Core.Services;
    using LiveScore.Soccer;
    using LiveScore.Soccer.Converters;
    using LiveScore.Soccer.Services;
    using LiveScore.Soccer.Views.Templates;
    using NSubstitute;
    using Prism.Ioc;
    using Xamarin.Forms;
    using Xunit;

    public class SoccerModuleTests
    {
        [Fact]
        public void OnInitialized_DoNothing()
        {
            // Arrange
            var soccerModule = new SoccerModule();
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
            var soccerModule = new SoccerModule();
            var container = Substitute.For<IContainerRegistry>();

            // Act
            soccerModule.RegisterTypes(container);

            // Assert
            container.Received(1).Register<IMatchService, MatchService>("1");
            container.Received(1).Register<IOddsService, OddsService>("1");
            //container.Received(1).Register<IHubService, HubService>("1");
            container.Received(1).Register<IMatchStatusConverter, MatchStatusConverter>("1");
            container.Received(1).Register<DataTemplate, MatchDataTemplate>("1");
        }
    }
}