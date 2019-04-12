namespace LiveScore.Soccer.Factories
{
    using LiveScore.Core.Factories;
    using LiveScore.Soccer.Views.Templates;
    using Xamarin.Forms;

    public class SoccerTemplateFactory : ITemplateFactory
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
