namespace LiveScore.Core.Factories
{
    using LiveScore.Core.Services;

    public interface ISportServiceFactory
    {
        void RegisterTo(ISportServiceFactoryProvider sportServiceFactoryProvider);

        IMatchService CreateMatchService();

        ILeagueService CreateLeagueService();
    }
}
