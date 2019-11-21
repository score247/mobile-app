using System;
using System.Collections.Generic;
using System.Linq;

namespace LiveScore.Soccer.ViewModels.MatchDetails.HeadToHead
{
    public class H2HMatchGroupViewModel : List<H2HMatchViewModel>
    {
        public H2HMatchGroupViewModel(
            IList<H2HMatchViewModel> matches,
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