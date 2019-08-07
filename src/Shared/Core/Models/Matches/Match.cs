namespace LiveScore.Core.Models.Matches
{
    using System;
    using System.Collections.Generic;
    using LiveScore.Core.Models.Leagues;
    using LiveScore.Core.Models.Teams;

    public interface IMatch : IEntity<string, string>
    {
        DateTime EventDate { get; set; }

        IEnumerable<ITeam> Teams { get; set; }

        IMatchResult MatchResult { get; set; }

        IEnumerable<ITimelineEvent> TimeLines { get; set; }

        IMatchCondition MatchCondition { get; set; }

        ILeague League { get; set; }

        int Attendance { get; set; }

        IVenue Venue { get; set; }

        string Referee { get; set; }

        ITimelineEvent LatestTimeline { get; set; }

        IEnumerable<MatchFunction> Functions { get; set; }
    }
}