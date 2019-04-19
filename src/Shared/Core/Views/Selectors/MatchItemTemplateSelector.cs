namespace LiveScore.Core.Views.Selectors
{
    using LiveScore.Core.ViewModels;
    using Xamarin.Forms;

    public class MatchItemTemplateSelector : DataTemplateSelector
    {
        private DataTemplate matchItemTemplate;
        private string SportName;

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var viewModel = container.BindingContext as ViewModelBase;

            if (string.IsNullOrWhiteSpace(SportName) ||
                SportName != viewModel.SettingsService.CurrentSportName)
            {
                SportName = viewModel.SettingsService.CurrentSportName;
                matchItemTemplate = viewModel.ServiceLocator.Create<DataTemplate>(SportName);
            }

            return matchItemTemplate;
        }
    }
}