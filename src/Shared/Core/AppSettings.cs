namespace LiveScore.Core
{
    using Enumerations;
    using Xamarin.Essentials;

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

        string ApiEndpoint { get; set; }

        string HubEndpoint { get; set; }

        string LoggingDns { get; set; }

        void Start();

        void Set(string key, string value);

        string Get(string key);
    }

    public class Settings : ISettings
    {
#if DEBUG
        public static string ApiEndPoint => "https://score247-api1.nexdev.net/dev/api";

        public static string SignalRHubEndPoint => "https://score247-api2.nexdev.net/dev/hubs/";
#elif TEST
        public static string ApiEndPoint => "https://score247-api1.nexdev.net/test/api/";

        public static string SignalRHubEndPoint => "https://score247-api2.nexdev.net/test/hubs/";
#elif AUTOTEST
        public static string ApiEndPoint => "https://api.nexdev.net/V4/api/";

        public static string SignalRHubEndPoint => "https://api.nexdev.net/V4/hubs/";
#else

        public static string ApiEndPoint => "https://score247-api1.nexdev.net/main/api/";

        public static string SignalRHubEndPoint => "https://score247-api2.nexdev.net/main/hubs/";

#endif

        public static string SentryDsn => "https://a75e3e7b51ea4de8baa2c27b67bbede3@sentry.nexdev.net/34";

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

        public string LoggingDns
        {
            get => Preferences.Get(nameof(LoggingDns), string.Empty);
            set => Preferences.Set(nameof(LoggingDns), value);
        }

        public string Get(string key) => Preferences.Get(key, string.Empty);

        public void Set(string key, string value) => Preferences.Set(key, value);

        public void Start()
        {
            ApiEndpoint = ApiEndPoint;
            HubEndpoint = SignalRHubEndPoint;
            LoggingDns = SentryDsn;
        }
    }
}