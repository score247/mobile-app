namespace LiveScore.Core.Converters
{
    using Models.Matches;

    public interface IMatchMinuteConverter
    {
        string BuildMatchMinute(IMatch match);
    }

    public interface IMatchMinuteConverter<in T>
    {
        string BuildMatchMinute(T match);
    }
}