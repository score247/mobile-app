namespace LiveScore.Core.Factories
{
    using LiveScore.Core.Services;

    public interface ISportServiceFactory
    {
        IMatchService CreateMatchService();

        ILeagueService CreateLeagueService();
    }
}
