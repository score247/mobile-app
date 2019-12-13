using LiveScore.Core.Controls.DateBar.Views.Templates;
using LiveScore.Features.Score.ViewModels;
using Xamarin.Forms;

namespace LiveScore.Features.Score.Views.TemplateSelectors
{
    internal class DateBarTemplateSelector : DataTemplateSelector
    {
        private static readonly LiveTemplate LiveTemplate = new LiveTemplate();
        private static readonly CalendarTemplate CalendarTemplate = new CalendarTemplate();
        private static readonly DateTemplate DateTemplate = new DateTemplate();

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return item switch
            {
                LiveMatchesViewModel _ => LiveTemplate,
                CalendarMatchesViewModel _ => CalendarTemplate,

                _ => DateTemplate,
            };
        }
    }
}