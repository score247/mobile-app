using System;
using LiveScore.Common.Extensions;
using LiveScore.Core.Converters;
using LiveScore.Core.Models.Matches;
using MvvmHelpers;
using PropertyChanged;

namespace LiveScore.Soccer.ViewModels.MatchDetails.HeadToHead
{
    [AddINotifyPropertyChangedInterface]
    public class SummaryMatchViewModel : BaseViewModel
    {
        private readonly IMatchDisplayStatusBuilder matchDisplayStatusBuilder;

        public SummaryMatchViewModel(
            IMatch match,
            IMatchDisplayStatusBuilder matchDisplayStatusBuilder)
        {
            this.matchDisplayStatusBuilder = matchDisplayStatusBuilder;

            BuildMatch(match);
        }

        public IMatch Match { get; private set; }

        public string DisplayMatchStatus { get; private set; }

        public string DisplayEventDate { get; private set; }

        public void BuildMatch(IMatch match)
        {
            if (match == null)
            {
                return;
            }

            Match = match;
            DisplayEventDate = BuildDisplayEventDate(match);
            DisplayMatchStatus = matchDisplayStatusBuilder.BuildDisplayStatus(Match);
        }

        private static string BuildDisplayEventDate(IMatch match)
        {
            var currentYear = DateTimeOffset.Now.Year;

            return match.EventDate.Year == currentYear
                ? match.EventDate.ToLocalShortDayMonth()
                : match.EventDate.ToLocalYear();
        }
    }
}