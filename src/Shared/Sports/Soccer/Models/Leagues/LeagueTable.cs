using System.Collections.Generic;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Models.Leagues;
using MessagePack;

namespace LiveScore.Soccer.Models.Leagues
{
    [MessagePackObject]
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

        [Key(0)]
        public League League { get; }

        [Key(1)]
        public LeagueTableType Type { get; }

        [Key(2)]
        public LeagueSeason LeagueSeason { get; }

        [Key(3)]
        public IEnumerable<LeagueGroupTable> GroupTables { get; private set; }
    }
}