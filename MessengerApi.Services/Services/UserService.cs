using AutoMapper;
using MessengerApi.DAL.Entities;
using MessengerApi.DAL.Interfaces;
using MessengerApi.BLL.Dto;
using MessengerApi.BLL.Infrastucture;
using MessengerApi.BLL.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MessengerApi.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _database;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper) 
        {
            _database = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserDto> GetUserById(string id)
        {
            ClientProfile profile = await _database.ClientProfileRepository.GetById(id);
            return _mapper.Map<ClientProfile, UserDto>(profile);
        }

        public async Task<string> GetUserId(string username)
        {
            return (await _database.UserManager.FindByNameAsync(username)).Id;
        }

        public async Task AddUserToRole(string userId, string roleName) 
        {
            await _database.UserManager.AddToRoleAsync(await _database.UserManager.FindByIdAsync(userId), roleName);
        }

        public async Task AddUserToRoles(string userId, List<string> roleNames) 
        {
            foreach (var roleName in roleNames)
            {
                await AddUserToRole(userId, roleName);
            }
        }

        public async Task<bool> UserInRole(string userName, string roleName) 
        {
            ApplicationUser user = await _database.UserManager.FindByIdAsync(await GetUserId(userName));
            return await _database.UserManager.IsInRoleAsync(user, roleName);
        }

        public async Task<OperationDetails> Create(UserDto item) 
        {
            OperationDetails resultOperation;
            string userId = await RegistrationUser(item.Email, item.Username, item.Password);
            if (userId != string.Empty)
            {
                await AddUserToRoles(userId, item.Roles);
                await CreateProfileForUser(userId, item.Username, item.PictureURL);
                await _database.SaveAsync();
                resultOperation = new OperationDetails(true, "Registration succseded, client profile created", string.Empty);
            }
            else 
            {
                resultOperation = new OperationDetails(false, "Cannot complite registration for user", string.Empty);
            }
            return resultOperation;
        }

        public async Task<OperationDetails> Delete(UserDto item) 
        {
            OperationDetails resultOperation;
            ApplicationUser user = await _database.UserManager.FindByIdAsync(item.Id);
            var result = await _database.UserManager.DeleteAsync(user);
            if (result.Succeeded) 
            {
                await _database.ClientProfileRepository.Delete(item.Id);
                await _database.SaveAsync();
                resultOperation = new OperationDetails(true, string.Empty, string.Empty);
            }
            else {
                resultOperation = new OperationDetails(false, string.Empty, string.Empty);
            }
            return resultOperation;
        }

        public async Task<OperationDetails> Update(UserDto item)
        {
            OperationDetails resultOperation;
            ClientProfile clientProfile = _mapper.Map<UserDto, ClientProfile>(item);
            clientProfile = await _database.ClientProfileRepository.Update(clientProfile);
            if (clientProfile == null) 
            {
                resultOperation = new OperationDetails(false, string.Empty, string.Empty);
            }
            else 
            {
                resultOperation = new OperationDetails(true, string.Empty, string.Empty);
            }
            return resultOperation;
        }

        private async Task<string> RegistrationUser(string email, string username, string password) {
            string userId = string.Empty;
            ApplicationUser user = await _database.UserManager.FindByEmailAsync(email);
            if (user == null) 
            {
                user = new ApplicationUser { Email = email, UserName = username, EmailConfirmed = false };
                var result = await _database.UserManager.CreateAsync(user, password);
                userId = result.Succeeded ? user.Id : string.Empty;
            }
            return userId;
        }

        private async Task CreateProfileForUser(string userId, string username, string pictureURL) 
        {
            ClientProfile clientProfile = new ClientProfile
            {
                Id = userId,
                Username = username,
                PictureURL = pictureURL
            };
            await _database.ClientProfileRepository.Create(clientProfile);
        }

        public async Task SeedDatabse() 
        {
            await _database.RoleManager.CreateAsync(new IdentityRole { Name = "User" });
            await _database.RoleManager.CreateAsync(new IdentityRole { Name = "Admin" });
        }
    }
}
