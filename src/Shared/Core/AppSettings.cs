using LiveScore.Core.Enumerations;
using Xamarin.Essentials;

namespace LiveScore.Core
{
    public static class AppSettings
    {
        private static volatile ISettings instance;

        private static readonly object padlock = new object();

        public static ISettings Current
        {
            get
            {
                if (instance != null)
                {
                    return instance;
                }

                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Settings();
                    }
                }

                return instance;
            }
        }
    }

    public interface ISettings
    {
        bool IsDemo { get; set; }

        byte SportId { get; set; }

        SportType CurrentSportType { get; }

        string LanguageCode { get; set; }

        Language CurrentLanguage { get; }

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

        public bool IsDemo
        {
            get => Preferences.Get(nameof(IsDemo), false);
            set => Preferences.Set(nameof(IsDemo), value);
        }

        public string Get(string key) => Preferences.Get(key, string.Empty);

        public void Set(string key, string value) => Preferences.Set(key, value);
    }
}