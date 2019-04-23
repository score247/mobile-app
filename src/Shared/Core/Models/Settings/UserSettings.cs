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

        public string SportId { get; private set; }

        public string Language { get; private set; }

        public string TimeZone { get; private set; }

        public override string ToString() => $"{SportId}-{Language}-{TimeZone}";
    }
}