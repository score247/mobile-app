using LiveScore.Soccer.ViewModels.MatchDetails.LineUps;
using Xamarin.Forms;

namespace LiveScore.Soccer.Views.Templates.MatchDetails.LineUps
{
    public class LineupsItemTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var itemViewModel = (LineupsItemViewModel)item;

            return itemViewModel.CreateTemplate();
        }
    }
}