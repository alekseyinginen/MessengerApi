using MessengerApi.DAL.EF;
using MessengerApi.DAL.Entities;
using MessengerApi.DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace MessengerApi.DAL.Repositories
{
    class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IClientProfileRepository _clientProfileRepository;
        private readonly IMessageRepository _messageRepository;

        public UnitOfWork(
                ApplicationContext context,
                UserManager<ApplicationUser> userManager,
                RoleManager<IdentityRole> roleManager,
                SignInManager<ApplicationUser> signInManager
            ) 
            {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;

            _clientProfileRepository = new ClientProfileRepository(_context);
            _messageRepository = new MessageRepository(_context);
        }

        public ApplicationContext Context => _context;

        public UserManager<ApplicationUser> UserManager => _userManager;

        public SignInManager<ApplicationUser> SignInManager => _signInManager;

        public RoleManager<IdentityRole> RoleManager => _roleManager;

        public IClientProfileRepository ClientProfileRepository => _clientProfileRepository;

        public IMessageRepository MessageRepository => _messageRepository;

        public async Task SaveAsync() {
            await _context.SaveChangesAsync();
        }
    }
}
