using MessengerApi.DAL.EF;
using MessengerApi.DAL.Entities;
using MessengerApi.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MessengerApi.DAL.Repositories
{
    public class GroupUserRepository : Repository<Group, string>//, IGroupUserRepository
    {
        public GroupUserRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
