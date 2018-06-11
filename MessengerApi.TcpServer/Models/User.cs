using System;
using System.Collections.Generic;
using System.Text;

namespace MessengerApi.TcpServer.Models
{
    public class User
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string PictureURL { get; set; }

        public List<string> GroupIds { get; set; }
    }
}
