namespace LiveScore.Core.Models.Leagues
{
    public interface ILeagueCategory : IEntity<string, string>
    {
        string CountryCode { get; }
    }

    public class LeagueCategory : Entity<string, string>, ILeagueCategory
    {
        public string CountryCode { get; set; }

        public override bool Equals(object obj)
            => (obj is LeagueCategory actualObj) && Name == actualObj.Name;

        public override int GetHashCode()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return CountryCode?.GetHashCode() ?? 0;
            }

            return Name.GetHashCode();
        }
    }
}