using LiveScore.Core.Models.Matches;

namespace LiveScore.Core.Converters
{
    public interface IMatchDisplayStatusBuilder
    {
        string BuildDisplayStatus(IMatch match);
    }
}