using AutoFixture;
using LiveScore.Soccer.ViewModels.Matches.MatchDetails.HeadToHead;
using Xunit;

namespace Soccer.Tests.ViewModels.DetailH2H
{
    public class H2HStatisticViewModelTests
    {
        private readonly Fixture fixture;

        public H2HStatisticViewModelTests()
        {
            fixture = new Fixture();
        }

        [Fact]
        public void DisplayHomeWin_CorrectAssginedValueAndFormat()
        {
            var h2hStats = fixture.Create<H2HStatisticViewModel>();

            Assert.Equal($"{h2hStats.HomeWin}/{h2hStats.Total}", h2hStats.DisplayHomeWin);
        }

        [Fact]
        public void DisplayAwayWin_CorrectAssginedValueAndFormat()
        {
            var h2hStats = fixture.Create<H2HStatisticViewModel>();

            Assert.Equal($"{h2hStats.AwayWin}/{h2hStats.Total}", h2hStats.DisplayAwayWin);
        }

        [Fact]
        public void DisplayDraw_CorrectAssginedValueAndFormat()
        {
            var h2hStats = fixture.Create<H2HStatisticViewModel>();

            Assert.Equal($"{h2hStats.Total - h2hStats.HomeWin - h2hStats.AwayWin}/{h2hStats.Total}", h2hStats.DisplayDraw);
        }
    }
}