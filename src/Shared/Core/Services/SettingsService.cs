﻿namespace LiveScore.Core.Services
{
    using System;
    using LiveScore.Common.Services;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Settings;

    public interface ISettingsService
    {
        string CurrentLanguage { get; set; }

        Language Language { get; }

        SportType CurrentSportType { get; set; }

        TimeZoneInfo CurrentTimeZone { get; set; }

        UserSettings UserSettings { get; }
    }

    public class SettingsService : ISettingsService
    {
        private readonly ICachingService cacheService;

        public SettingsService(ICachingService cacheService)
        {
            this.cacheService = cacheService;

            UserSettings = new UserSettings(CurrentSportType.DisplayName, CurrentLanguage, CurrentTimeZone.BaseUtcOffset.ToString());

            Language = Enumeration.FromDisplayName<Language>(CurrentLanguage);
        }

        protected SettingsService(ICachingService cacheService, UserSettings userSettings)
            : this(cacheService) => UserSettings = userSettings;

        public SportType CurrentSportType
        {
            get => cacheService.GetValueOrDefaultFromUserAccount(nameof(CurrentSportType), SportType.Soccer);
            set => cacheService.AddOrUpdateValueToUserAccount(nameof(CurrentSportType), value);
        }

        public string CurrentLanguage
        {
            get => cacheService.GetValueOrDefaultFromUserAccount(nameof(CurrentLanguage), Language.English.DisplayName);
            set => cacheService.AddOrUpdateValueToUserAccount(nameof(CurrentLanguage), value);
        }

        public TimeZoneInfo CurrentTimeZone
        {
            get => cacheService.GetValueOrDefaultFromUserAccount(nameof(CurrentTimeZone), TimeZoneInfo.Local);
            set => cacheService.AddOrUpdateValueToUserAccount(nameof(CurrentTimeZone), value);
        }

        public UserSettings UserSettings { get; }
        public Language Language { get; }
    }
}