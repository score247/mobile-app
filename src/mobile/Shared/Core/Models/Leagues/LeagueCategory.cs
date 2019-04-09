namespace LiveScore.Core.Models.Leagues
{
    public interface ILeagueCategory : IEntity<string, string>
    {
        string CountryCode { get; }
    }
}