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
            Assert.Equal(expected, $"{actual.From.ToString()} - {actual.To.ToString()}");
        }

        [Fact]
        public void FromDate_Aways_Return_BeginingOfDay()
        {
            // Arrange
            var dateTime = DateTime.Now;

            // Act
            var dateRange = new DateRange(dateTime);

            // Assset
            Assert.Equal(dateRange.From, dateTime.BeginningOfDay());
        }

        [Fact]
        public void ToDate_Always_Return_EndOfDay()
        {
            // Arrange
            var dateTime = DateTime.Now;

            // Act
            var dateRange = new DateRange(dateTime);

            // Assset
            Assert.Equal(dateRange.To, dateTime.EndOfDay());
        }

    }
}
