using System;
using System.Collections.Generic;
using MessagePack;

namespace LiveScore.Core.Models.Odds
{
    public interface IOddsMovement
    {
        IEnumerable<BetOptionOdds> BetOptions { get; }

        string MatchTime { get; }

        int HomeScore { get; }

        int AwayScore { get; }

        DateTimeOffset UpdateTime { get; }

        bool IsMatchStarted { get; }
    }

    [MessagePackObject(keyAsPropertyName: true)]
    public class OddsMovement : IOddsMovement, IEquatable<OddsMovement>
    {
        public OddsMovement(
            string matchTime,
            int homeScore,
            int awayScore,
            bool isMatchStarted,
            IEnumerable<BetOptionOdds> betOptions,
            DateTimeOffset updateTime)
        {
            MatchTime = matchTime;
            HomeScore = homeScore;
            AwayScore = awayScore;
            IsMatchStarted = isMatchStarted;
            BetOptions = betOptions;
            UpdateTime = updateTime;
        }       

        public string MatchTime { get; private set; }

        public int HomeScore { get; private set; }

        public int AwayScore { get; private set; }

        public bool IsMatchStarted { get; private set; }

        public IEnumerable<BetOptionOdds> BetOptions { get; private set; }

        public DateTimeOffset UpdateTime { get; private set; }        

        public bool Equals(OddsMovement other)
        {
            if (other == null)
            {
                return false;
            }

            return UpdateTime == other.UpdateTime
                && MatchTime.Equals(other.MatchTime, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var bookmakerObject = obj as OddsMovement;

            if (bookmakerObject == null)
            {
                return false;
            }

            return Equals(bookmakerObject);
        }

        public override int GetHashCode()
            => (int)UpdateTime.Ticks;

        public static bool operator ==(OddsMovement oddMovement1, OddsMovement oddMovement2)
        {
            if (((object)oddMovement1) == null || ((object)oddMovement2) == null)
            {
                return Object.Equals(oddMovement1, oddMovement2);
            }

            return oddMovement1.Equals(oddMovement2);
        }

        public static bool operator !=(OddsMovement oddMovement1, OddsMovement oddMovement2)
        {
            if (((object)oddMovement1) == null || ((object)oddMovement2) == null)
            {
                return !Object.Equals(oddMovement1, oddMovement2);
            }

            return !oddMovement1.Equals(oddMovement2);
        }
    }
}