namespace Common.Services
{
    using System.Collections.Generic;
    using System.Reactive.Linq;
    using Akavache;
    using Common.Contants;
    using Common.Extensions;

    public interface ISettingsService
    {
        string ApiEndPoint { get; }

        string[] LeagueGroups { get; }

        IDictionary<string, string> ApiKeyMapper { get; }

        IDictionary<string, string> LanguageMapper { get; }

        IDictionary<string, string> SportNameMapper { get; }

        string CurrentSportName { get; set; }

        int CurrentSportId { get; set; }

        string CurrentLanguage { get; set; }

        T GetValueOrDefault<T>(string key, T defaultValue);

        void AddOrUpdateValue<T>(string key, T value);
    }

    public class SettingsService : ISettingsService
    {
        public string ApiEndPoint => "https://api.sportradar.us";

        public IDictionary<string, string> ApiKeyMapper => new Dictionary<string, string>
        {
            { "am", "a4tmwcm5rj73kd6ctgbvazzb" },
            { "eu", "udt47krvjf2uugrx94mhf5z4" },
            { "as", "d5vvgr7c3vg77kksd94rp8t2" },
            { "intl", "9bs6t3aa8xjhtq5ytf2n8q5q" },
            { "other", "yfvcbfg8eb6r8jv5z9jsh6bb" }
        };

        public string[] LeagueGroups => new[] { "am", "eu", "as", "intl", "other" };

        public IDictionary<string, string> LanguageMapper => new Dictionary<string, string>
        {
            { "en-US", "en" }
        };

        public IDictionary<string, string> SportNameMapper => new Dictionary<string, string>
        {
            { "Soccer", "soccer" }
        };

        public string CurrentSportName
        {
            get => GetValueOrDefault(nameof(CurrentSportName), SportType.Soccer.GetDescription());
            set => AddOrUpdateValue(nameof(CurrentSportName), value);
        }

        public int CurrentSportId
        {
            get => GetValueOrDefault(nameof(CurrentSportId), (int)SportType.Soccer);
            set => AddOrUpdateValue(nameof(CurrentSportId), value);
        }

        public string CurrentLanguage
        {
            get => GetValueOrDefault(nameof(CurrentLanguage), "en-US");
            set => AddOrUpdateValue(nameof(CurrentLanguage), value);
        }

        public void AddOrUpdateValue<T>(string key, T value)
            => BlobCache.UserAccount.InsertObject(key, value).Wait();

        public T GetValueOrDefault<T>(string key, T defaultValue)
            => BlobCache.UserAccount.GetOrCreateObject(key, () => defaultValue).Wait();
    }
}
