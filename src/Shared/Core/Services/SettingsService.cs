namespace LiveScore.Core.Services
{
    using System;
    using System.Reactive.Linq;
    using Akavache;
    using LiveScore.Common.Extensions;
    using LiveScore.Core.Constants;
    using LiveScore.Core.Models.Settings;

    public interface ISettingsService
    {
        string CurrentLanguage { get; set; }

        SportType CurrentSport { get; set; }

        TimeZoneInfo CurrentTimeZone { get; set; }

        T GetValueOrDefault<T>(string key, T defaultValue);

        void AddOrUpdateValue<T>(string key, T value);

        UserSettings UserSettings { get; }
    }

    public class SettingsService : ISettingsService
    {
        public static string LocalEndPoint => "https://testing2.nexdev.net/main/api/";

        public SportType CurrentSport
        {
            get => GetValueOrDefault(nameof(CurrentSport), SportType.Soccer);
            set => AddOrUpdateValue(nameof(CurrentSport), value);
        }

        public string CurrentLanguage
        {
            get => GetValueOrDefault(nameof(CurrentLanguage), LanguageCode.En.ToString());
            set => AddOrUpdateValue(nameof(CurrentLanguage), value);
        }

        public UserSettings UserSettings => new UserSettings(CurrentSportId, CurrentLanguage, CurrentTimeZone.BaseUtcOffset.ToString());

        public TimeZoneInfo CurrentTimeZone
        {
            get => GetValueOrDefault(nameof(CurrentTimeZone), TimeZoneInfo.FindSystemTimeZoneById("Asia/Bangkok"));
            set => AddOrUpdateValue(nameof(CurrentLanguage), value);
        }

        public void AddOrUpdateValue<T>(string key, T value)
            => BlobCache.UserAccount.InsertObject(key, value).Wait();

        public T GetValueOrDefault<T>(string key, T defaultValue)
            => BlobCache.UserAccount.GetOrCreateObject(key, () => defaultValue).Wait();
    }
}