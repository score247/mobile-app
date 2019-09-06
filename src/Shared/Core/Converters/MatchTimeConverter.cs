namespace LiveScore.Core.Converters
{
    using Models.Matches;

    public interface IMatchMinuteConverter
    {
        string BuildMatchMinute(IMatch match);
    }
}