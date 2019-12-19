using LiveScore.Core.ViewModels.Leagues;
using LiveScore.Core.Views.Templates.Leagues;
using Xamarin.Forms;

namespace LiveScore.Core.Views.Selectors
{
    public class LeagueItemSelector : DataTemplateSelector
    {
        private static readonly DataTemplate LeagueItemTemplate = new LeagueItemTemplate();
        private static readonly DataTemplate RegionGroupTemplate = new RegionGroupTemplate();

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return item switch
            {
                LeagueItemViewModel _ => LeagueItemTemplate,
                RegionGroupViewModel _ => RegionGroupTemplate,

                _ => LeagueItemTemplate,
            };
        }
    }
}