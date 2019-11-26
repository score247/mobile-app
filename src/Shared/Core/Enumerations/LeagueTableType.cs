using MessagePack;

namespace LiveScore.Core.Enumerations
{
    [MessagePackObject]
    public class LeagueTableType : Enumeration
    {
        public const string Total = "total";
        public const string Home = "home";
        public const string Away = "away";

        public static readonly LeagueTableType CupRound = new LeagueTableType(1, Total);
        public static readonly LeagueTableType GroupRound = new LeagueTableType(2, Home);
        public static readonly LeagueTableType PlayOffRound = new LeagueTableType(3, Away);

        public LeagueTableType()
        {
        }

        public LeagueTableType(byte value, string displayName)
            : base(value, displayName)
        {
        }
    }
}