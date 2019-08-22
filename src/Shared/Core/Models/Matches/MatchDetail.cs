namespace LiveScore.Core.Models.Matches
{
    public interface IMatchDetail
    {
        IMatch Match { get; }

        int Attendance { get; }

        Venue Venue { get; }

        string Referee { get; }
    }
}