namespace LiveScore.Core.Factories
{
    using LiveScore.Core.Constants;

    public interface IGlobalFactory
    {
        ISportServiceFactory BuildSportService(SportType sportType);
    }
}
