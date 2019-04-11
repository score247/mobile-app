namespace LiveScore.Core.Converters
{
    using LiveScore.Core.Constants;
    using LiveScore.Core.Factories;
    using LiveScore.Core.Services;
    using Xamarin.Forms;

    public class MatchTemplateSelector : DataTemplateSelector
    {
        private readonly IGlobalFactoryProvider globalFactoryProvider;
        private readonly ISettingsService settingsService;

        public MatchTemplateSelector(IGlobalFactoryProvider globalFactoryProvider, ISettingsService settingsService)
        {
            this.globalFactoryProvider = globalFactoryProvider;
            this.settingsService = settingsService;
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return globalFactoryProvider.TemplateFactoryProvider
                .GetInstance((SportType)settingsService.CurrentSportId)
                .GetMatchTemplate();
        }
    }
}
