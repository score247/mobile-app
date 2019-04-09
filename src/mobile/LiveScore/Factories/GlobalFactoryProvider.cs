namespace LiveScore.Factories
{
    using Core.Factories;

    public class GlobalFactoryProvider : IGlobalFactoryProvider
    {
        public GlobalFactoryProvider()
        {
            SportServiceFactoryProvider = new SportServiceFactoryProvider();
        }

        public ISportServiceFactoryProvider SportServiceFactoryProvider { get; private set; }
    }
}
