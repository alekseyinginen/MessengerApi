using System;
using System.Collections.Generic;
using System.Text;

namespace MessengerApi.BLL.Dto
{
    public class GroupUserDto
    {
        public string Id { get; set; }

        public string ApplicationUserId { get; set; }

        public string GroupId { get; set; }
    }
}
