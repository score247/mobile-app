namespace Core.Factories
{
    using Core.Services;

    public interface ISportServiceFactory
    {
        IMatchService CreateMatchService();

        ILeagueService CreateLeagueService();
    }
}
