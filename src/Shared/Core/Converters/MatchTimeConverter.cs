using LiveScore.Core.Models.Matches;

namespace LiveScore.Core.Converters
{
    public interface IMatchMinuteBuilder
    {
        string BuildMatchMinute(IMatch match);
    }
}