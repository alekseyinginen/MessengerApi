using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace MessengerApi.DAL.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ClientProfile ClientProfile { get; set; }

        public virtual List<Message> Messages { get; set; }
    }
}
