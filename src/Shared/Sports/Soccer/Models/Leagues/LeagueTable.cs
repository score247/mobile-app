using System.Collections.Generic;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Models.Leagues;
using MessagePack;

namespace LiveScore.Soccer.Models.Leagues
{
    [MessagePackObject(keyAsPropertyName: true)]
    public class LeagueTable : ILeagueTable
    {
        public LeagueTable(
            League league,
            LeagueTableType type,
            LeagueSeason leagueSeason,
            IEnumerable<LeagueGroupTable> groupTables)
        {
            League = league;
            Type = type;
            LeagueSeason = leagueSeason;
            GroupTables = groupTables;
        }

        
        public League League { get; }

        
        public LeagueTableType Type { get; }

        
        public LeagueSeason LeagueSeason { get; }

        
        public IEnumerable<LeagueGroupTable> GroupTables { get; private set; }
    }
}