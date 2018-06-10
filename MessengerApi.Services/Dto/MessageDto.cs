using System;
using System.Collections.Generic;
using System.Text;

namespace MessengerApi.BLL.Dto
{
    public class MessageDto
    {
        public Guid Id { get; set; }

        public string MessageText { get; set; }

        public DateTime PublishTime { get; set; }

        public string ApplicationUserId { get; set; }

        public string GroupId { get; set; }
    }
}
