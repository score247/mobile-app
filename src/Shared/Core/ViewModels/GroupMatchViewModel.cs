namespace LiveScore.Core.ViewModels
{
    using System;
    using LiveScore.Common.Extensions;

    public class GroupMatchViewModel
    {
        public GroupMatchViewModel(string leagueId, string leagueName, DateTimeOffset eventDate)
        {
            LeagueId = leagueId;
            LeagueName = leagueName.ToUpperInvariant();
            EventDate = eventDate.ToLocalShortDayMonth();
        }

        public string LeagueId { get; }

        public string LeagueName { get; }

        public string EventDate { get; }

        public override bool Equals(object obj)
        {
            var actualObj = obj as GroupMatchViewModel;

            if (actualObj == null)
            {
                return false;
            }

            return LeagueId == actualObj.LeagueId && EventDate == actualObj.EventDate;
        }

        public override int GetHashCode() => LeagueId.GetHashCode();
    }
}