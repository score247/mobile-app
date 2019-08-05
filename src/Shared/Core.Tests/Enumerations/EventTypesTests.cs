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
            var actual = EventTypes.RedCard;

            // Assert
            Assert.Equal(8, actual.Value);
            Assert.Equal("RedCard", actual.DisplayName);
        }

        [Fact]
        public void ScoreChange_Always_CreateCorrectEventType()
        {
            // Act
            var actual = EventTypes.ScoreChange;

            // Assert
            Assert.Equal(9, actual.Value);
            Assert.Equal("ScoreChange", actual.DisplayName);
        }

        [Fact]
        public void Substitution_Always_CreateCorrectEventType()
        {
            // Act
            var actual = EventTypes.Substitution;

            // Assert
            Assert.Equal(10, actual.Value);
            Assert.Equal("Substitution", actual.DisplayName);
        }

        [Fact]
        public void YellowCard_Always_CreateCorrectEventType()
        {
            // Act
            var actual = EventTypes.YellowCard;

            // Assert
            Assert.Equal(11, actual.Value);
            Assert.Equal("YellowCard", actual.DisplayName);
        }

        [Fact]
        public void YellowRedCard_Always_CreateCorrectEventType()
        {
            // Act
            var actual = EventTypes.YellowRedCard;

            // Assert
            Assert.Equal(12, actual.Value);
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