using System.Collections.Generic;

namespace MessengerApi.BLL.Dto
{
    public class UserDto
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Username { get; set; }

        public string PictureURL { get; set; }
        
        public List<string> Roles { get; set; }
    }
}
