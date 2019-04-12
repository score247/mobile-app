namespace LiveScore.Core.Factories
{
    using LiveScore.Core.Services;

    public interface IServiceFactory
    {
        IMatchService CreateMatchService();

        ILeagueService CreateLeagueService();
    }
}
