namespace LiveScore.Core.Enumerations
{
    public class TabDetailImages : Images
    {
        public static readonly TabDetailImages Odds = new TabDetailImages("images/tabstrip/inactive/tab_odds.png", nameof(Odds));
        public static readonly TabDetailImages Info = new TabDetailImages("images/tabstrip/inactive/tab_info.png", nameof(Info));
        public static readonly TabDetailImages H2H = new TabDetailImages("images/tabstrip/inactive/tab_h2h.png", nameof(H2H));
        public static readonly TabDetailImages Lineups = new TabDetailImages("images/tabstrip/inactive/tab_lineup.png", nameof(Lineups));
        public static readonly TabDetailImages Social = new TabDetailImages("images/tabstrip/inactive/tab_social.png", nameof(Social));
        public static readonly TabDetailImages Stats = new TabDetailImages("images/tabstrip/inactive/tab_stats.png", nameof(Stats));
        public static readonly TabDetailImages Tracker = new TabDetailImages("images/tabstrip/inactive/tab_tracker.png", nameof(Tracker));
        public static readonly TabDetailImages Table = new TabDetailImages("images/tabstrip/inactive/tab_table.png", nameof(Table));
        public static readonly TabDetailImages TV = new TabDetailImages("images/tabstrip/inactive/tab_tv.png", nameof(TV));

        public static readonly TabDetailImages OddsActive = new TabDetailImages("images/tabstrip/active/tab_odds_active.png", nameof(OddsActive));
        public static readonly TabDetailImages InfoActive = new TabDetailImages("images/tabstrip/active/tab_info_active.png", nameof(InfoActive));
        public static readonly TabDetailImages H2HActive = new TabDetailImages("images/tabstrip/active/tab_h2h_active.png", nameof(H2HActive));
        public static readonly TabDetailImages LineupsActive = new TabDetailImages("images/tabstrip/active/tab_lineup_active.png", nameof(LineupsActive));
        public static readonly TabDetailImages SocialActive = new TabDetailImages("images/tabstrip/active/tab_social_active.png", nameof(SocialActive));
        public static readonly TabDetailImages StatsActive = new TabDetailImages("images/tabstrip/active/tab_stats_active.png", nameof(StatsActive));
        public static readonly TabDetailImages TrackerActive = new TabDetailImages("images/tabstrip/active/tab_tracker_active.png", nameof(TrackerActive));
        public static readonly TabDetailImages TableActive = new TabDetailImages("images/tabstrip/active/tab_table_active.png", nameof(TableActive));
        public static readonly TabDetailImages TVActive = new TabDetailImages("images/tabstrip/active/tab_tv_active.png", nameof(TVActive));

        public TabDetailImages()
        {
        }

        protected TabDetailImages(string value, string name) : base(value, name)
        {
        }
    }
}