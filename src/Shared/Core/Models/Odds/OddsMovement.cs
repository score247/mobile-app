namespace LiveScore.Core.Models.Odds
{
    using System;
    using System.Collections.Generic;

    public interface IOddsMovement : IEntity<int, string>
    {
        IEnumerable<BetOptionOdds> BetOptions { get; }

        string MatchTime { get; }

        int HomeScore { get; }

        int AwayScore { get; }

        DateTimeOffset UpdateTime { get; }

        bool IsMatchStarted { get; }
    }

    public class OddsMovement : Entity<int, string>, IOddsMovement, IEquatable<OddsMovement>
    {
        public IEnumerable<BetOptionOdds> BetOptions { get; set; }

        public string MatchTime { get; set; }

        public int HomeScore { get; set; }

        public int AwayScore { get; set; }

        public DateTimeOffset UpdateTime { get; set; }

        public bool IsMatchStarted { get; set; }

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
        {
            return Id.GetHashCode();
        }

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