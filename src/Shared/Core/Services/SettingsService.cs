namespace LiveScore.Core.Services
{
    using Enumerations;
    using Xamarin.Essentials;

    public interface ISettings
    {
        bool IsDemo { get; set; }

        byte SportId { get; set; }

        SportType CurrentSportType { get; }

        string LanguageCode { get; set; }

        Language CurrentLanguage { get; }

        string ApiEndpoint { get; set; }

        string HubEndpoint { get; set; }

        void Set(string key, string value);

        string Get(string key);
    }

    public class Settings : ISettings
    {
        public SportType CurrentSportType => Enumeration.FromValue<SportType>(SportId);

        public byte SportId
        {
            get => (byte)Preferences.Get(nameof(SportId), SportType.Soccer.Value);
            set => Preferences.Set(nameof(SportId), value);
        }

        public string LanguageCode
        {
            get => Preferences.Get(nameof(LanguageCode), Language.English.DisplayName);
            set => Preferences.Set(nameof(LanguageCode), value);
        }

        public Language CurrentLanguage => Enumeration.FromDisplayName<Language>(LanguageCode);

        public bool IsDemo { get; set; }

        public string ApiEndpoint
        {
            get => Preferences.Get(nameof(ApiEndpoint), string.Empty);
            set => Preferences.Set(nameof(ApiEndpoint), value);
        }

        public string HubEndpoint
        {
            get => Preferences.Get(nameof(HubEndpoint), string.Empty);
            set => Preferences.Set(nameof(HubEndpoint), value);
        }

        public string Get(string key) => Preferences.Get(key, string.Empty);

        public void Set(string key, string value) => Preferences.Set(key, value);
    }
}