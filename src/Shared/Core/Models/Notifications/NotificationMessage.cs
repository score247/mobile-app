using System;
using LiveScore.Core.Enumerations;

namespace LiveScore.Core.Models.Notifications
{
    public class NotificationMessage
    {
        public NotificationMessage(string sportId, string id, string type)
        {
            SportId = sportId;
            Id = id;
            Type = Enumeration.FromValue<NotificationType>(Convert.ToByte(type));
        }

        public string SportId { get; }

        public string Id { get; }

        public NotificationType Type { get; }
    }
}