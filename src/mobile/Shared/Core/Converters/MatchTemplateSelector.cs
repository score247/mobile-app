namespace LiveScore.Core.Converters
{
    using LiveScore.Core.Constants;
    using LiveScore.Core.ViewModels;
    using Xamarin.Forms;

    public class MatchTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var viewModel = container.BindingContext as ViewModelBase;

            return viewModel.GlobalFactoryProvider.TemplateFactoryProvider
                .GetInstance((SportType)viewModel.SettingsService.CurrentSportId)
                .GetMatchTemplate();
        }
    }
}
