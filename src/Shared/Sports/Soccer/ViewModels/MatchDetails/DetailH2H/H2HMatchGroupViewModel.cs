using System;
using System.Collections.Generic;
using System.Linq;

namespace LiveScore.Soccer.ViewModels.MatchDetails.DetailH2H
{
    public class H2HMatchGroupViewModel : List<SummaryMatchViewModel>
    {
        public H2HMatchGroupViewModel(
            IEnumerable<SummaryMatchViewModel> matches,
            Func<string, string> buildFlagUrl) : base(matches)
        {
            var match = this.FirstOrDefault();
            LeagueName = match?.Match.LeagueGroupName;
            CountryFlag = buildFlagUrl(match?.Match.CountryCode);
        }

        public string LeagueName { get; }

        public string CountryFlag { get; }
    }
}