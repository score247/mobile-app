namespace LiveScore.Core.Views.Selectors
{
    using LiveScore.Core.Constants;
    using LiveScore.Core.ViewModels;
    using Xamarin.Forms;

    public interface MatchItemTemplate
    {
    }

    public class MatchItemTemplateSelector : DataTemplateSelector
    {
        private MatchItemTemplate matchItemTemplate;

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var viewModel = container.BindingContext as ViewModelBase;

            if (matchItemTemplate == null)
            {
                matchItemTemplate = viewModel.ServiceLocator.Create<MatchItemTemplate>(nameof(SportType.Soccer));
            }

            return (DataTemplate)matchItemTemplate;
        }
    }
}