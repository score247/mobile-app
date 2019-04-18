namespace LiveScore.Core.Models.Settings
{
    public class UserSettings
    {
        public UserSettings(int sportId, string language, string timeZone) 
        {
            SportId = sportId;
            Language = language;
            TimeZone = timeZone;
        }

        public int SportId { get; set; }

        public string Language { get; set; }

        public string TimeZone { get; set; }

        public override string ToString() => $"{SportId}-{Language}-{TimeZone}";
    }
}
