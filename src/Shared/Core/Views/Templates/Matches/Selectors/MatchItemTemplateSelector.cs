using LiveScore.Core.ViewModels;
using Xamarin.Forms;

namespace LiveScore.Core.Views.Templates.Matches.Selectors
{
    public class MatchItemTemplateSelector : DataTemplateSelector
    {
        private DataTemplate matchItemTemplate;

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (matchItemTemplate != null || !(container.BindingContext is ViewModelBase viewModel))
            {
                return matchItemTemplate;
            }

            var sportType = viewModel.CurrentSportId.ToString();

            matchItemTemplate = viewModel.DependencyResolver.Resolve<DataTemplate>(sportType);

            return matchItemTemplate;
        }
    }
}