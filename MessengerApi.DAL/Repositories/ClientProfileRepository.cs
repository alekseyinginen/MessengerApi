using MessengerApi.DAL.EF;
using MessengerApi.DAL.Entities;
using MessengerApi.DAL.Interfaces;
using System.Linq;

namespace MessengerApi.DAL.Repositories
{
    class ClientProfileRepository : Repository<ClientProfile, string>, IClientProfileRepository
    {
        public ClientProfileRepository(ApplicationContext context) : base(context) 
        {
        }

        public ClientProfile GetByName(string username) 
        {
            return _context.ClientProfiles.FirstOrDefault(cp => cp.Username.Equals(username));
        }
    }
}
