namespace LiveScore.Common.Tests.Extensions
{
    using System;
    using LiveScore.Common.Extensions;
    using Xunit;

    public class DateRangeTests
    {
        [Fact]
        public void FromYesterdayUntilNow_ShouldReturnCorrectDate()
        {
            // Arrange
            var yesterday = DateTime.Today.AddDays(-1);
            var today = DateTime.Today.AddDays(1).AddMilliseconds(-1);

            var expected = $"{yesterday.ToString()} - {today.ToString()}";

            // Act
            var actual = DateRange.FromYesterdayUntilNow();

            // Assert
            Assert.Equal(expected, $"{actual.FromDate.ToString()} - {actual.ToDate.ToString()}");
        }
    }
}
