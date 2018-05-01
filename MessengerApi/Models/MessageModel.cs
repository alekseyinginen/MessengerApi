using System;

namespace MessengerApi.Models
{
    public class MessageModel
    {
        public Guid Id { get; set; }

        public string MessageText { get; set; }

        public DateTime PublishTime { get; set; }

        public string SenderUsername { get; set; }
    }
}
