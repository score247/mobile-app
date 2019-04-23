namespace LiveScore.Core.Views.Selectors
{
    using LiveScore.Common.Extensions;
    using LiveScore.Core.ViewModels;
    using Xamarin.Forms;

    public class MatchItemTemplateSelector : DataTemplateSelector
    {
        private DataTemplate matchItemTemplate;

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (matchItemTemplate == null)
            {
                var viewModel = container.BindingContext as ViewModelBase;
                var sportType = viewModel.SettingsService.CurrentSport;

                matchItemTemplate = viewModel.DepdendencyResolver.Resolve<DataTemplate>(sportType.GetDescription());
            }

            return matchItemTemplate;
        }
    }
}