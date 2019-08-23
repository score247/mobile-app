namespace LiveScore.Core.Enumerations
{
    public class TabDetailFunction : Enumeration
    {
        public static readonly TabDetailFunction Odds = new TabDetailFunction(1, "Odds");
        public static readonly TabDetailFunction Info = new TabDetailFunction(2, "Info");
        public static readonly TabDetailFunction Tracker = new TabDetailFunction(3, "Tracker");
        public static readonly TabDetailFunction Stats = new TabDetailFunction(4, "Stats");
        public static readonly TabDetailFunction Lineups = new TabDetailFunction(5, "Line-ups");
        public static readonly TabDetailFunction H2H = new TabDetailFunction(6, "H2H");
        public static readonly TabDetailFunction Table = new TabDetailFunction(7, "Table");
        public static readonly TabDetailFunction Social = new TabDetailFunction(8, "Social");
        public static readonly TabDetailFunction TV = new TabDetailFunction(9, "TV");

        public TabDetailFunction()
        {
        }

        private TabDetailFunction(byte value, string displayName)
            : base(value, displayName)
        {
        }
    }
}