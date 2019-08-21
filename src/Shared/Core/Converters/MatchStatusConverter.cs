namespace LiveScore.Core.Converters
{
    using LiveScore.Core.Models.Matches;

    public interface IMatchStatusConverter
    {
        string BuildStatus(IMatchSummary match);
    }
}