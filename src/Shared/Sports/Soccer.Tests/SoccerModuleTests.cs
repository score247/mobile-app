using LiveScore.Core.Converters;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Services;
using LiveScore.Soccer;
using LiveScore.Soccer.Converters;
using LiveScore.Soccer.Converters.TimelineImages;
using LiveScore.Soccer.Services;
using LiveScore.Soccer.Views.Templates;
using NSubstitute;
using Prism.Ioc;
using Xamarin.Forms;
using Xunit;

namespace Soccer.Tests
{
    public class SoccerModuleTests
    {
        [Fact]
        public void RegisterTypes_Always_RegisterTypeWithSoccer()
        {
            // Arrange
            var soccerModule = new SoccerModule();
            var container = Substitute.For<IContainerRegistry>();

            // Act
            soccerModule.RegisterTypes(container);

            // Assert
            container.Received(1).RegisterSingleton<IMatchService, MatchService>("1");
            container.Received(1).RegisterSingleton<IOddsService, OddsService>("1");
            container.Received(1).RegisterSingleton<IMatchDisplayStatusBuilder, MatchStatusConverter>("1");
            container.Received(1).RegisterSingleton<DataTemplate, MatchDataTemplate>("1");
            container.Received(1).RegisterSingleton<IHubService, SoccerHubService>("1");
            container.Received(1).RegisterSingleton<ISoccerMatchService, MatchService>();
            container.Received(1).RegisterSingleton<ITimelineEventImageConverter, DefaultEventImageConverter>();
            container.Received(1).RegisterSingleton<ITimelineEventImageConverter, ScoreChangeImageConverter>(EventType.ScoreChange.Value.ToString());
            container.Received(1).RegisterSingleton<ITimelineEventImageConverter, PenaltyShootOutImageConverter>(EventType.PenaltyShootout.Value.ToString());
        }
    }
}