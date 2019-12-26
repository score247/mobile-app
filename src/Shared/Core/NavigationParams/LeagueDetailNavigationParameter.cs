namespace LiveScore.Core.NavigationParams
{
    public class LeagueDetailNavigationParameter
    {
        public LeagueDetailNavigationParameter(string id, string seasonId, string roundGroup)
            : this(id, null, 0, null, false, roundGroup, seasonId)
        {
        }

        public LeagueDetailNavigationParameter(string id, string name, int order, string countryCode, bool isInternational, string roundGroup, string seasonId)
        {
            Id = id;
            Name = name;
            Order = order;
            CountryCode = countryCode;
            IsInternational = isInternational;
            RoundGroup = roundGroup;
            SeasonId = seasonId;
        }

        public string Id { get; private set; }

        public string Name { get; private set; }

        public int Order { get; private set; }

        public string CountryCode { get; private set; }

        public bool IsInternational { get; private set; }

        public string RoundGroup { get; private set; }

        public string SeasonId { get; private set; }
    }
}