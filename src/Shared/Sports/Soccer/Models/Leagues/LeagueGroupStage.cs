using System.Collections.Generic;
using LiveScore.Core.Models.Leagues;
using MessagePack;

namespace LiveScore.Soccer.Models.Leagues
{
    [MessagePackObject(keyAsPropertyName: true)]
    public class LeagueGroupStage : ILeagueGroupStage
    {
        public LeagueGroupStage(
            string leagueId,
            string leagueSeasonId,
            string groupStageName,
            bool hasStanding,
            LeagueRound leagueRound)
        {
            LeagueId = leagueId;
            LeagueSeasonId = leagueSeasonId;
            GroupStageName = groupStageName;
            HasStanding = hasStanding;
            LeagueRound = leagueRound;
        }

        public string LeagueId { get; }

        public string LeagueSeasonId { get; }

        public string GroupStageName { get; }

        public bool HasStanding { get; }

        public LeagueRound LeagueRound { get; }
    }

    /// <summary>
    /// Temp class for Message Pack generate AOT class
    /// </summary>
    [MessagePackObject(keyAsPropertyName: true)]
    public class LeagueGroupStageList
    {
        public IEnumerable<LeagueGroupStage> LeagueGroupStages { get; set; }
    }
}