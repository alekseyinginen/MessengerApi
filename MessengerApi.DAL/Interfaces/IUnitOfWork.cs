using MessengerApi.DAL.EF;
using MessengerApi.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace MessengerApi.DAL.Interfaces
{
    interface IUnitOfWork
    {
        ApplicationContext Context { get; }
        UserManager<ApplicationUser> UserManager { get; }
        SignInManager<ApplicationUser> SignInManager { get; }
        RoleManager<IdentityRole> RoleManager { get; }

        IClientProfileRepository ClientProfileRepository { get; }
        IMessageRepository MessageRepository { get; }

        Task SaveAsync();
    }
}
