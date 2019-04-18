namespace LiveScore.Core.Converters
{
    using LiveScore.Core.Constants;
    using LiveScore.Core.ViewModels;
    using Xamarin.Forms;

    public interface MatchItemTemplate
    { }

    public class MatchTemplateSelector : DataTemplateSelector
    {
        private MatchItemTemplate matchItemTemplate;

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var viewModel = container.BindingContext as ViewModelBase;

            //return viewModel.GlobalFactoryProvider.TemplateFactoryProvider
            //    .GetInstance((SportType)viewModel.SettingsService.CurrentSportId)
            //    .GetMatchTemplate();

            if (matchItemTemplate == null)
            {
                matchItemTemplate = viewModel.ServiceLocator.Create<MatchItemTemplate>(nameof(SportType.Soccer));
            }

            return (DataTemplate)matchItemTemplate;
        }
    }
}