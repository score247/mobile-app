using System.Collections.Generic;
using LiveScore.Core.Models.Matches;

namespace LiveScore.Core.Models.Teams
{
    public interface IHeadToHeads
    {       
        IEnumerable<ITeam> Teams { get; }

        IEnumerable<IMatch> Matches { get; }
    }
}
