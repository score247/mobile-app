using System;
using System.Collections.Generic;
using System.Linq;
using LiveScore.Core.Models.Matches;
using MvvmHelpers;

namespace LiveScore.Soccer.ViewModels.MatchDetails.DetailH2H
{
    public class H2HMatchGroupViewModel : List<SummaryMatchViewModel>
    {
        public H2HMatchGroupViewModel(IEnumerable<SummaryMatchViewModel> matches, Func<string, string> buildFlagUrl) : base(matches)
        {
            var match = this.FirstOrDefault();
            LeagueName = match?.Match.LeagueGroupName;
            CountryFlag = buildFlagUrl(match?.Match.CountryCode);
        }

        public string LeagueName { get; }

        public string CountryFlag { get; }
    }

    public class GroupHeaderMatchViewModel : BaseViewModel
    {
        public GroupHeaderMatchViewModel(IMatch match)
        {
            if (match == null)
            {
                IsBusy = true;
                return;
            }

            LeagueId = match.LeagueId;
            LeagueName = match.LeagueGroupName;
            LeagueSeasonId = match.LeagueSeasonId;

            Match = match;
        }

        private IMatch Match { get; }

        public string LeagueId { get; }

        public string LeagueName { get; }

        public string LeagueSeasonId { get; }

        public override bool Equals(object obj)
            => (obj is GroupHeaderMatchViewModel actualObj) && LeagueId == actualObj.LeagueId && LeagueSeasonId == actualObj.LeagueSeasonId;

        public override int GetHashCode()
        {
            if (string.IsNullOrWhiteSpace(LeagueSeasonId))
            {
                if (string.IsNullOrWhiteSpace(LeagueId))
                {
                    return LeagueName?.GetHashCode() ?? 0;
                }

                return LeagueId.GetHashCode();
            }

            return LeagueSeasonId.GetHashCode();
        }
    }
}