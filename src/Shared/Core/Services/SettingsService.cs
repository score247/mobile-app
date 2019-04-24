namespace LiveScore.Core.Services
{
    using System;
    using LiveScore.Common.Services;
    using LiveScore.Core.Constants;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Settings;

    public interface ISettingsService
    {
        string CurrentLanguage { get; set; }

        SportTypes CurrentSportType { get; set; }

        TimeZoneInfo CurrentTimeZone { get; set; }

        UserSettings UserSettings { get; }
    }

    public class SettingsService : ISettingsService
    {
        private readonly ILocalStorage cacheService;

        public SettingsService(ILocalStorage cacheService)
        {
            this.cacheService = cacheService;
        }

        public SportTypes CurrentSportType
        {
            get => cacheService.GetValueOrDefault(nameof(CurrentSportType), SportTypes.Soccer);
            set => cacheService.AddOrUpdateValue(nameof(CurrentSportType), value);
        }

        public string CurrentLanguage
        {
            get => cacheService.GetValueOrDefault(nameof(CurrentLanguage), LanguageCode.En.ToString());
            set => cacheService.AddOrUpdateValue(nameof(CurrentLanguage), value);
        }

        public TimeZoneInfo CurrentTimeZone
        {
            get => cacheService.GetValueOrDefault(nameof(CurrentTimeZone), TimeZoneInfo.Local);
            set => cacheService.AddOrUpdateValue(nameof(CurrentTimeZone), value);
        }

        public UserSettings UserSettings => new UserSettings(CurrentSportType.Value, CurrentLanguage, CurrentTimeZone.BaseUtcOffset.ToString());
    }
}