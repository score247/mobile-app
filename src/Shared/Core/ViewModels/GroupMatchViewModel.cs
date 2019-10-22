using System;
using LiveScore.Common.Extensions;
using LiveScore.Core.Models.Matches;
using MvvmHelpers;

namespace LiveScore.Core.ViewModels
{
    public class GroupMatchViewModel : BaseViewModel
    {
        private static readonly Random random = new Random();

        public GroupMatchViewModel(IMatch match, Func<string, string> buildFlagUrl)
        {
            if (match == null)
            {
                IsBusy = true;
                return;
            }

            LeagueId = match.LeagueId;
            LeagueName = match.LeagueGroupName;
            EventDate = match.EventDate.ToLocalShortDayMonth().ToUpperInvariant();
            CountryFlag = buildFlagUrl(match.CountryCode);
            CountryCode = match.CountryCode;
            LeagueOrder = match.LeagueOrder;
            Match = match;
        }

        private IMatch Match { get; }

        public string LeagueId { get; }

        public string LeagueName { get; }

        public string EventDate { get; }

        public string CountryCode { get; }

        public string CountryFlag { get; }

        public int LeagueOrder { get; }

        public override bool Equals(object obj)
            => (obj is GroupMatchViewModel actualObj) && LeagueId == actualObj.LeagueId && EventDate == actualObj.EventDate;

        public override int GetHashCode()
        {
            if (string.IsNullOrWhiteSpace(LeagueId))
            {
                if (string.IsNullOrWhiteSpace(LeagueName))
                {
                    if (string.IsNullOrWhiteSpace(CountryCode))
                    {
                        return Match?.GetHashCode() ?? random.Next(0, 9);
                    }

                    return CountryCode.GetHashCode();
                }

                return LeagueName.GetHashCode();
            }

            return LeagueId.GetHashCode();
        }
    }
}