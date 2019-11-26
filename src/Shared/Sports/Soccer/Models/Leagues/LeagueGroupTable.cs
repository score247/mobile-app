using System.Collections.Generic;
using LiveScore.Core.Models.Leagues;
using LiveScore.Soccer.Enumerations;
using LiveScore.Soccer.Models.Teams;
using MessagePack;

namespace LiveScore.Soccer.Models.Leagues
{
    [MessagePackObject]
    public class LeagueGroupTable
    {
        public LeagueGroupTable(
            string id,
            string name,
            IEnumerable<LeagueGroupNote> groupNotes,
            IEnumerable<TeamStanding> teamStandings,
            IEnumerable<TeamOutcome> outcomeList)
        {
            Id = id;
            Name = name;
            GroupNotes = groupNotes;
            TeamStandings = teamStandings;
            OutcomeList = outcomeList;
        }

        [Key(0)]
        public string Id { get; }

        [Key(1)]
        public string Name { get; }

        [Key(2)]
        public IEnumerable<LeagueGroupNote> GroupNotes { get; }

        [Key(3)]
        public IEnumerable<TeamStanding> TeamStandings { get; }

        [Key(4)]
        public IEnumerable<TeamOutcome> OutcomeList { get; private set; }
    }
}