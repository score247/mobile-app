using System;
using LiveScore.Common.Extensions;
using LiveScore.Core.Models.Matches;

namespace LiveScore.Core.ViewModels
{
    public class GroupMatchViewModel
    {
        public GroupMatchViewModel(IMatch match, Func<string,string> buildFlagUrl)
        {
            LeagueId = match.LeagueId;
            LeagueName = match.LeagueGroupName;
            EventDate = match.EventDate.ToLocalShortDayMonth().ToUpperInvariant();
            CountryFlag = buildFlagUrl(match.CountryCode);
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