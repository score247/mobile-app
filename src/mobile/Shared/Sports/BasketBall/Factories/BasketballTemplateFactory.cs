namespace LiveScore.Basketball.Factories
{
    using LiveScore.Basketball.Views.Templates;
    using LiveScore.Core.Factories;
    using Xamarin.Forms;

    public class BasketballTemplateFactory : ITemplateFactory
    {
        public DataTemplate GetMatchTemplate()
        {
            return new MatchDataTemplate();
        }
    }
}
