using LiveScore.Core.Models.Matches;

namespace LiveScore.Core.Converters
{
    public interface IMatchMinuteBuilder
    {
        string BuildMatchMinute(IMatch match);
    }


    //TODO: Anders will impl later
    public interface IMatchMinuteBuilder<in TMatch> : IMatchMinuteBuilder
    {
        string BuildMatchMinute(TMatch match);
    }
}