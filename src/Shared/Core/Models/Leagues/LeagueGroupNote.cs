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

    [MessagePackObject]
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

        [Key(0)]
        public string TeamId { get; }

        [Key(1)]
        public string TeamName { get; }

        [Key(2)]
#pragma warning restore S109 // Magic numbers should not be used
        public IEnumerable<string> Comments { get; }
    }
}