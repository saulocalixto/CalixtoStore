using MediatR;
using System;

namespace CalixtoStore.Core.Messages.CommonMessages.Notifications
{
    public class DomainNotification : Message, INotification
    {
        public DateTime Timestamp { get; set; }
        public Guid DomainNotificationId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }

        public DomainNotification(string key, string value)
        {
            Key = key;
            Value = value;
            Timestamp = DateTime.Now;
            DomainNotificationId = Guid.NewGuid();
        }
    }
}