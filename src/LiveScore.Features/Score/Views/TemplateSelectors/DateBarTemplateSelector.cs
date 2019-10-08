using LiveScore.Core.Controls.DateBar.Views.Templates;
using LiveScore.Features.Score.ViewModels;
using Xamarin.Forms;

namespace LiveScore.Features.Score.Views.TemplateSelectors
{
    internal class DateBarTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is LiveItemViewModel)
            {
                return new LiveTemplate();
            }

            if (item is CalendarItemViewModel)
            {
                return new CalendarTemplate();
            }

            return new DateTemplate();
        }
    }
}