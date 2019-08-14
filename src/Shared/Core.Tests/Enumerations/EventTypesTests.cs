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
            var actual = EventType.RedCard;

            // Assert
            Assert.Equal(8, actual.Value);
            Assert.Equal("red_card", actual.DisplayName);
        }

        [Fact]
        public void ScoreChange_Always_CreateCorrectEventType()
        {
            // Act
            var actual = EventType.ScoreChange;

            // Assert
            Assert.Equal(9, actual.Value);
            Assert.Equal("score_change", actual.DisplayName);
        }

        [Fact]
        public void Substitution_Always_CreateCorrectEventType()
        {
            // Act
            var actual = EventType.Substitution;

            // Assert
            Assert.Equal(10, actual.Value);
            Assert.Equal("substitution", actual.DisplayName);
        }

        [Fact]
        public void YellowCard_Always_CreateCorrectEventType()
        {
            // Act
            var actual = EventType.YellowCard;

            // Assert
            Assert.Equal(11, actual.Value);
            Assert.Equal("yellow_card", actual.DisplayName);
        }

        [Fact]
        public void YellowRedCard_Always_CreateCorrectEventType()
        {
            // Act
            var actual = EventType.YellowRedCard;

            // Assert
            Assert.Equal(12, actual.Value);
            Assert.Equal("yellow_red_card", actual.DisplayName);
        }

        [Fact]
        public void Constructor_DoNothing()
        {
            // Act
            new EventType();

            // Assert
            Assert.True(true);
        }
    }
}