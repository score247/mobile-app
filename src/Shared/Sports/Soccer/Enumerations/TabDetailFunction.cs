namespace LiveScore.Core.Enumerations
{
    public class MatchDetailFunction : TextEnumeration
    {
        public static readonly MatchDetailFunction Odds = new MatchDetailFunction("Odds", "Odds");
        public static readonly MatchDetailFunction Info = new MatchDetailFunction("Info", "Match Info");
        public static readonly MatchDetailFunction Tracker = new MatchDetailFunction("Tracker", "Tracker");
        public static readonly MatchDetailFunction Stats = new MatchDetailFunction("Stats", "Statistics");
        public static readonly MatchDetailFunction Lineups = new MatchDetailFunction("Line-ups", "Line -ups");
        public static readonly MatchDetailFunction H2H = new MatchDetailFunction("H2H", "Head to Head");
        public static readonly MatchDetailFunction Table = new MatchDetailFunction("Table", "Table");
        public static readonly MatchDetailFunction Social = new MatchDetailFunction("Social", "Social");
        public static readonly MatchDetailFunction TV = new MatchDetailFunction("TV", "TV Schedule");

        public MatchDetailFunction()
        {
        }

        private MatchDetailFunction(string value, string displayName)
            : base(value, displayName)
        {
        }
    }
}