using System.Collections.Generic;
using LiveScore.Core.Models.Matches;
using LiveScore.Core.Models.Teams;
using MessagePack;

namespace LiveScore.Soccer.Models.Teams
{
    [MessagePackObject]
    public class HeadToHeads : IHeadToHeads
    {
        public HeadToHeads(IEnumerable<ITeam> teams, IEnumerable<IMatch> matches)
        {
            Teams = teams;
            Matches = matches;
        }

        [Key(0)]
        public IEnumerable<ITeam> Teams { get; }

        [Key(1)]
        public IEnumerable<IMatch> Matches { get; }
    }
}