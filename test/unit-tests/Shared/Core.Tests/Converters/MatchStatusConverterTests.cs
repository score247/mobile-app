namespace LiveScore.Core.Tests.Converters
{
    using LiveScore.Core.Tests.Fixtures;
    using Xunit;

    public class MatchStatusConverterTests : IClassFixture<CommonFixture>
    {
        //private readonly MatchStatusConverter converter;
        //private readonly Match match;

        //public MatchStatusConverterTests(CommonFixture commonFixture)
        //{
        //    converter = new MatchStatusConverter();
        //    match = commonFixture.Fixture.Create<Match>();
        //}

        //[Fact]
        //public void Convert_ValueIsNull_ReturnFullTime()
        //{
        //    // Act
        //    var actual = converter.Convert(null, null, null, null);

        //    // Assert
        //    Assert.Equal("FT", actual);
        //}

        //[Fact]
        //public void Convert_StatusIsNotStarted_ReturnCorrectFormat()
        //{
        //    // Arrange
        //    match.EventDate = new DateTime(2019, 01, 01, 18, 30, 00);
        //    match.MatchResult.EventStatus.Value = MatchStatus.NotStarted;
        //    const string expected = "18:30";

        //    // Act
        //    var actual = converter.Convert(match, null, null, null);

        //    // Assert
        //    Assert.Equal(expected, actual);
        //}

        //[Theory]
        //[InlineData(MatchStatus.Ended, "FT")]
        //[InlineData(MatchStatus.FullTime, "FT")]
        //[InlineData(MatchStatus.Closed, "FT")]
        //[InlineData(MatchStatus.Abandoned, "AB")]
        //[InlineData(MatchStatus.EndedExtraTime, "A.E.T")]
        //[InlineData(MatchStatus.EndedAfterPenalties, "A.P")]
        //[InlineData("", "FT")]
        //[InlineData(null, "FT")]
        //public void Convert_StatusIsClosed_ReturnCorrectFormat(string matchStatus, string expectedStatus)
        //{
        //    // Arrange
        //    match.MatchResult.EventStatus.Value = MatchStatus.Closed;
        //    match.MatchResult.MatchStatus.Value = matchStatus;

        //    // Act
        //    var actual = converter.Convert(match, null, null, null);

        //    // Assert
        //    Assert.Equal(expectedStatus, actual);
        //}

        //[Theory]
        //[InlineData(MatchStatus.Postponed, "Postp.")]
        //[InlineData(MatchStatus.StartDelayed, "Start Delay")]
        //[InlineData(MatchStatus.Cancelled, "Canc.")]
        //[InlineData(MatchStatus.Abandoned, "AB")]
        //[InlineData(MatchStatus.Delayed, "Delayed")]
        //[InlineData(MatchStatus.Ended, "FT")]
        //[InlineData("", "")]
        //public void Convert_StatusIsOtherSituations_ReturnCorrectFormat(string eventStatus, string expectedStatus)
        //{
        //    // Arrange
        //    match.MatchResult.EventStatus.Value = eventStatus;

        //    // Act
        //    var actual = converter.Convert(match, null, null, null);

        //    // Assert
        //    Assert.Equal(expectedStatus, actual.ToString());
        //}

        //[Fact]
        //public void ConvertBack_ReturnMatchStatus()
        //{
        //    // Act
        //    var actual = converter.ConvertBack(MatchStatus.NotStartedStatus, null, null, null);

        //    // Assert
        //    Assert.IsType<MatchStatus>(actual);
        //}
    }
}