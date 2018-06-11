using System.Collections.Generic;

namespace MessengerApi.Models
{
    public class UserModel
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string PictureURL { get; set; }

        public List<string> Roles { get; set; }
    }
}
