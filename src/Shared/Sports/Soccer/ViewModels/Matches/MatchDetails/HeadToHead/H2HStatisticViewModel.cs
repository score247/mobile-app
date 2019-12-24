namespace LiveScore.Soccer.ViewModels.Matches.MatchDetails.HeadToHead
{
    public class H2HStatisticViewModel
    {
        public H2HStatisticViewModel()
            : this(0, 0, 0)
        {
        }

        public H2HStatisticViewModel(int homeWin, int awayWin, int total)
        {
            HomeWin = homeWin;
            AwayWin = awayWin;
            Draw = total - homeWin - awayWin;
            Total = total;
        }

        public int HomeWin { get; }

        public int AwayWin { get; }

        public int Draw { get; }

        public int Total { get; }

        public string DisplayHomeWin => $"{HomeWin}/{Total}";

        public string DisplayDraw => $"{Draw}/{Total}";

        public string DisplayAwayWin => $"{AwayWin}/{Total}";
    }
}