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
            var actual = PeriodTypes.RegularPeriodType;

            // Assert
            Assert.Equal("regular_period", actual.Value);
            Assert.Equal("RegularPeriod", actual.DisplayName);
        }

        [Fact]
        public void OvertimeType_Always_CreateCorrectType()
        {
            // Act
            var actual = PeriodTypes.OvertimeType;

            // Assert
            Assert.Equal("overtime", actual.Value);
            Assert.Equal("Overtime", actual.DisplayName);
        }

        [Fact]
        public void PenaltiesType_Always_CreateCorrectType()
        {
            // Act
            var actual = PeriodTypes.PenaltiesType;

            // Assert
            Assert.Equal("penalties", actual.Value);
            Assert.Equal("Penalties", actual.DisplayName);
        }
    }
}