namespace LiveScore.Core.Factories
{
    public interface IGlobalFactoryProvider
    {
        ISportServiceFactoryProvider SportServiceFactoryProvider { get; }
    }
}
