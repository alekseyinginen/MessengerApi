using System;
using System.Collections.Generic;
using System.Text;

namespace MessengerApi.TcpServer.Models
{
    class EventDetails
    {
        public string MessageText { get; set; }

        public DateTime PublishTime { get; set; }
    }
}
