namespace LiveScore.Core.Models.Teams
{
    public interface ICoach : IEntity<int, string>
    {
        string Nationality { get; }

        string CountryCode { get; }
    }
}