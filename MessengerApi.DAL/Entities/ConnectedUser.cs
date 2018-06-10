using System;
using System.Collections.Generic;
using System.Text;

namespace MessengerApi.DAL.Entities
{
    public class ConnectedUser
    {
        public string Id { get; set; }

        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public string Connection { get; set; }
    }
}
