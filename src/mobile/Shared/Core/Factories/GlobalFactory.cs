namespace LiveScore.Core.Factories
{
    using LiveScore.Core.Contants;

    public interface IGlobalFactory
    {
        ISportServiceFactory BuildSportService(SportType sportType);
    }
}
