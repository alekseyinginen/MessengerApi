using MessengerApi.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MessengerApi.BLL.Interfaces
{
    public interface IConnectedUsersService
    {
        ConnectedUser GetByUserId(string userId);

        ConnectedUser GetByConnectionId(string connection);

        Task Connect(string userId, string connection);

        Task UpdateConnection(ConnectedUser user, string connection);

        Task Disconnect(string userId);

        Task<List<string>> GetRelatedConnectionIds(string userId);
    }
}
