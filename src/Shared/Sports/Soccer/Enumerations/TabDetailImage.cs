namespace LiveScore.Core.Enumerations
{
    public class TabDetailImage : Images
    {
        public static readonly TabDetailImage Odds = new TabDetailImage("images/tabstrip/inactive/tab_odds.png", nameof(Odds));
        public static readonly TabDetailImage Info = new TabDetailImage("images/tabstrip/inactive/tab_info.png", nameof(Info));
        public static readonly TabDetailImage H2H = new TabDetailImage("images/tabstrip/inactive/tab_h2h.png", nameof(H2H));
        public static readonly TabDetailImage Lineups = new TabDetailImage("images/tabstrip/inactive/tab_lineup.png", nameof(Lineups));
        public static readonly TabDetailImage Social = new TabDetailImage("images/tabstrip/inactive/tab_social.png", nameof(Social));
        public static readonly TabDetailImage Stats = new TabDetailImage("images/tabstrip/inactive/tab_stats.png", nameof(Stats));
        public static readonly TabDetailImage Tracker = new TabDetailImage("images/tabstrip/inactive/tab_tracker.png", nameof(Tracker));
        public static readonly TabDetailImage Table = new TabDetailImage("images/tabstrip/inactive/tab_table.png", nameof(Table));
        public static readonly TabDetailImage TV = new TabDetailImage("images/tabstrip/inactive/tab_tv.png", nameof(TV));

        public static readonly TabDetailImage OddsActive = new TabDetailImage("images/tabstrip/active/tab_odds_active.png", nameof(OddsActive));
        public static readonly TabDetailImage InfoActive = new TabDetailImage("images/tabstrip/active/tab_info_active.png", nameof(InfoActive));
        public static readonly TabDetailImage H2HActive = new TabDetailImage("images/tabstrip/active/tab_h2h_active.png", nameof(H2HActive));
        public static readonly TabDetailImage LineupsActive = new TabDetailImage("images/tabstrip/active/tab_lineup_active.png", nameof(LineupsActive));
        public static readonly TabDetailImage SocialActive = new TabDetailImage("images/tabstrip/active/tab_social_active.png", nameof(SocialActive));
        public static readonly TabDetailImage StatsActive = new TabDetailImage("images/tabstrip/active/tab_stats_active.png", nameof(StatsActive));
        public static readonly TabDetailImage TrackerActive = new TabDetailImage("images/tabstrip/active/tab_tracker_active.png", nameof(TrackerActive));
        public static readonly TabDetailImage TableActive = new TabDetailImage("images/tabstrip/active/tab_table_active.png", nameof(TableActive));
        public static readonly TabDetailImage TVActive = new TabDetailImage("images/tabstrip/active/tab_tv_active.png", nameof(TVActive));

        public TabDetailImage()
        {
        }

        protected TabDetailImage(string value, string name) : base(value, name)
        {
        }
    }
}