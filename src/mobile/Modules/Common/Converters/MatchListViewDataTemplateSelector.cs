namespace Common.Converters
{
    using Common.Contants;
    using Common.Settings;
    using Xamarin.Forms;

    public class MatchListViewDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SoccerTemplate { get; set; }

        public DataTemplate BasketBallTemplate { get; set; }

        public DataTemplate ESportsTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            switch (Settings.CurrentSportId)
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
