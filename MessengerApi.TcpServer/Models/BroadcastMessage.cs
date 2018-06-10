using System;
using System.Collections.Generic;
using System.Text;

namespace MessengerApi.TcpServer.Models
{
    public class BroadcastMessage
    {
        public Guid Id { get; set; }

        public string MessageText { get; set; }

        public DateTime PublishTime { get; set; }

        public string GroupId { get; set; }

        public string SenderUsername { get; set; }
    }
}
