namespace LiveScore.Core.NavigationParams
{
    public class LeagueDetailNavigationParameter
    {
#pragma warning disable S107 // Methods should not have too many parameters

        public LeagueDetailNavigationParameter(
            string id,
            string leagueGroupName,
            int order,
            string countryCode,
            bool isInternational,
            string roundGroup,
            string seasonId,
            bool hasStanding,
            bool hasGroups = false)
#pragma warning restore S107 // Methods should not have too many parameters
        {
            Id = id;
            LeagueGroupName = leagueGroupName;
            Order = order;
            CountryCode = countryCode;
            IsInternational = isInternational;
            RoundGroup = roundGroup;
            SeasonId = seasonId;
            HasStanding = hasStanding;
            HasGroups = hasGroups;
        }

        public string Id { get; private set; }

        public string LeagueGroupName { get; private set; }

        public int Order { get; private set; }

        public string CountryCode { get; private set; }

        public bool IsInternational { get; private set; }

        public string RoundGroup { get; private set; }

        public string SeasonId { get; private set; }

        public bool HasStanding { get; private set; }

        public bool HasGroups { get; private set; }
    }
}