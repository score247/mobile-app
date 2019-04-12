namespace LiveScore.Core.Factories
{
    public interface IGlobalFactoryProvider
    {
        IFactoryProvider<IServiceFactory> ServiceFactoryProvider { get; }

        IFactoryProvider<ITemplateFactory> TemplateFactoryProvider { get; }
    }
}
