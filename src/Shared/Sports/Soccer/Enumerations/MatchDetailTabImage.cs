namespace LiveScore.Core.Enumerations
{
    public class MatchDetailTabImage : Images
    {
        public static readonly MatchDetailTabImage Odds = new MatchDetailTabImage("images/tabstrip/inactive/tab_odds.png", nameof(Odds));
        public static readonly MatchDetailTabImage Info = new MatchDetailTabImage("images/tabstrip/inactive/tab_info.png", nameof(Info));
        public static readonly MatchDetailTabImage H2H = new MatchDetailTabImage("images/tabstrip/inactive/tab_h2h.png", nameof(H2H));
        public static readonly MatchDetailTabImage Lineups = new MatchDetailTabImage("images/tabstrip/inactive/tab_lineup.png", nameof(Lineups));
        public static readonly MatchDetailTabImage Social = new MatchDetailTabImage("images/tabstrip/inactive/tab_social.png", nameof(Social));
        public static readonly MatchDetailTabImage Stats = new MatchDetailTabImage("images/tabstrip/inactive/tab_stats.png", nameof(Stats));
        public static readonly MatchDetailTabImage Tracker = new MatchDetailTabImage("images/tabstrip/inactive/tab_tracker.png", nameof(Tracker));
        public static readonly MatchDetailTabImage Table = new MatchDetailTabImage("images/tabstrip/inactive/tab_table.png", nameof(Table));
        public static readonly MatchDetailTabImage TV = new MatchDetailTabImage("images/tabstrip/inactive/tab_tv.png", nameof(TV));

        public static readonly MatchDetailTabImage OddsActive = new MatchDetailTabImage("images/tabstrip/active/tab_odds_active.png", nameof(OddsActive));
        public static readonly MatchDetailTabImage InfoActive = new MatchDetailTabImage("images/tabstrip/active/tab_info_active.png", nameof(InfoActive));
        public static readonly MatchDetailTabImage H2HActive = new MatchDetailTabImage("images/tabstrip/active/tab_h2h_active.png", nameof(H2HActive));
        public static readonly MatchDetailTabImage LineupsActive = new MatchDetailTabImage("images/tabstrip/active/tab_lineup_active.png", nameof(LineupsActive));
        public static readonly MatchDetailTabImage SocialActive = new MatchDetailTabImage("images/tabstrip/active/tab_social_active.png", nameof(SocialActive));
        public static readonly MatchDetailTabImage StatsActive = new MatchDetailTabImage("images/tabstrip/active/tab_stats_active.png", nameof(StatsActive));
        public static readonly MatchDetailTabImage TrackerActive = new MatchDetailTabImage("images/tabstrip/active/tab_tracker_active.png", nameof(TrackerActive));
        public static readonly MatchDetailTabImage TableActive = new MatchDetailTabImage("images/tabstrip/active/tab_table_active.png", nameof(TableActive));
        public static readonly MatchDetailTabImage TVActive = new MatchDetailTabImage("images/tabstrip/active/tab_tv_active.png", nameof(TVActive));

        public MatchDetailTabImage()
        {
        }

        protected MatchDetailTabImage(string value, string name) : base(value, name)
        {
        }
    }
}