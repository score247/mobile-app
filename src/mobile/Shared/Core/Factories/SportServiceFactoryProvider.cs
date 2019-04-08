namespace LiveScore.Core.Factories
{
    using System.Collections.Generic;
    using LiveScore.Core.Constants;

    public interface ISportServiceFactoryProvider
    {
        void RegisterInstance<T>(SportType sportType, T instance) where T : ISportServiceFactory;

        ISportServiceFactory GetInstance(SportType sportType);
    }

    public class SportServiceFactoryProvider : ISportServiceFactoryProvider
    {
        private readonly IDictionary<SportType, ISportServiceFactory> sportServiceFactories;

        public SportServiceFactoryProvider()
        {
            sportServiceFactories = new Dictionary<SportType, ISportServiceFactory>();
        }

        public void RegisterInstance<T>(SportType sportType, T instance) where T : ISportServiceFactory
        {
            sportServiceFactories.Add(sportType, instance);
        }

        public ISportServiceFactory GetInstance(SportType sportType)
        {
            return sportServiceFactories[sportType];
        }
    }
}
