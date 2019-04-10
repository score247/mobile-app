namespace LiveScore.Core.Factories
{
    using LiveScore.Core.Services;

    public interface IServiceFactory : IFactory<IFactoryProvider<IServiceFactory>>
    {
        IMatchService CreateMatchService();

        ILeagueService CreateLeagueService();
    }
}
