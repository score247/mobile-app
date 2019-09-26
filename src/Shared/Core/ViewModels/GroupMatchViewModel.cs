using LiveScore.Core.Models.Matches;

namespace LiveScore.Core.ViewModels
{
    using Common.Extensions;

    public class GroupMatchViewModel
    {
        public GroupMatchViewModel(IMatch match, string assetsEndPoint)
        {
            LeagueId = match.LeagueId;
            LeagueName = match.LeagueGroupName;
            EventDate = match.EventDate.ToLocalShortDayMonth().ToUpperInvariant();
            // TODO: Ricky Should be reusable here
            CountryFlag = $"{assetsEndPoint}flags/{match.CountryCode}.svg";
        }

        public string LeagueId { get; }

        public string LeagueName { get; }

        public string EventDate { get; }

        public string CountryCode { get; }

        public string CountryFlag { get; }

        public override bool Equals(object obj)
            => (obj is GroupMatchViewModel actualObj) && LeagueId == actualObj.LeagueId && EventDate == actualObj.EventDate;

        public override int GetHashCode() => LeagueId?.GetHashCode() ?? 0;
    }
}