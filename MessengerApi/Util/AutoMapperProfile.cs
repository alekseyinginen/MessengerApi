using AutoMapper;
using MessengerApi.BLL.Dto;
using MessengerApi.DAL.Entities;
using MessengerApi.Models;
using MessengerApi.TcpServer.Models;

namespace MessengerApi.Util
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() {
            CreateMapsForUser();
            CreateMapsForMessage();
            CreateMapsForGroups();
        }

        private void CreateMapsForUser()
        {
            CreateMap<UserDto, LoginModel>()
                .ReverseMap();

            CreateMap<RegisterModel, UserDto>()
                .ReverseMap();

            CreateMap<UserModel, UserDto>()
                .ReverseMap();

            CreateMap<UserDto, UserModel>()
                .ReverseMap();

            CreateMap<ClientProfile, UserDto>()
                .ReverseMap();

            CreateMap<UserDto, ClientProfile>()
                .ReverseMap();
        }

        private void CreateMapsForMessage()
        {
            CreateMap<Message, MessageDto>()
                .ReverseMap();

            CreateMap<MessageDto, Message>()
                .ReverseMap();

            CreateMap<MessageModel, MessageDto>()
                .ReverseMap();

            CreateMap<MessageDto, MessageModel>()
                .ReverseMap();

            CreateMap<BroadcastMessage, MessageDto>()
                .ReverseMap();

            CreateMap<MessageDto, BroadcastMessage>()
                .ReverseMap();
        }

        private void CreateMapsForGroups()
        {
            CreateMap<Group, GroupDto>()
                .ForMember(x => x.Username, y => y.MapFrom(z => z.ApplicationUser.UserName))
                .ReverseMap();

            CreateMap<GroupUser, GroupUserDto>()
                .ForMember(x => x.Username, y => y.MapFrom(z => z.ApplicationUser.UserName))
                .ReverseMap();

            CreateMap<GroupDto, GroupModel>()
                .ReverseMap();
        }
    }
}
