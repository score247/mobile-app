namespace LiveScore.Core.Views.Selectors
{
    using LiveScore.Common.Extensions;
    using LiveScore.Core.Constants;
    using LiveScore.Core.ViewModels;
    using Xamarin.Forms;

    public class MatchItemTemplateSelector : DataTemplateSelector
    {
        private DataTemplate matchItemTemplate;
        private SportType sportType;

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var viewModel = container.BindingContext as ViewModelBase;

            if (sportType != viewModel.SettingsService.CurrentSport)
            {
                sportType = viewModel.SettingsService.CurrentSport;
                matchItemTemplate = viewModel.ServiceLocator.Create<DataTemplate>(sportType.GetDescription());
            }

            return matchItemTemplate;
        }
    }
}