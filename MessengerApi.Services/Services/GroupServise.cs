using AutoMapper;
using MessengerApi.DAL.Entities;
using MessengerApi.DAL.Interfaces;
using MessengerApi.BLL.Dto;
using MessengerApi.BLL.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace MessengerApi.BLL.Services
{
    public class GroupServise : IGroupService 
    {
        private readonly IUnitOfWork _database;
        private readonly IMapper _mapper;

        public GroupServise(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _database = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GroupDto> GetGroupById(string groupId)
        {
            var group = await _database.GroupRepository.GetById(groupId);

            if (group == null)
            {
                return null;
            }

            return await MapGroup(group);
        }

        public async Task<List<GroupDto>> GetUserGroups(string username)
        {
            var userGroups = _database.GroupRepository.Query().Where(x => x.GroupUsers.Select(y => y.ApplicationUser.UserName).Contains(username));

            var groups = new List<GroupDto>();

            foreach (var group in userGroups)
            {
                groups.Add(await MapGroup(group));
            }

            return groups;
        }

        public async Task<GroupDto> CreateGroup(GroupDto groupDto)
        {
            Group group = MapGroupDto(groupDto);

            if (group == null)
            {
                return null;
            }

            group = await _database.GroupRepository.Create(group);

            return await MapGroup(group);
        }

        private Group MapGroupDto(GroupDto groupDto)
        {
            Group group = _mapper.Map<Group>(groupDto);
            ClientProfile profile = _database.ClientProfileRepository.GetByName(groupDto.Username);

            if (profile != null)
            {
                group.ApplicationUserId = profile.Id;
                return group;
            }

            return null;
        }

        private async Task<GroupDto> MapGroup(Group group)
        {
            GroupDto groupDto = _mapper.Map<GroupDto>(group);

            groupDto.Username = (await _database.ClientProfileRepository.GetById(group.ApplicationUserId)).Username;

            return groupDto;
        } 
    }
}
