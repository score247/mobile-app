namespace LiveScore.Core.Models.Matches
{
    public interface IMatchCondition
    {
        int Attendance { get; }

        IVenue Venue { get; }

        string Referee { get; }
    }
}