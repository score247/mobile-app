namespace LiveScore.Core.Enumerations
{
    public class LeagueRoundTypes : Enumeration
    {
        public const string Cup = "cup";
        public const string Group = "group";
        public const string PlayOff = "playoff";
        public const string Qualifier = "qualification";
        public const string Variable = "variable";

        public static readonly LeagueRoundTypes CupRound = new LeagueRoundTypes(1, Cup);
        public static readonly LeagueRoundTypes GroupRound = new LeagueRoundTypes(2, Group);
        public static readonly LeagueRoundTypes PlayOffRound = new LeagueRoundTypes(3, PlayOff);
        public static readonly LeagueRoundTypes QualifierRound = new LeagueRoundTypes(4, Qualifier);
        public static readonly LeagueRoundTypes VariableRound = new LeagueRoundTypes(5, Variable);

        public LeagueRoundTypes()
        {
        }

        private LeagueRoundTypes(byte value, string displayName)
            : base(value, displayName)
        {
        }
    }
}