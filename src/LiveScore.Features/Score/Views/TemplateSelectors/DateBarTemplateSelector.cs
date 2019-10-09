using LiveScore.Core.Controls.DateBar.Views.Templates;
using LiveScore.Features.Score.ViewModels;
using Xamarin.Forms;

namespace LiveScore.Features.Score.Views.TemplateSelectors
{
    internal class DateBarTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return item switch
            {
                LiveItemViewModel _ => new LiveTemplate(),
                CalendarItemViewModel _ => new CalendarTemplate(),

                _ => new DateTemplate(),
            };
        }
    }
}