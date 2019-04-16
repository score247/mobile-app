namespace LiveScore.Core.Services
{
    using System;
    using System.Reactive.Linq;
    using Akavache;
    using LiveScore.Common.Extensions;
    using LiveScore.Core.Constants;

    public interface ISettingsService
    {
        string CurrentSportName { get; set; }

        int CurrentSportId { get; set; }

        string CurrentLanguage { get; set; }

        TimeZoneInfo TimeZone { get; set; }

        T GetValueOrDefault<T>(string key, T defaultValue);

        void AddOrUpdateValue<T>(string key, T value);
    }

    public class SettingsService : ISettingsService
    {
        public static string LocalEndPoint => "https://testing2.nexdev.net/v1/api/";

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
            get => GetValueOrDefault(nameof(CurrentLanguage), LanguageCode.En.ToString());
            set => AddOrUpdateValue(nameof(CurrentLanguage), value);
        }
        public TimeZoneInfo TimeZone { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void AddOrUpdateValue<T>(string key, T value)
            => BlobCache.UserAccount.InsertObject(key, value).Wait();

        public T GetValueOrDefault<T>(string key, T defaultValue)
            => BlobCache.UserAccount.GetOrCreateObject(key, () => defaultValue).Wait();
    }
}
