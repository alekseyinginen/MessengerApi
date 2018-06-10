using MessengerApi.BLL.Interfaces;
using MessengerApi.DAL.Entities;
using MessengerApi.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerApi.BLL.Services
{
    public class ConnectedUsersService : IConnectedUsersService
    {
        private readonly IUnitOfWork _database;
        
        public ConnectedUsersService(IUnitOfWork unitOfWork)
        {
            _database = unitOfWork;
        }

        public async Task Connect(string userId, string connection)
        {
            ConnectedUser user = new ConnectedUser
            {
                ApplicationUserId = userId,
                Connection = connection
            };

            await _database.Context.ConnectedUsers.AddAsync(user);

            await _database.SaveAsync();
        }

        public async Task Disconnect(string userId)
        {
            ConnectedUser user = GetByUserId(userId);

            _database.Context.ConnectedUsers.Remove(user);

            await _database.SaveAsync();
        }

        public ConnectedUser GetByUserId(string userId)
        {
            return _database.Context.ConnectedUsers.FirstOrDefault(x => x.ApplicationUserId == userId);
        }
    }
}
