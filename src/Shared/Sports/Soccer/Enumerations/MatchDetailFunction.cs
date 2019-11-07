using LiveScore.Core.Enumerations;

namespace LiveScore.Soccer.Enumerations
{
    public class MatchDetailFunction : Enumeration
    {
        //public static readonly MatchDetailFunction Odds = new MatchDetailFunction(0, "Odds");
        public static readonly MatchDetailFunction Info = new MatchDetailFunction(0, "SoccerMatch Info");
        public static readonly MatchDetailFunction Tracker = new MatchDetailFunction(1, "Tracker");
        public static readonly MatchDetailFunction Stats = new MatchDetailFunction(2, "Statistics");
        public static readonly MatchDetailFunction Lineups = new MatchDetailFunction(3, "Line - ups");
        public static readonly MatchDetailFunction H2H = new MatchDetailFunction(4, "Head to Head");
        public static readonly MatchDetailFunction Table = new MatchDetailFunction(5, "Table");
        public static readonly MatchDetailFunction Social = new MatchDetailFunction(6, "Social");
        public static readonly MatchDetailFunction TV = new MatchDetailFunction(7, "TV Schedule");

        public MatchDetailFunction()
        {
        }

        private MatchDetailFunction(byte value, string displayName) : base(value, displayName)
        {
        }
    }
}