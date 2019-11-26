using LiveScore.Core.Enumerations;
using MessagePack;
using Xamarin.Forms;

namespace LiveScore.Soccer.Enumerations
{
    [MessagePackObject]
    public class TeamOutcome : Enumeration
    {
        public static readonly TeamOutcome AFCChampionsLeague = new TeamOutcome(1, "afc champions league", "AFC Champions League", Color.Green);
        public static readonly TeamOutcome AFCCup = new TeamOutcome(2, "afc cup", "AFC Cup", Color.Blue);
        public static readonly TeamOutcome CAFConfederationCup = new TeamOutcome(3, "caf confederation cup", "CAF Confederation Cup", Color.Accent);
        public static readonly TeamOutcome ChampionsLeague = new TeamOutcome(4, "champions league", "Champions League", Color.Blue);
        public static readonly TeamOutcome ChampionsLeagueQualification = new TeamOutcome(5, "champions league qualification", "Champions League Qualification", Color.BlueViolet);
        public static readonly TeamOutcome ChampionsRound = new TeamOutcome(6, "champions round", "Champions Round", Color.Yellow);
        public static readonly TeamOutcome ChampionshipRound = new TeamOutcome(7, "championship round", "Championship Round", Color.YellowGreen);
        public static readonly TeamOutcome ClubChampionship = new TeamOutcome(8, "club championship", "Club Championship", Color.LawnGreen);
        public static readonly TeamOutcome CopaLibertadores = new TeamOutcome(9, "copa libertadores", "Copa Libertadores", Color.LawnGreen);
        public static readonly TeamOutcome CopaLibertadoresQualification = new TeamOutcome(10, "copa libertadores qualification", "Copa Libertadores Qualification", Color.LawnGreen);
        public static readonly TeamOutcome CopaSudamericana = new TeamOutcome(11, "copa sudamericana", "Copa Sudamericana", Color.LawnGreen);
        public static readonly TeamOutcome CupWinners = new TeamOutcome(12, "cup winners", "Cup Winners", Color.LawnGreen);
        public static readonly TeamOutcome Eliminated = new TeamOutcome(13, "eliminated", "Eliminated", Color.LawnGreen);
        public static readonly TeamOutcome EuropaLeague = new TeamOutcome(14, "europa league", "Europa League", Color.LawnGreen);
        public static readonly TeamOutcome EuropaLeagueQualification = new TeamOutcome(15, "europa league qualification", "Europa League Qualification", Color.LawnGreen);
        public static readonly TeamOutcome EuropeanCup = new TeamOutcome(16, "european cup", "European Cup", Color.LawnGreen);
        public static readonly TeamOutcome FinalFour = new TeamOutcome(17, "final four", "Final Four", Color.LawnGreen);
        public static readonly TeamOutcome FinalRound = new TeamOutcome(18, "final round", "Final Round", Color.LawnGreen);
        public static readonly TeamOutcome Finals = new TeamOutcome(19, "finals", "Finals", Color.LawnGreen);
        public static readonly TeamOutcome GroupMatches = new TeamOutcome(20, "group matches", "Group Matches", Color.LawnGreen);
        public static readonly TeamOutcome InternationalCompetition = new TeamOutcome(21, "international competition", "International Competition", Color.LawnGreen);
        public static readonly TeamOutcome MainRound = new TeamOutcome(22, "main round", "Main Round", Color.LawnGreen);
        public static readonly TeamOutcome NextGroupPhase = new TeamOutcome(23, "next group phase", "Next Group Phase", Color.LawnGreen);
        public static readonly TeamOutcome PlacementMatches = new TeamOutcome(24, "placement matches", "Placement Matches", Color.LawnGreen);
        public static readonly TeamOutcome Playoffs = new TeamOutcome(25, "playoffs", "Playoffs", Color.LawnGreen);
        public static readonly TeamOutcome PreliminaryRound = new TeamOutcome(26, "preliminary round", "Preliminary Round", Color.LawnGreen);
        public static readonly TeamOutcome Promotion = new TeamOutcome(27, "promotion", "Promotion", Color.LawnGreen);
        public static readonly TeamOutcome PromotionPlayoff = new TeamOutcome(28, "promotion playoff", "Promotion Playoff", Color.LawnGreen);
        public static readonly TeamOutcome PromotionPlayoffs = new TeamOutcome(29, "promotion playoffs", "Promotion Playoffs", Color.LawnGreen);
        public static readonly TeamOutcome PromotionRound = new TeamOutcome(30, "promotion round", "Promotion Round", Color.LawnGreen);
        public static readonly TeamOutcome QualificationPlayoffs = new TeamOutcome(31, "qualification playoffs", "Qualification Playoffs", Color.LawnGreen);
        public static readonly TeamOutcome Qualified = new TeamOutcome(32, "qualified", "Qualified", Color.LawnGreen);
        public static readonly TeamOutcome QualifyingRound = new TeamOutcome(33, "qualifying round", "Qualifying Round", Color.LawnGreen);
        public static readonly TeamOutcome Relegation = new TeamOutcome(34, "relegation", "Relegation", Color.Red);
        public static readonly TeamOutcome RelegationPlayoff = new TeamOutcome(35, "relegation playoff", "Relegation Playoff", Color.Orange);
        public static readonly TeamOutcome RelegationPlayoffs = new TeamOutcome(36, "relegation playoffs", "Relegation Playoffs", Color.Purple);
        public static readonly TeamOutcome RelegationRound = new TeamOutcome(37, "relegation round", "Relegation Round", Color.LawnGreen);
        public static readonly TeamOutcome Semifinal = new TeamOutcome(38, "semifinal", "Semifinal", Color.LawnGreen);
        public static readonly TeamOutcome TopSix = new TeamOutcome(39, "top six", "Top Six", Color.LawnGreen);
        public static readonly TeamOutcome UEFACup = new TeamOutcome(40, "uefa cup", "UEFA Cup", Color.LawnGreen);
        public static readonly TeamOutcome UEFACupQualification = new TeamOutcome(41, "uefa cup qualification", "UEFA Cup Qualification", Color.LawnGreen);
        public static readonly TeamOutcome UEFAIntertotoCup = new TeamOutcome(42, "uefa intertoto Cup", "UEFA Intertoto Cup", Color.LawnGreen);
        public static readonly TeamOutcome Unknown = new TeamOutcome(43, "unknown", "Unknown", Color.Transparent);

        public TeamOutcome()
        {
        }

        public TeamOutcome(byte value, string displayName, string friendlyName, Color color)
            : base(value, displayName)
        {
            FriendlyName = friendlyName;
            Color = color;
        }

#pragma warning disable S109 // Magic numbers should not be used

        [Key(2)]
        public string FriendlyName { get; set; }

        [IgnoreMember]
        public Color Color { get; set; }

        public bool IsUnknown() => this == Unknown;

#pragma warning restore S109 // Magic numbers should not be used
    }
}