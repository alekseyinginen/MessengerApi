using MessengerApi.BLL.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MessengerApi.BLL.Interfaces
{
    public interface IGroupService
    {
        Task<GroupDto> GetGroupById(string groupId);

        Task<List<GroupDto>> GetUserGroups(string username);

        Task<GroupDto> CreateGroup(GroupDto group);
    }
}
