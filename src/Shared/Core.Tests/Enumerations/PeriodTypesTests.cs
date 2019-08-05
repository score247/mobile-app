namespace LiveScore.Core.Tests.Enumerations
{
    using LiveScore.Core.Enumerations;
    using Xunit;

    public class PeriodTypesTests
    {
        [Fact]
        public void RegularPeriodType_Always_CreateCorrectType()
        {
            // Act
            var actual = PeriodTypes.RegularPeriod;

            // Assert
            Assert.Equal(1, actual.Value);
            Assert.Equal("RegularPeriod", actual.DisplayName);
        }

        [Fact]
        public void OvertimeType_Always_CreateCorrectType()
        {
            // Act
            var actual = PeriodTypes.Overtime;

            // Assert
            Assert.Equal(2, actual.Value);
            Assert.Equal("Overtime", actual.DisplayName);
        }

        [Fact]
        public void PenaltiesType_Always_CreateCorrectType()
        {
            // Act
            var actual = PeriodTypes.Penalties;

            // Assert
            Assert.Equal(3, actual.Value);
            Assert.Equal("Penalties", actual.DisplayName);
        }
    }
}