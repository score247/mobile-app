using LiveScore.Features.Score.ViewModels;
using LiveScore.Features.Score.Views.Templates;
using Xamarin.Forms;

namespace LiveScore.Features.Score.Views.TemplateSelectors
{
    internal class ScoresTemplateSelector : DataTemplateSelector
    {
        private static readonly ScoreMatchesTemplate ScoreMatchesTemplate = new ScoreMatchesTemplate();
        private static readonly CalendarTemplate CalendarTemplate = new CalendarTemplate();

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return item switch
            {
                CalendarMatchesViewModel _ => CalendarTemplate,

                _ => ScoreMatchesTemplate,
            };
        }
    }
}