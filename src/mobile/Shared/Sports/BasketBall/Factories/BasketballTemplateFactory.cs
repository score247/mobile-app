namespace LiveScore.Basketball.Factories
{
    using LiveScore.Basketball.Views.Templates;
    using LiveScore.Core.Factories;
    using Xamarin.Forms;

    public class BasketballTemplateFactory : ITemplateFactory
    {
        private MatchDataTemplate matchDataTemplate;

        public DataTemplate GetMatchTemplate()
        {
            if (matchDataTemplate == null)
            {
                matchDataTemplate = new MatchDataTemplate();
            }

            return matchDataTemplate;
        }
    }
}
