namespace LiveScore.Core.Enumerations
{
    public class TabFunction : Enumeration
    {        
        public static readonly TabFunction Odds = new TabFunction(1, "Odds");
        public static readonly TabFunction Info = new TabFunction(2, "Info");
        public static readonly TabFunction Tracker = new TabFunction(3, "Tracker");
        public static readonly TabFunction Stats = new TabFunction(4, "Stats");
        public static readonly TabFunction Lineups = new TabFunction(5, "Line-ups");
        public static readonly TabFunction H2H = new TabFunction(6, "H2H");
        public static readonly TabFunction Table = new TabFunction(7, "Table");
        public static readonly TabFunction Social = new TabFunction(8, "Social");
        public static readonly TabFunction TV = new TabFunction(9, "TV");


        public TabFunction()
        {
        }
        
        private TabFunction(byte value, string displayName)
            : base(value, displayName)
        {
        }

    }
}
