namespace LiveScore.Core.Models.Leagues
{
    public interface ILeagueCategory
    {
        string CountryCode { get; }

        string CountryName { get; }
    }

    public class LeagueCategory : ILeagueCategory

    {
        public LeagueCategory(string countryCode, string countryName)
        {
            CountryCode = countryCode;
            CountryName = countryName;
        }

        public string CountryCode { get; }

        public string CountryName { get; }

        public override bool Equals(object obj)
            => (obj is LeagueCategory actualObj) && CountryCode == actualObj.CountryCode;

        public override int GetHashCode()
        {
            if (string.IsNullOrWhiteSpace(CountryCode))
            {
                return CountryName?.GetHashCode() ?? 0;
            }

            return CountryCode.GetHashCode();
        }
    }
}