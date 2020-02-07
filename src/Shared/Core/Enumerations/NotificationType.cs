namespace LiveScore.Core.Enumerations
{
    public class NotificationType : Enumeration
    {
        public static readonly NotificationType Match = new NotificationType(1, "match");

        public NotificationType()
        {
        }

        public NotificationType(byte value, string displayName)
            : base(value, displayName)
        {
        }

        public bool IsMatchType() => this == Match;
    }
}