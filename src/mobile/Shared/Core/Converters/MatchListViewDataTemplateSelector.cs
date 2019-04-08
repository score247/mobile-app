namespace LiveScore.Core.Converters
{
    using LiveScore.Core.Constants;
    using LiveScore.Core.Services;
    using Xamarin.Forms;

    public class MatchListViewDataTemplateSelector : DataTemplateSelector
    {
        private readonly ISettingsService settingsService;

        public MatchListViewDataTemplateSelector() : this(null)
        {
        }

        internal MatchListViewDataTemplateSelector(ISettingsService settingsService)
        {
            this.settingsService = settingsService ?? new SettingsService();
        }

        public DataTemplate SoccerTemplate { get; set; }

        public DataTemplate BasketBallTemplate { get; set; }

        public DataTemplate ESportsTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            switch (settingsService.CurrentSportId)
            {
                case (int)SportType.Soccer:
                    return SoccerTemplate;

                case (int)SportType.BasketBall:
                    return BasketBallTemplate;

                case (int)SportType.ESports:
                    return ESportsTemplate;

                default:
                    return SoccerTemplate;
            }
        }
    }
}
