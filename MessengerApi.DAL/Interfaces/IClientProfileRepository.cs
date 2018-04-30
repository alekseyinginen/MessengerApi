using MessengerApi.DAL.Entities;

namespace MessengerApi.DAL.Interfaces
{
    public interface IClientProfileRepository : IRepository<ClientProfile, string> 
    {
        ClientProfile GetByName(string username);
    }
}
