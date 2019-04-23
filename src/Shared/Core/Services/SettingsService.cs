namespace LiveScore.Core.Services
{
    using System;   
    using LiveScore.Common.Services;
    using LiveScore.Core.Constants;
    using LiveScore.Core.Models.Settings;

    public interface ISettingsService
    {
        string CurrentLanguage { get; set; }

        SportType CurrentSport { get; set; }

        TimeZoneInfo CurrentTimeZone { get; set; }
       
        UserSettings UserSettings { get; }
    }

    public class SettingsService : ISettingsService
    {
        private readonly ICacheService cacheService;

        public SettingsService(ICacheService cacheService) 
        {
            this.cacheService = cacheService;
        }

        public SportType CurrentSport
        {
            get => cacheService.GetValueOrDefault(nameof(CurrentSport), SportType.Soccer);
            set => cacheService.AddOrUpdateValue(nameof(CurrentSport), value);
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

        public UserSettings UserSettings => new UserSettings((int)CurrentSport, CurrentLanguage, CurrentTimeZone.BaseUtcOffset.ToString());
    }
}