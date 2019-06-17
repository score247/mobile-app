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
            Assert.Equal("own_goal", actual.Value);
            Assert.Equal("OwnGoal", actual.DisplayName);
        }

        [Fact]
        public void Penalty_Always_GetExpectedValue()
        {
            // Act
            var actual = GoalMethod.PenaltyType;

            // Assert
            Assert.Equal("penalty", actual.Value);
            Assert.Equal("Penalty", actual.DisplayName);
        }
    }
}