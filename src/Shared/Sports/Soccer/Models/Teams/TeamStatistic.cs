using LiveScore.Core.Models.Teams;
using MessagePack;
using PropertyChanged;

namespace LiveScore.Soccer.Models.Teams
{
    [AddINotifyPropertyChangedInterface, MessagePackObject]
    public class TeamStatistic : ITeamStatistic
    {
        [SerializationConstructor]
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

        [Key(0)]
        public byte Possession { get; }

        [Key(1)]
        public byte FreeKicks { get; }

        [Key(2)]
        public byte ThrowIns { get; }

        [Key(3)]
        public byte GoalKicks { get; }

        [Key(4)]
        public byte ShotsBlocked { get; }

        [Key(5)]
        public byte ShotsOnTarget { get; }

        [Key(6)]
        public byte ShotsOffTarget { get; }


        [Key(7)]
        public byte CornerKicks { get; }


        [Key(8)]
        public byte Fouls { get; }


        [Key(9)]
        public byte ShotsSaved { get; }


        [Key(10)]
        public byte Offsides { get; }


        [Key(11)]
        public byte YellowCards { get; }


        [Key(12)]
        public byte Injuries { get; }


        [Key(13)]
        public byte RedCards { get; }


        [Key(14)]
        public byte YellowRedCards { get; }
    }
}