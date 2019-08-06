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
            Assert.Equal(1, actual.Value);
            Assert.Equal("cup", actual.DisplayName);
        }

        [Fact]
        public void GroupRound_Always_CreateCorrectType()
        {
            // Act
            var actual = LeagueRoundTypes.GroupRound;

            // Assert
            Assert.Equal(2, actual.Value);
            Assert.Equal("group", actual.DisplayName);
        }

        [Fact]
        public void PlayOffRound_Always_CreateCorrectType()
        {
            // Act
            var actual = LeagueRoundTypes.PlayOffRound;

            // Assert
            Assert.Equal(3, actual.Value);
            Assert.Equal("playoff", actual.DisplayName);
        }

        [Fact]
        public void QualifierRound_Always_CreateCorrectType()
        {
            // Act
            var actual = LeagueRoundTypes.QualifierRound;

            // Assert
            Assert.Equal(4, actual.Value);
            Assert.Equal("qualification", actual.DisplayName);
        }

        [Fact]
        public void VariableRound_Always_CreateCorrectType()
        {
            // Act
            var actual = LeagueRoundTypes.VariableRound;

            // Assert
            Assert.Equal(5, actual.Value);
            Assert.Equal("variable", actual.DisplayName);
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