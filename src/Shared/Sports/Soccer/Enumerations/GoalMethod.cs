namespace LiveScore.Soccer.Enumerations
{
    using LiveScore.Core.Enumerations;

    public class GoalMethod : Enumeration
    {
        public const string OwnGoal = "own_goal";

        public static readonly GoalMethod OwnGoalType = new GoalMethod(OwnGoal, nameof(OwnGoal));

        public const string Penalty = "penalty";

        public static readonly GoalMethod PenaltyType = new GoalMethod(Penalty, nameof(Penalty));

        private GoalMethod(string value, string displayName)
           : base(value, displayName)
        {
        }
    }
}

