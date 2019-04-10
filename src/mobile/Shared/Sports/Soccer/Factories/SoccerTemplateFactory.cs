namespace LiveScore.Soccer.Factories
{
    using LiveScore.Core.Constants;
    using LiveScore.Core.Factories;
    using LiveScore.Soccer.Views.Templates;
    using Xamarin.Forms;

    public class SoccerTemplateFactory : ITemplateFactory
    {
        public DataTemplate GetMatchTemplate()
        {
            return new MatchDataTemplate();
        }

        public void RegisterTo(IFactoryProvider<ITemplateFactory> factoryProvider)
        {
            factoryProvider.RegisterInstance(SportType.Soccer, this);
        }
    }
}
