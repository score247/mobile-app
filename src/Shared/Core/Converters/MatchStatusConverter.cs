namespace LiveScore.Core.Converters
{
    using Models.Matches;

    public interface IMatchStatusConverter
    {
        string BuildStatus(IMatch match);
    }
}