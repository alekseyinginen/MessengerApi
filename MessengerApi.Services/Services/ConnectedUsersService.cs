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
        private readonly IGroupService groupService;
        private readonly IGroupUserService groupUserService;

        public ConnectedUsersService(IUnitOfWork unitOfWork, IGroupService groupService, IGroupUserService groupUserService)
        {
            _database = unitOfWork;
            this.groupService = groupService;
            this.groupUserService = groupUserService;
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

        public async Task UpdateConnection(ConnectedUser user, string connection)
        {
            user.Connection = connection;

            _database.Context.ConnectedUsers.Update(user);

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

        public ConnectedUser GetByConnectionId(string connection)
        {
            return _database.Context.ConnectedUsers.FirstOrDefault(x => x.Connection == connection);
        }

        public async Task<List<string>> GetRelatedConnectionIds(string userId)
        {
            var user = _database.Context.ClientProfiles.FirstOrDefault(x => x.Id == userId);

            if (user != null)
            {
                var relatedUserIds = (await groupService.GetUserGroups(user.Username)).Select(x => x.ApplicationUserId).ToList();

                var relatedConnectionIds = _database.Context.ConnectedUsers
                                                                .Where(x => relatedUserIds.Contains(x.ApplicationUserId))
                                                                .Select(y => y.Connection)
                                                                .ToList();

                return relatedConnectionIds;
            }
            return new List<string>();
        }
    }
}
