namespace Core.Factories
{
    using Core.Contants;

    public interface IGlobalFactory
    {
        ISportServiceFactory BuildSportService(SportType sportType);
    }
}
