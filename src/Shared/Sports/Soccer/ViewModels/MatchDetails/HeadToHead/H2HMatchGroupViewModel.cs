using System;
using System.Collections.Generic;
using System.Linq;

namespace LiveScore.Soccer.ViewModels.MatchDetails.HeadToHead
{
    public class H2HMatchGroupViewModel : List<SummaryMatchViewModel>
    {
        public H2HMatchGroupViewModel(
            IList<SummaryMatchViewModel> matches,
            Func<string, string> buildFlagUrl) : base(matches)
        {
            var match = this.FirstOrDefault()?.Match;
            LeagueName = match?.LeagueGroupName;
            CountryFlag = buildFlagUrl(match?.CountryCode);
        }

        public string LeagueName { get; }

        public string CountryFlag { get; }
    }
}