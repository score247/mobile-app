using LiveScore.Core.Enumerations;
using MessagePack;

namespace LiveScore.Soccer.Enumerations
{
    [MessagePackObject(keyAsPropertyName: true)]
    public class TeamOutcome : Enumeration
    {
        private const string FirstPositiveOutcomeColor = "FirstPositiveOutcomeColor";
        private const string SecondPositiveOutcomeColor = "SecondPositiveOutcomeColor";
        private const string ThirdPositiveOutcomeColor = "ThirdPositiveOutcomeColor";
        private const string FourthPositiveOutcomeColor = "FourthPositiveOutcomeColor";
        private const string FifthPositiveOutcomeColor = "FifthPositiveOutcomeColor";
        private const string FirstNegativeOutcomeColor = "FirstNegativeOutcomeColor";
        private const string SecondNegativeOutcomeColor = "SecondNegativeOutcomeColor";

        public static readonly TeamOutcome AFCChampionsLeague = new TeamOutcome(1, "afc champions league", "AFC Champions League", FirstPositiveOutcomeColor);
        public static readonly TeamOutcome AFCCup = new TeamOutcome(2, "afc cup", "AFC Cup", FirstPositiveOutcomeColor);
        public static readonly TeamOutcome CAFConfederationCup = new TeamOutcome(3, "caf confederation cup", "CAF Confederation Cup", FirstPositiveOutcomeColor);
        public static readonly TeamOutcome ChampionsLeague = new TeamOutcome(4, "champions league", "Champions League", FirstPositiveOutcomeColor);
        public static readonly TeamOutcome ChampionsLeagueQualification = new TeamOutcome(5, "champions league qualification", "Champions League Qualification", SecondPositiveOutcomeColor);
        public static readonly TeamOutcome ChampionsRound = new TeamOutcome(6, "champions round", "Champions Round", FourthPositiveOutcomeColor);
        public static readonly TeamOutcome ChampionshipRound = new TeamOutcome(7, "championship round", "Championship Round", FourthPositiveOutcomeColor);
        public static readonly TeamOutcome ClubChampionship = new TeamOutcome(8, "club championship", "Club Championship", SecondPositiveOutcomeColor);
        public static readonly TeamOutcome CopaLibertadores = new TeamOutcome(9, "copa libertadores", "Copa Libertadores", FirstPositiveOutcomeColor);
        public static readonly TeamOutcome CopaLibertadoresQualification = new TeamOutcome(10, "copa libertadores qualification", "Copa Libertadores Qualification", SecondPositiveOutcomeColor);
        public static readonly TeamOutcome CopaSudamericana = new TeamOutcome(11, "copa sudamericana", "Copa Sudamericana", ThirdPositiveOutcomeColor);
        public static readonly TeamOutcome CupWinners = new TeamOutcome(12, "cup winners", "Cup Winners", FirstPositiveOutcomeColor);
        public static readonly TeamOutcome Eliminated = new TeamOutcome(13, "eliminated", "Eliminated", FirstNegativeOutcomeColor);
        public static readonly TeamOutcome EuropaLeague = new TeamOutcome(14, "europa league", "Europa League", ThirdPositiveOutcomeColor);
        public static readonly TeamOutcome EuropaLeagueQualification = new TeamOutcome(15, "europa league qualification", "Europa League Qualification", FourthPositiveOutcomeColor);
        public static readonly TeamOutcome EuropeanCup = new TeamOutcome(16, "european cup", "European Cup", FirstPositiveOutcomeColor);
        public static readonly TeamOutcome FinalFour = new TeamOutcome(17, "final four", "Final Four", FirstPositiveOutcomeColor);
        public static readonly TeamOutcome FinalRound = new TeamOutcome(18, "final round", "Final Round", FirstPositiveOutcomeColor);
        public static readonly TeamOutcome Finals = new TeamOutcome(19, "finals", "Finals", FirstPositiveOutcomeColor);
        public static readonly TeamOutcome GroupMatches = new TeamOutcome(20, "group matches", "Group Matches", FirstPositiveOutcomeColor);
        public static readonly TeamOutcome InternationalCompetition = new TeamOutcome(21, "international competition", "International Competition", FourthPositiveOutcomeColor);
        public static readonly TeamOutcome MainRound = new TeamOutcome(22, "main round", "Main Round", FourthPositiveOutcomeColor);
        public static readonly TeamOutcome NextGroupPhase = new TeamOutcome(23, "next group phase", "Next Group Phase", FifthPositiveOutcomeColor);
        public static readonly TeamOutcome PlacementMatches = new TeamOutcome(24, "placement matches", "Placement Matches", FourthPositiveOutcomeColor);
        public static readonly TeamOutcome Playoffs = new TeamOutcome(25, "playoffs", "Playoffs", SecondPositiveOutcomeColor);
        public static readonly TeamOutcome PreliminaryRound = new TeamOutcome(26, "preliminary round", "Preliminary Round", ThirdPositiveOutcomeColor);
        public static readonly TeamOutcome Promotion = new TeamOutcome(27, "promotion", "Promotion", FirstPositiveOutcomeColor);
        public static readonly TeamOutcome PromotionPlayoff = new TeamOutcome(28, "promotion playoff", "Promotion Playoff", SecondPositiveOutcomeColor);
        public static readonly TeamOutcome PromotionPlayoffs = new TeamOutcome(29, "promotion playoffs", "Promotion Playoffs", SecondPositiveOutcomeColor);
        public static readonly TeamOutcome PromotionRound = new TeamOutcome(30, "promotion round", "Promotion Round", SecondPositiveOutcomeColor);
        public static readonly TeamOutcome QualificationPlayoffs = new TeamOutcome(31, "qualification playoffs", "Qualification Playoffs", ThirdPositiveOutcomeColor);
        public static readonly TeamOutcome Qualified = new TeamOutcome(32, "qualified", "Qualified", FirstPositiveOutcomeColor);
        public static readonly TeamOutcome QualifyingRound = new TeamOutcome(33, "qualifying round", "Qualifying Round", SecondPositiveOutcomeColor);
        public static readonly TeamOutcome Relegation = new TeamOutcome(34, "relegation", "Relegation", FirstNegativeOutcomeColor);
        public static readonly TeamOutcome RelegationPlayoff = new TeamOutcome(35, "relegation playoff", "Relegation Playoff", SecondNegativeOutcomeColor);
        public static readonly TeamOutcome RelegationPlayoffs = new TeamOutcome(36, "relegation playoffs", "Relegation Playoffs", SecondNegativeOutcomeColor);
        public static readonly TeamOutcome RelegationRound = new TeamOutcome(37, "relegation round", "Relegation Round", SecondNegativeOutcomeColor);
        public static readonly TeamOutcome Semifinal = new TeamOutcome(38, "semifinal", "Semifinal", FirstPositiveOutcomeColor);
        public static readonly TeamOutcome TopSix = new TeamOutcome(39, "top six", "Top Six", FirstPositiveOutcomeColor);
        public static readonly TeamOutcome UEFACup = new TeamOutcome(40, "uefa cup", "UEFA Cup", FirstPositiveOutcomeColor);
        public static readonly TeamOutcome UEFACupQualification = new TeamOutcome(41, "uefa cup qualification", "UEFA Cup Qualification", SecondPositiveOutcomeColor);
        public static readonly TeamOutcome UEFAIntertotoCup = new TeamOutcome(42, "uefa intertoto Cup", "UEFA Intertoto Cup", FirstPositiveOutcomeColor);
        public static readonly TeamOutcome Unknown = new TeamOutcome(43, "unknown", "Unknown", "ListViewBgColor");

        public TeamOutcome()
        {
        }

        public TeamOutcome(byte value, string displayName, string friendlyName, string colorResourceKey)
            : base(value, displayName)
        {
            FriendlyName = friendlyName;
            ColorResourceKey = colorResourceKey;
        }

#pragma warning disable S109 // Magic numbers should not be used

        public string FriendlyName { get; set; }

        [IgnoreMember]
        public string ColorResourceKey { get; set; }

        public bool IsUnknown() => this == Unknown;

#pragma warning restore S109 // Magic numbers should not be used
    }
}