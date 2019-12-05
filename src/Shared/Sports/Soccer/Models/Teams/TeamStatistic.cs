using LiveScore.Core.Models.Teams;
using MessagePack;
using PropertyChanged;

namespace LiveScore.Soccer.Models.Teams
{
    [AddINotifyPropertyChangedInterface, MessagePackObject(keyAsPropertyName: true)]
    public class TeamStatistic : ITeamStatistic
    {
        [SerializationConstructor]
#pragma warning disable S107 // Methods should not have too many parameters
        public TeamStatistic(
             byte possession,
             byte freeKicks,
             byte throwIns,
             byte goalKicks,
             byte shotsBlocked,
             byte shotsOnTarget,
             byte shotsOffTarget,
             byte cornerKicks,
             byte fouls,
             byte shotsSaved,
             byte offsides,
             byte yellowCards,
             byte injuries,
             byte redCards,
             byte yellowRedCards)
#pragma warning restore S107 // Methods should not have too many parameters
        {
            Possession = possession;
            FreeKicks = freeKicks;
            ThrowIns = throwIns;
            GoalKicks = goalKicks;
            ShotsBlocked = shotsBlocked;
            ShotsOnTarget = shotsOnTarget;
            ShotsOffTarget = shotsOffTarget;
            CornerKicks = cornerKicks;
            Fouls = fouls;
            ShotsSaved = shotsSaved;
            Offsides = offsides;
            YellowCards = yellowCards;
            Injuries = injuries;
            RedCards = redCards;
            YellowRedCards = yellowRedCards;
        }

        public byte Possession { get; }

        public byte FreeKicks { get; }

        public byte ThrowIns { get; }

        public byte GoalKicks { get; }

        public byte ShotsBlocked { get; }

        public byte ShotsOnTarget { get; }

        public byte ShotsOffTarget { get; }

        public byte CornerKicks { get; }

        public byte Fouls { get; }

        public byte ShotsSaved { get; }

        public byte Offsides { get; }

        public byte YellowCards { get; }

        public byte Injuries { get; }

        public byte RedCards { get; }

        public byte YellowRedCards { get; }
    }
}