namespace Common.Settings
{
    using System;
    using System.Collections.Generic;
    using Plugin.Settings;
    using Plugin.Settings.Abstractions;

    public static class Settings
    {
        private static ISettings AppSettings => CrossSettings.Current;

        public static string CurrentSportName
        {
            get => AppSettings.GetValueOrDefault(nameof(CurrentSportName), "Soccer");
            set => AppSettings.AddOrUpdateValue(nameof(CurrentSportName), value);
        }

        public static int CurrentSportId
        {
            get => AppSettings.GetValueOrDefault(nameof(CurrentSportId), 1);
            set => AppSettings.AddOrUpdateValue(nameof(CurrentSportId), value);
        }

        public static string CurrentLanguage
        {
            get => AppSettings.GetValueOrDefault(nameof(CurrentLanguage), "en-US");
            set => AppSettings.AddOrUpdateValue(nameof(CurrentLanguage), value);
        }

        public static string BaseSportRadarEndPoint
        {
            get => AppSettings.GetValueOrDefault(nameof(BaseSportRadarEndPoint), "https://api.sportradar.us");
            set => AppSettings.AddOrUpdateValue(nameof(BaseSportRadarEndPoint), value);
        }

        public static string SportRadarApiKey
        {
            get => AppSettings.GetValueOrDefault(nameof(SportRadarApiKey), "s37g6cgqfabegn5mu8snw293");
            set => AppSettings.AddOrUpdateValue(nameof(SportRadarApiKey), value);
        }

        public static DateTime CurrentDate
        {
            get => AppSettings.GetValueOrDefault(nameof(CurrentDate), DateTime.Today);
            set => AppSettings.AddOrUpdateValue(nameof(CurrentDate), value);
        }

        public static IDictionary<string, string> ApiKeyMapper { get; } = new Dictionary<string, string>
        {
            { "eu", "s37g6cgqfabegn5mu8snw293" },
            { "as", "sryuchhgtvff7nb62wktdre4" },
            { "intl", "vrpveq5y64pvxpx4z7x5j24j" },
            { "other", "3py8dv68pmb4dypxaqesm9jq" }
        };

        public static string[] SportRadarLeagueGroup = new string[] { "eu", "as", "intl", "other" };


        public static IDictionary<string, string> LanguageMapper { get; } = new Dictionary<string, string>
        {
            {"en-US", "en" }
        };

        public static IDictionary<string, string> SportNameMapper { get; } = new Dictionary<string, string>
        {
            {"Soccer", "soccer" }
        };
    }
}