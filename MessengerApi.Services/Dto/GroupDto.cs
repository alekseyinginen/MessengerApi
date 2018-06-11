using MessengerApi.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessengerApi.BLL.Dto
{
    public class GroupDto
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string ApplicationUserId { get; set; }
    }
}
