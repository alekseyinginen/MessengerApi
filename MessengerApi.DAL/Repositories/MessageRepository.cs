using MessengerApi.DAL.EF;
using MessengerApi.DAL.Entities;
using MessengerApi.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MessengerApi.DAL.Repositories
{
    public class MessageRepository : Repository<Message, Guid>, IMessageRepository
    {
        public MessageRepository(ApplicationContext context) : base(context) 
        {
        }

        public IEnumerable<Message> GetAllUsersMessages(string userId) {
            return _context.Messages
                .Where(m => m.ApplicationUserId.Equals(userId))
                .OrderBy(m => m.PublishTime);
        }

        public IEnumerable<Message> GetRangeOfUsersMessages(string userId, int page, int itemsPerPage) {
            return _context.Messages
                .Where(m => m.ApplicationUserId.Equals(userId))
                .OrderBy(m => m.PublishTime)
                .Skip(page * itemsPerPage)
                .Take(itemsPerPage);
        }
    }
}
