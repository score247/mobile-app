namespace LiveScore.Basketball.Factories
{
    using LiveScore.Basketball.Views.Templates;
    using LiveScore.Core.Constants;
    using LiveScore.Core.Factories;
    using Xamarin.Forms;

    public class BasketballTemplateFactory : ITemplateFactory
    {
        public DataTemplate GetMatchTemplate()
        {
            return new MatchDataTemplate();
        }

        public void RegisterTo(IFactoryProvider<ITemplateFactory> factoryProvider)
        {
            factoryProvider.RegisterInstance(SportType.Basketball, this);
        }
    }
}
