namespace LiveScore.Core.Models.Settings
{
    public class UserSettings
    {
        public UserSettings(string sportId, string language, string timeZone)
        {
            SportId = sportId;
            Language = language;
            TimeZone = timeZone;
        }

        public string SportId { get; }

        public string Language { get; }

        public string TimeZone { get; }

        public override string ToString() => $"{SportId}-{Language}-{TimeZone}";
    }
}