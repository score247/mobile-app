using LiveScore.Soccer.ViewModels.Matches.MatchDetails.LineUps;
using Xamarin.Forms;

namespace LiveScore.Soccer.Views.Templates.MatchDetails.LineUps
{
    public class LineupsItemTemplateSelector : DataTemplateSelector
    {
        private static readonly DataTemplate SubsitutionTemplate = new SubstitutionTemplate();
        private static readonly DataTemplate LineupsPlayerTemplate = new LineupsPlayerTemplate();

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var itemViewModel = (LineupsItemViewModel)item;

            return itemViewModel.IsSubstitution ? SubsitutionTemplate : LineupsPlayerTemplate;
        }
    }
}