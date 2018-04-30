using MessengerApi.DAL.Entities;
using System;
using System.Collections.Generic;

namespace MessengerApi.DAL.Interfaces
{
    public interface IMessageRepository : IRepository<Message, Guid>
    {
        IEnumerable<Message> GetAllUsersMessages(string userId);

        IEnumerable<Message> GetRangeOfUsersMessages(string userId, int page, int itemsPerPage);
    }
}
