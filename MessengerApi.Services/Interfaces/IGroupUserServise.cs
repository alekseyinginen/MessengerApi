using MessengerApi.BLL.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MessengerApi.BLL.Interfaces
{
    public interface IGroupUserService
    {
        Task<List<GroupUserDto>> GetGroupUsers(string groupId);

        bool CheckForUserExistingInGroup(string groupId, string username);

        Task<List<GroupUserDto>> GetAllUsersFromGroup(string groupId);

        Task<GroupUserDto> AddUserToGroup(GroupUserDto groupUser);
    }
}
