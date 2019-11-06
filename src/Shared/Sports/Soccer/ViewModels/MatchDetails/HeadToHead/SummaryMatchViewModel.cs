using System;
using LiveScore.Common.Extensions;
using LiveScore.Core.Converters;
using LiveScore.Core.Enumerations;
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
            IMatchDisplayStatusBuilder matchDisplayStatusBuilder,
            bool isH2H,
            string teamId)
        {
            this.matchDisplayStatusBuilder = matchDisplayStatusBuilder;
            IsH2H = isH2H;

            BuildMatch(match, teamId);
        }

        public IMatch Match { get; private set; }

        public string DisplayMatchStatus { get; private set; }

        public string DisplayEventDate { get; private set; }

        public bool IsH2H { get; private set; }

        public string Result { get; private set; }

        public void BuildMatch(IMatch match, string teamId)
        {
            if (match == null)
            {
                return;
            }

            Match = match;
            DisplayEventDate = BuildDisplayEventDate(match);
            DisplayMatchStatus = matchDisplayStatusBuilder.BuildDisplayStatus(Match);
            Result = BuildTeamResult(match, teamId);
        }

        private static string BuildDisplayEventDate(IMatch match)
        {
            var currentYear = DateTimeOffset.Now.Year;

            return match.EventDate.Year == currentYear
                ? match.EventDate.ToLocalShortDayMonth()
                : match.EventDate.ToLocalYear();
        }

        private static string BuildTeamResult(IMatch match, string teamId)
        {
            if (string.IsNullOrWhiteSpace(match.WinnerId))
            {
                return TeamResult.Draw.DisplayName;
            }
            else
            {
                return match.WinnerId == teamId ? TeamResult.Win.DisplayName : TeamResult.Loose.DisplayName;
            }
        }
    }
}