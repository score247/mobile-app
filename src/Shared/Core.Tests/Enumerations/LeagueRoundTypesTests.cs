namespace LiveScore.Core.Tests.Enumerations
{
    using LiveScore.Core.Enumerations;
    using Xunit;

    public class LeagueRoundTypesTests
    {
        [Fact]
        public void CupRound_Always_CreateCorrectType()
        {
            // Act
            var actual = LeagueRoundTypes.CupRound;

            // Assert
            Assert.Equal("cup", actual.Value);
            Assert.Equal("Cup", actual.DisplayName);
        }

        [Fact]
        public void GroupRound_Always_CreateCorrectType()
        {
            // Act
            var actual = LeagueRoundTypes.GroupRound;

            // Assert
            Assert.Equal("group", actual.Value);
            Assert.Equal("Group", actual.DisplayName);
        }

        [Fact]
        public void PlayOffRound_Always_CreateCorrectType()
        {
            // Act
            var actual = LeagueRoundTypes.PlayOffRound;

            // Assert
            Assert.Equal("playoff", actual.Value);
            Assert.Equal("PlayOff", actual.DisplayName);
        }

        [Fact]
        public void QualifierRound_Always_CreateCorrectType()
        {
            // Act
            var actual = LeagueRoundTypes.QualifierRound;

            // Assert
            Assert.Equal("qualification", actual.Value);
            Assert.Equal("Qualifier", actual.DisplayName);
        }

        [Fact]
        public void VariableRound_Always_CreateCorrectType()
        {
            // Act
            var actual = LeagueRoundTypes.VariableRound;

            // Assert
            Assert.Equal("variable", actual.Value);
            Assert.Equal("Variable", actual.DisplayName);
        }

        [Fact]
        public void Constructor_DoNothing()
        {
            // Act
            new LeagueRoundTypes();

            // Assert
            Assert.True(true);
        }
    }
}