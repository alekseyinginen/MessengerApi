using MessengerApi.DAL.EF;
using MessengerApi.DAL.Entities;
using MessengerApi.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MessengerApi.DAL.Repositories
{
    public class GroupRepository : Repository<Group, string>//, IGroupUserRepository
    {
        public GroupRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
