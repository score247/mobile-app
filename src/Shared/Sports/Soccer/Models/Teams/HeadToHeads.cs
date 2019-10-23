using System.Collections.Generic;
using LiveScore.Core.Models.Matches;
using LiveScore.Core.Models.Teams;

namespace LiveScore.Soccer.Models.Teams
{
    public class HeadToHeads : IHeadToHeads
    {
        public HeadToHeads(IEnumerable<ITeam> teams, IEnumerable<IMatch> matches)
        {
            Teams = teams;
            Matches = matches;
        }

        public IEnumerable<ITeam> Teams { get; }

        public IEnumerable<IMatch> Matches { get; }
    }
}
