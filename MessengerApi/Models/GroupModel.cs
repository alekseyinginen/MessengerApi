using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessengerApi.Models
{
    public class GroupModel
    {
        public string GroupId { get; set; }

        public List<string> Usernames { get; set; }
    }
}
