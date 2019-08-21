using System;
using LiveScore.Core.Enumerations;

namespace LiveScore.Core.Models.Matches
{
    public interface IMatchSummary
    {
        string Id { get; }

        DateTimeOffset EventDate { get; }

        string LeagueId { get; }

        string LeagueName { get; }

        MatchStatus MatchStatus { get; }

        MatchStatus EventStatus { get; }
    }
}