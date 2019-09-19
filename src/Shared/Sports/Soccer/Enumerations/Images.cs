namespace LiveScore.Soccer.Enumerations
{
    using LiveScore.Core.Enumerations;

    /// <summary>
    /// Keep using image for dynamic loading
    /// </summary>
    public class Images : TextEnumeration
    {
        public static readonly Images YellowCard = new Images("images/common/yellow_card.png", nameof(YellowCard));
        public static readonly Images RedYellowCard = new Images("images/common/red_yellow_card.png", nameof(RedYellowCard));
        public static readonly Images RedCard = new Images("images/common/red_card.png", nameof(RedCard));
        public static readonly Images OwnGoal = new Images("images/common/own_goal.png", nameof(OwnGoal));
        public static readonly Images Goal = new Images("images/common/ball.png", nameof(Goal));
        public static readonly Images PenaltyShootoutGoal = new Images("images/common/ball.png", nameof(PenaltyShootoutGoal));
        public static readonly Images MissedPenaltyShootoutGoal = new Images("images/common/missed_goal.png", nameof(MissedPenaltyShootoutGoal));
        public static readonly Images SecondLeg = new Images("images/common/second_leg_winner.png", nameof(SecondLeg));
        public static readonly Images PenaltyWinner = new Images("images/common/penalty_winner.png", nameof(PenaltyWinner));
        public static readonly Images GoalAssist = new Images("images/common/assist_goal.png", nameof(GoalAssist));
        public static readonly Images PenaltyGoal = new Images("images/common/penalty_goal.png", nameof(PenaltyGoal));
        public static readonly Images MissPenaltyGoal = new Images("images/common/missed_penalty_goal.png", nameof(PenaltyGoal));

        public Images()
        {
        }

        protected Images(string value, string name) : base(value, name)
        {
        }
    }
}