using System;
using LiveScore.Common.Extensions;
using LiveScore.Core.Converters;
using LiveScore.Core.Models.Matches;
using MvvmHelpers;
using PropertyChanged;

namespace LiveScore.Soccer.ViewModels.MatchDetails.DetailH2H
{
    [AddINotifyPropertyChangedInterface]
    public class SummaryMatchViewModel : BaseViewModel
    {
        private readonly IMatchDisplayStatusBuilder matchDisplayStatusBuilder;
        private readonly IMatchMinuteBuilder matchMinuteBuilder;

        public SummaryMatchViewModel(
            IMatch match,
            IMatchDisplayStatusBuilder matchDisplayStatusBuilder,
            IMatchMinuteBuilder matchMinuteBuilder)
        {
            this.matchDisplayStatusBuilder = matchDisplayStatusBuilder;
            this.matchMinuteBuilder = matchMinuteBuilder;

            BuildMatch(match);
        }

        public IMatch Match { get; private set; }

        public string DisplayMatchStatus { get; private set; }

        public string DisplayEventDate { get; private set; }

        public void BuildMatch(IMatch match)
        {
            Match = match;
            DisplayEventDate = BuildDisplayEventDate(match);
            BuildDisplayMatchStatus();
        }

        private string BuildDisplayEventDate(IMatch match)
        {
            var currentYear = DateTimeOffset.Now.Year;

            return match.EventDate.Year == currentYear
                ? match.EventDate.ToLocalShortDayMonth()
                : match.EventDate.ToLocalYear();
        }

        private void BuildDisplayMatchStatus()
        {
            var matchStatus = matchDisplayStatusBuilder.BuildDisplayStatus(Match);

            DisplayMatchStatus = string.IsNullOrWhiteSpace(matchStatus)
                ? matchMinuteBuilder.BuildMatchMinute(Match)
                : matchStatus;
        }
    }
}
