namespace LiveScore.Soccer.Models.Teams
{
    using LiveScore.Core.Models.Teams;
    using PropertyChanged;

    [AddINotifyPropertyChangedInterface]
    public class TeamStatistic : ITeamStatistic
    {
        public byte Possession { get; set; }

        public byte FreeKicks { get; set; }

        public byte ThrowIns { get; set; }

        public byte GoalKicks { get; set; }

        public byte ShotsBlocked { get; set; }

        public byte ShotsOnTarget { get; set; }

        public byte ShotsOffTarget { get; set; }

        public byte CornerKicks { get; set; }

        public byte Fouls { get; set; }

        public byte ShotsSaved { get; set; }

        public byte Offsides { get; set; }

        public byte YellowCards { get; set; }

        public byte YellowRedCards { get; set; }

        public byte RedCards { get; set; }

        public byte Injuries { get; set; }
    }
}