using MessengerApi.BLL.Dto;
using MessengerApi.BLL.Infrastucture;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MessengerApi.BLL.Interfaces
{
    public interface IUserService
    {
        List<UserDto> SearchForUsers(string username);

        Task<UserDto> GetUserById(string id);

        Task<string> GetUserId(string username);

        Task<OperationDetails> Create(UserDto userDto);

        Task<OperationDetails> Delete(UserDto userDto);

        Task<OperationDetails> Update(UserDto userDto);

        Task AddUserToRole(string userId, string roleName);

        Task AddUserToRoles(string userId, List<string> roleNames);

        Task<bool> UserInRole(string username, string roleName);

        Task SeedDatabse();
    }
}
