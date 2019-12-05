using System.Collections.Generic;
using LiveScore.Core.Models.Leagues;
using LiveScore.Soccer.Enumerations;
using LiveScore.Soccer.Models.Teams;
using MessagePack;

namespace LiveScore.Soccer.Models.Leagues
{
    [MessagePackObject(keyAsPropertyName: true)]
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

        
        public string Id { get; private set; }

        
        public string Name { get; private set; }

        
        public IEnumerable<LeagueGroupNote> GroupNotes { get; private set; }

        
        public IEnumerable<TeamStanding> TeamStandings { get; private set; }

        
        public IEnumerable<TeamOutcome> OutcomeList { get; private set; }
    }
}