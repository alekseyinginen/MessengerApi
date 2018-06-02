using System;
using System.Collections.Generic;
using System.Text;

namespace MessengerApi.DAL.Entities
{
    public class GroupUser
    {
        public string Id { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public string ApplicationUserId { get; set; }

        public Group Group { get; set; }

        public string GroupId { get; set; }
    }
}
