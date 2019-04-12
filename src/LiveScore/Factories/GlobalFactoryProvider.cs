namespace LiveScore.Factories
{
    using Core.Factories;

    public class GlobalFactoryProvider : IGlobalFactoryProvider
    {
        public GlobalFactoryProvider()
        {
            ServiceFactoryProvider = new FactoryProvider<IServiceFactory>();
            TemplateFactoryProvider = new FactoryProvider<ITemplateFactory>();
        }

        public IFactoryProvider<IServiceFactory> ServiceFactoryProvider { get; private set; }

        public IFactoryProvider<ITemplateFactory> TemplateFactoryProvider { get; private set; }
    }
}
