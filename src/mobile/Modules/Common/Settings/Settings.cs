namespace Common.Settings
{
    using System.Collections.Generic;
    using Common.Contants;
    using Common.Extensions;
    using Plugin.Settings;
    using Plugin.Settings.Abstractions;

    public static class Settings
    {
        private static ISettings AppSettings => CrossSettings.Current;

        public static string ApiEndPoint => "https://api.sportradar.us";

        public static IDictionary<string, string> ApiKeyMapper => new Dictionary<string, string>
        {
            { "eu", "vequ6wxdqyt7eg8qzh26dm5u" },
            { "as", "2hr7x43y4st2g7a5j3yfa9yt" },
            { "intl", "33sv7bm2f7uq286wyhe2ejcp" },
            { "other", "cwp8y9e84bnsnxst7vah7xcn" }
        };

        public static string[] LeagueGroups => new string[] { "eu", "as", "intl", "other" };

        public static IDictionary<string, string> LanguageMapper => new Dictionary<string, string>
        {
            {"en-US", "en" }
        };

        public static IDictionary<string, string> SportNameMapper => new Dictionary<string, string>
        {
            {"Soccer", "soccer" }
        };

        public static string CurrentSportName
        {
            get => AppSettings.GetValueOrDefault(nameof(CurrentSportName), SportType.Soccer.GetDescription());
            set => AppSettings.AddOrUpdateValue(nameof(CurrentSportName), value);
        }

        public static int CurrentSportId
        {
            get => AppSettings.GetValueOrDefault(nameof(CurrentSportId), (int)SportType.Soccer);
            set => AppSettings.AddOrUpdateValue(nameof(CurrentSportId), value);
        }

        public static string CurrentLanguage
        {
            get => AppSettings.GetValueOrDefault(nameof(CurrentLanguage), "en-US");
            set => AppSettings.AddOrUpdateValue(nameof(CurrentLanguage), value);
        }
    }
}