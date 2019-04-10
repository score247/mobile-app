namespace LiveScore.Core.Factories
{
    public interface IFactory<TFactoryProvider>
    {
        void RegisterTo(TFactoryProvider factoryProvider);
    }
}
