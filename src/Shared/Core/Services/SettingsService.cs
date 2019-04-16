namespace LiveScore.Core.Services
{
    using System.Collections.Generic;
    using System.Reactive.Linq;
    using Akavache;
    using LiveScore.Common.Extensions;
    using LiveScore.Core.Constants;

    public interface ISettingsService
    {
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
        public static string LocalEndPoint => "https://testing2.nexdev.net/v1/api/";

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
