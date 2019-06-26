﻿namespace LiveScore.Core.Services
{
    using System;
    using LiveScore.Common.Services;
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
            get => cacheService.GetValueOrDefaultFromUserAccount(nameof(CurrentSportType), SportTypes.Soccer);
            set => cacheService.AddOrUpdateValueToUserAccount(nameof(CurrentSportType), value);
        }

        public string CurrentLanguage
        {
            get => cacheService.GetValueOrDefaultFromUserAccount(nameof(CurrentLanguage), Languages.English.Value);
            set => cacheService.AddOrUpdateValueToUserAccount(nameof(CurrentLanguage), value);
        }

        public TimeZoneInfo CurrentTimeZone
        {
            get => cacheService.GetValueOrDefaultFromUserAccount(nameof(CurrentTimeZone), TimeZoneInfo.Local);
            set => cacheService.AddOrUpdateValueToUserAccount(nameof(CurrentTimeZone), value);
        }

        public UserSettings UserSettings => new UserSettings(CurrentSportType.Value, CurrentLanguage, CurrentTimeZone.BaseUtcOffset.ToString());
    }
}