namespace LiveScore.Core.Factories
{
    using System.Collections.Generic;
    using LiveScore.Core.Constants;

    public interface IFactoryProvider<TFactory>
    {
        void RegisterInstance<TFactoryInstance>(SportType sportType, TFactoryInstance instance) where TFactoryInstance : TFactory;

        TFactory GetInstance(SportType sportType);
    }

    public class FactoryProvider<TFactory> : IFactoryProvider<TFactory>
    {
        private readonly IDictionary<SportType, TFactory> fatories;

        public FactoryProvider()
        {
            fatories = new Dictionary<SportType, TFactory>();
        }

        public void RegisterInstance<TFactoryInstance>(SportType sportType, TFactoryInstance instance)
            where TFactoryInstance : TFactory
        {
            fatories.Add(sportType, instance);
        }

        public TFactory GetInstance(SportType sportType)
        {
            return fatories[sportType];
        }
    }
}
