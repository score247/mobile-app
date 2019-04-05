namespace LiveScore.Domain.Models.Teams
{
    public interface ITeamStatistic
    {
        int BallPossession { get; }

        int FreeKicks { get; }

        int ThrowIns { get; }

        int GoalKicks { get; }

        int ShotsBlocked { get; }

        int ShotsOnTarget { get; }

        int ShotsOffTarget { get; }

        int CornerKicks { get; }

        int Fouls { get; }

        int ShotsSaved { get; }

        int Offsides { get; }

        int YellowCards { get; }

        int Injuries { get; }
    }

    public class TeamStatistic : ITeamStatistic
    {
        public int BallPossession { get; set; }

        public int FreeKicks { get; set; }

        public int ThrowIns { get; set; }

        public int GoalKicks { get; set; }

        public int ShotsBlocked { get; set; }

        public int ShotsOnTarget { get; set; }

        public int ShotsOffTarget { get; set; }

        public int CornerKicks { get; set; }

        public int Fouls { get; set; }

        public int ShotsSaved { get; set; }

        public int Offsides { get; set; }

        public int YellowCards { get; set; }

        public int Injuries { get; set; }
    }
}