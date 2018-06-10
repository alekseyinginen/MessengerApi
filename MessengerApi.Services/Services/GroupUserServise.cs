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
    public class GroupUserServise : IGroupUserService
    {
        private readonly IUnitOfWork _database;
        private readonly IMapper _mapper;

        public GroupUserServise(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _database = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<GroupUserDto>> GetGroupUsers(string groupId)
        {
            var groupUsers = _database.GroupUserRepository.Query().Where(x => x.GroupId == groupId).ToList();

            return await MapGroupUsers(groupUsers);
        }

        public bool CheckForUserExistingInGroup(string groupId, string username)
        {
            return _database.GroupUserRepository.Query().Any(x => x.GroupId == groupId && x.ApplicationUser.UserName == username);
        }

        public async Task<List<GroupUserDto>> GetAllUsersFromGroup(string groupId)
        {
            var groupUsers = _database.GroupUserRepository.Query().Where(x => x.GroupId == groupId).ToList();

            return await MapGroupUsers(groupUsers);
        }

        public async Task<GroupUserDto> AddUserToGroup(GroupUserDto groupUserDto)
        {
            GroupUser groupUser = MapGroupUserDto(groupUserDto);

            if (groupUser == null)
            {
                return null;
            }

            groupUser = await _database.GroupUserRepository.Create(groupUser);

            return await MapGroupUser(groupUser);
        }

        private GroupUser MapGroupUserDto(GroupUserDto groupUserDto)
        {
            GroupUser groupUser = _mapper.Map<GroupUser>(groupUserDto);
            ClientProfile profile = _database.ClientProfileRepository.GetByName(groupUserDto.Username);

            if (profile != null)
            {
                groupUser.ApplicationUserId = profile.Id;
                return groupUser;
            }

            return null;
        }

        private async Task<List<GroupUserDto>> MapGroupUsers(List<GroupUser> groupUsers)
        {
            var groupUserDtos = new List<GroupUserDto>();

            foreach (var groupUser in groupUsers)
            {
                groupUserDtos.Add(await MapGroupUser(groupUser));
            }

            return groupUserDtos;
        }

        private async Task<GroupUserDto> MapGroupUser(GroupUser groupUser)
        {
            var groupUserDto = _mapper.Map<GroupUserDto>(groupUser);

            groupUserDto.Username = (await _database.ClientProfileRepository.GetById(groupUser.ApplicationUserId)).Username;

            return groupUserDto;
        }
    }
}
