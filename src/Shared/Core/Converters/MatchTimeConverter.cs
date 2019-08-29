namespace LiveScore.Core.Converters
{
    using LiveScore.Core.Models.Matches;

    public interface IMatchMinuteConverter
    {
        string BuildMatchMinute(IMatch match);
    }
}