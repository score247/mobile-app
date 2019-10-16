using LiveScore.Core.Models.Matches;

namespace LiveScore.Core.Converters
{
    public interface IMatchMinuteConverter
    {
        string BuildMatchMinute(IMatch match);
    }

    public interface IMatchMinuteConverter<in T>
    {
        string BuildMatchMinute(T match);
    }
}