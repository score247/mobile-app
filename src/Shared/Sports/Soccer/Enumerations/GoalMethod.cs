using LiveScore.Core.Enumerations;

namespace LiveScore.Soccer.Enumerations
{
    public sealed class GoalMethod : Enumeration
    {
        public const string OwnGoal = "own_goal";

        public static readonly GoalMethod OwnGoalType = new GoalMethod(1, OwnGoal);

        public const string Penalty = "penalty";

        public static readonly GoalMethod PenaltyType = new GoalMethod(2, Penalty);

        private GoalMethod(byte value, string displayName)
           : base(value, displayName)
        {
        }
    }
}