using System;
using LiveScore.Core.Enumerations;

namespace LiveScore.Core.Models.Notifications
{
    public class NotificationMessage
    {
        public const string NotificationMessageCachedKey = "NotificationMessage";

        public NotificationMessage(string sportId, string id, string type)
        {
            SportType = Enumeration.FromValue<SportType>(Convert.ToByte(sportId));
            Id = id;
            Type = Enumeration.FromValue<NotificationType>(Convert.ToByte(type));
        }

        public SportType SportType { get; }

        public string Id { get; }

        public NotificationType Type { get; }
    }
}