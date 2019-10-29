using LiveScore.Soccer.ViewModels.MatchDetails.DetailH2H;
using Xunit;

namespace Soccer.Tests.ViewModels.DetailH2H
{
    public class H2HStatisticViewModelTests
    {
        private readonly H2HStatisticViewModel h2hStats;
        public H2HStatisticViewModelTests() 
        {
            h2hStats = new H2HStatisticViewModel(3, 5, 10);
        }

        [Fact]
        public void DisplayHomeWin_CorrectAssginedValueAndFormat() 
        {
            Assert.Equal("3/10", h2hStats.DisplayHomeWin);
        }

        [Fact]
        public void DisplayAwayWin_CorrectAssginedValueAndFormat()
        {
            Assert.Equal("5/10", h2hStats.DisplayAwayWin);
        }

        [Fact]
        public void DisplayDraw_CorrectAssginedValueAndFormat()
        {
            Assert.Equal("2/10", h2hStats.DisplayDraw);
        }
    }
}
