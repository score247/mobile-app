namespace LiveScore.Core.Views.Selectors
{
    using LiveScore.Core.ViewModels;
    using Xamarin.Forms;

    public class MatchItemTemplateSelector : DataTemplateSelector
    {
        private DataTemplate matchItemTemplate;

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var viewModel = container.BindingContext as ViewModelBase;
            var template = viewModel.ServiceLocator.Create<DataTemplate>(viewModel.SettingsService.CurrentSportName);

            if (matchItemTemplate?.GetType() != template?.GetType())
            {
                matchItemTemplate = template;
            }

            return matchItemTemplate;
        }
    }
}