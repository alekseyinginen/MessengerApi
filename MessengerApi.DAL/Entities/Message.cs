using System;
using System.ComponentModel.DataAnnotations;

namespace MessengerApi.DAL.Entities
{
    public class Message
    {
        public Message()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        [DataType("nvarchar(100)")]
        public string MessageText { get; set; }

        public DateTime PublishTime { get; set; }

        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}
