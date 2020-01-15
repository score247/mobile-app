using System;
using LiveScore.Common.Extensions;
using LiveScore.Core.Converters;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Models.Matches;
using MvvmHelpers;
using PropertyChanged;

namespace LiveScore.Soccer.ViewModels.Matches.MatchDetails.HeadToHead
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

        public string Result { get; private set; } = string.Empty;

        public bool IsHomeSelected { get; private set; }

        public bool IsAwaySelected { get; private set; }

        public void BuildMatch(string teamId, IMatch match)
        {
            if (match == null)
            {
                return;
            }

            Match = match;
            DisplayEventDate = GetDisplayEventDate(match);
            DisplayMatchStatus = matchDisplayStatusBuilder.BuildDisplayStatus(Match);

            if (!IsH2H)
            {
                Result = GetTeamResult(teamId, match);
                IsHomeSelected = teamId == match.HomeTeamId;
                IsAwaySelected = !IsHomeSelected;
            }
        }

        private static string GetDisplayEventDate(IMatch match) => match.EventDate.ToH2HMatchShortDate();

        private static string GetTeamResult(string teamId, IMatch match)
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