using LiveScore.Core.Models.Matches;

namespace LiveScore.Core.ViewModels
{
    using Common.Extensions;

    public class GroupMatchViewModel
    {
        public GroupMatchViewModel(IMatch match)
        {
            LeagueId = match.LeagueId;
            LeagueName = $"{match.CountryCode} {match.LeagueName.ToUpperInvariant()}";
            EventDate = match.EventDate.ToLocalShortDayMonth().ToUpperInvariant();
        }

        public string LeagueId { get; }

        public string LeagueName { get; }

        public string EventDate { get; }

        public override bool Equals(object obj)
            => (obj is GroupMatchViewModel actualObj) && LeagueId == actualObj.LeagueId && EventDate == actualObj.EventDate;

        public override int GetHashCode() => LeagueId?.GetHashCode() ?? 0;
    }
}