namespace Common.Settings
{
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
    }
}