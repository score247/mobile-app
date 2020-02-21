using LiveScore.Core.Converters;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Services;
using LiveScore.Soccer;
using LiveScore.Soccer.Models.Matches;
using LiveScore.Soccer.Models.TimelineImages;
using LiveScore.Soccer.Services;
using LiveScore.Soccer.Views.Matches.Templates;
using NSubstitute;
using Prism.Ioc;
using Xamarin.Forms;
using Xunit;

namespace Soccer.Tests
{
    public class SoccerModuleTests
    {
        [Fact]
#pragma warning disable S2699 // Tests should include assertions
        public void RegisterTypes_Always_RegisterTypeWithSoccer()
        {
            // Arrange
            var soccerModule = new SoccerModule();
            var container = Substitute.For<IContainerRegistry>();

            // Act
            soccerModule.RegisterTypes(container);

            // Assert
            container.Received(1).RegisterSingleton<IMatchService, MatchService>("1");
            container.Received(1).RegisterSingleton<IMatchDisplayStatusBuilder, MatchStatusBuilder>("1");
            container.Received(1).RegisterSingleton<DataTemplate, MatchDataTemplate>("1");
            container.Received(1).RegisterSingleton<IHubService, SoccerHubService>("1");
            container.Received(1).RegisterSingleton<ISoccerMatchService, MatchService>();
            container.Received(1).RegisterSingleton<ITimelineEventImageBuilder, DefaultEventImageBuilder>();
            container.Received(1).RegisterSingleton<ITimelineEventImageBuilder, ScoreChangeImageBuilder>(EventType.ScoreChange.Value.ToString());
            container.Received(1).RegisterSingleton<ITimelineEventImageBuilder, PenaltyShootOutImageBuilder>(EventType.PenaltyShootout.Value.ToString());
        }

#pragma warning restore S2699 // Tests should include assertions
    }
}