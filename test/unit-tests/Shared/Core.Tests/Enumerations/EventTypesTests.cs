namespace LiveScore.Core.Tests.Enumerations
{
    using LiveScore.Core.Enumerations;
    using Xunit;

    public class EventTypesTests
    {
        [Fact]
        public void RedCard_Always_CreateCorrectEventType()
        {
            // Act
            var actual = EventTypes.RedCardType;

            // Assert
            Assert.Equal("red_card", actual.Value);
            Assert.Equal("RedCard", actual.DisplayName);
        }

        [Fact]
        public void ScoreChange_Always_CreateCorrectEventType()
        {
            // Act
            var actual = EventTypes.ScoreChangeType;

            // Assert
            Assert.Equal("score_change", actual.Value);
            Assert.Equal("ScoreChange", actual.DisplayName);
        }

        [Fact]
        public void Substitution_Always_CreateCorrectEventType()
        {
            // Act
            var actual = EventTypes.SubstitutionType;

            // Assert
            Assert.Equal("substitution", actual.Value);
            Assert.Equal("Substitution", actual.DisplayName);
        }

        [Fact]
        public void YellowCard_Always_CreateCorrectEventType()
        {
            // Act
            var actual = EventTypes.YellowCardType;

            // Assert
            Assert.Equal("yellow_card", actual.Value);
            Assert.Equal("YellowCard", actual.DisplayName);
        }

        [Fact]
        public void YellowRedCard_Always_CreateCorrectEventType()
        {
            // Act
            var actual = EventTypes.YellowRedCardType;

            // Assert
            Assert.Equal("yellow_red_card", actual.Value);
            Assert.Equal("YellowRedCard", actual.DisplayName);
        }

        [Fact]
        public void Constructor_DoNothing()
        {
            // Act
            new EventTypes();

            // Assert
            Assert.True(true);
        }
    }
}