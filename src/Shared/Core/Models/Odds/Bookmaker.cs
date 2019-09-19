namespace LiveScore.Core.Models.Odds
{
    using System;
    using MessagePack;

    [MessagePackObject(keyAsPropertyName: true)]
    public class Bookmaker : IEquatable<Bookmaker>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public bool Equals(Bookmaker other)
        {
            if (other == null)
            {
                return false;
            }

            return Id.Equals(other.Id, StringComparison.OrdinalIgnoreCase)
                && Name.Equals(other.Name, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var bookmakerObject = obj as Bookmaker;

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

        public static bool operator ==(Bookmaker bookmaker1, Bookmaker bookmaker2)
        {
            if (((object)bookmaker1) == null || ((object)bookmaker2) == null)
            {
                return Object.Equals(bookmaker1, bookmaker2);
            }

            return bookmaker1.Equals(bookmaker2);
        }

        public static bool operator !=(Bookmaker bookmaker1, Bookmaker bookmaker2)
        {
            if (((object)bookmaker1) == null || ((object)bookmaker2) == null)
            {
                return !Object.Equals(bookmaker1, bookmaker2);
            }

            return !(bookmaker1.Equals(bookmaker2));
        }
    }
}