namespace LiveScore.Core.Enumerations
{
    public class TeamResult : Enumeration
    {
        public static readonly TeamResult Win = new TeamResult(1, "W");
        public static readonly TeamResult Draw = new TeamResult(2, "D");
        public static readonly TeamResult Lose = new TeamResult(3, "L");

        public TeamResult()
        {
        }

        public TeamResult(byte value, string displayName)
            : base(value, displayName)
        {
        }
    }
}
