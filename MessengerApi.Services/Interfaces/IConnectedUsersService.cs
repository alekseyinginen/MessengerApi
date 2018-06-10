using MessengerApi.DAL.Entities;
using System.Threading.Tasks;

namespace MessengerApi.BLL.Interfaces
{
    public interface IConnectedUsersService
    {
        ConnectedUser GetByUserId(string userId);

        Task Connect(string userId, string connection);

        Task Disconnect(string userId);
    }
}
