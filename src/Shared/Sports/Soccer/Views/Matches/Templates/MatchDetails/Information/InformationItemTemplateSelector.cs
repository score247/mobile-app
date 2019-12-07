using LiveScore.Soccer.ViewModels.Matches.MatchDetails.Information.InfoItems;
using Xamarin.Forms;

namespace LiveScore.Soccer.Views.Templates.MatchDetails.Information
{
    public class InformationItemTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var itemViewModel = (BaseItemViewModel)item;

            return itemViewModel.CreateTemplate();
        }
    }
}