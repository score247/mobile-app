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
    public class H2HMatchViewModel : BaseViewModel
    {
        private readonly IMatchDisplayStatusBuilder matchDisplayStatusBuilder;

        public H2HMatchViewModel(
            bool isH2H,
            string teamId,
            IMatch match,
            IMatchDisplayStatusBuilder matchDisplayStatusBuilder
            )
        {
            IsH2H = isH2H;

            this.matchDisplayStatusBuilder = matchDisplayStatusBuilder;

            BuildMatch(teamId, match);
        }

        public IMatch Match { get; private set; }

        public string DisplayMatchStatus { get; private set; }

        public string DisplayEventDate { get; private set; }

        public bool IsH2H { get; private set; }

        public string Result { get; private set; }

        public void BuildMatch(string teamId, IMatch match)
        {
            if (match == null)
            {
                return;
            }

            Match = match;
            DisplayEventDate = BuildDisplayEventDate(match);
            DisplayMatchStatus = matchDisplayStatusBuilder.BuildDisplayStatus(Match);

            Result = IsH2H ? string.Empty : BuildTeamResult(teamId, match);
        }

        private static string BuildDisplayEventDate(IMatch match)
        {
            var currentYear = DateTimeOffset.Now.Year;

            return match.EventDate.Year == currentYear
                ? match.EventDate.ToLocalShortDayMonth()
                : match.EventDate.ToLocalYear();
        }

        private static string BuildTeamResult(string teamId, IMatch match)
        {
            if (string.IsNullOrWhiteSpace(match.WinnerId))
            {
                return TeamResult.Draw.DisplayName;
            }

            return match.WinnerId == teamId
                ? TeamResult.Win.DisplayName
                : TeamResult.Lose.DisplayName;
        }
    }
}