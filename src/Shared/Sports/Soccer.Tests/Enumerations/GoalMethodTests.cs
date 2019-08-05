namespace Soccer.Tests.Enumerations
{
    using LiveScore.Soccer.Enumerations;
    using Xunit;

    public class GoalMethodTests
    {
        [Fact]
        public void OwnGoal_Always_GetExpectedValue()
        {
            // Act
            var actual = GoalMethod.OwnGoalType;

            // Assert
            Assert.Equal(1, actual.Value);
            Assert.Equal("own_goal", actual.DisplayName);
        }

        [Fact]
        public void Penalty_Always_GetExpectedValue()
        {
            // Act
            var actual = GoalMethod.PenaltyType;

            // Assert
            Assert.Equal(2, actual.Value);
            Assert.Equal("penalty", actual.DisplayName);
        }
    }
}