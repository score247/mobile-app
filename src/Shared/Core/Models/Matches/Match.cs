namespace LiveScore.Core.Models.Matches
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using LiveScore.Common.Extensions;
    using LiveScore.Core.Models.Leagues;
    using LiveScore.Core.Models.Teams;
    using Newtonsoft.Json;
    using PropertyChanged;

    public interface IMatch : IEntity<string, string>
    {
        DateTime EventDate { get; set; }

        IEnumerable<ITeam> Teams { get; set; }

        IMatchResult MatchResult { get; set; }

        IEnumerable<ITimeline> TimeLines { get; set; }

        IMatchCondition MatchCondition { get; set; }

        ILeague League { get; set; }

        int Attendance { get; set; }

        IVenue Venue { get; set; }

        string Referee { get; set; }

        ITimeline LatestTimeline { get; set; }

        IEnumerable<MatchFunction> Functions { get; set; }
    }
}