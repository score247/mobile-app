using System.Collections.Generic;
using MessagePack;

namespace LiveScore.Core.Models.Leagues
{
    public interface ILeagueGroupNote
    {
        string TeamId { get; }

        string TeamName { get; }

        IEnumerable<string> Comments { get; }
    }

    [MessagePackObject(keyAsPropertyName: true)]
    public class LeagueGroupNote : ILeagueGroupNote
    {
        public LeagueGroupNote(
            string teamId,
            string teamName,
            IEnumerable<string> comments)
        {
            TeamId = teamId;
            TeamName = teamName;
            Comments = comments;
        }

#pragma warning disable S109 // Magic numbers should not be used

        public string TeamId { get; }

        public string TeamName { get; }

#pragma warning restore S109 // Magic numbers should not be used
        public IEnumerable<string> Comments { get; }
    }
}