using LiveScore.Core.Models.Teams;
using PropertyChanged;

namespace LiveScore.Soccer.Models.Teams
{
    [AddINotifyPropertyChangedInterface]
    public class TeamStatistic : ITeamStatistic
    {
        public TeamStatistic(byte redCards, byte yellowRedCards)
        {
            RedCards = redCards;
            YellowRedCards = yellowRedCards;
        }

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