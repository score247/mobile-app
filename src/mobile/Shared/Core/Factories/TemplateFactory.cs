namespace LiveScore.Core.Factories
{
    using Xamarin.Forms;

    public interface ITemplateFactory : IFactory<IFactoryProvider<ITemplateFactory>>
    {
        DataTemplate GetMatchTemplate();
    }
}
