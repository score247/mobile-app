using LiveScore.Core.Models.Matches;

namespace LiveScore.Core.Converters
{
    public interface IMatchStatusConverter
    {
        string BuildStatus(IMatch match);
    }
}