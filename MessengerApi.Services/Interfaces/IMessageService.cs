using MessengerApi.BLL.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MessengerApi.BLL.Interfaces
{
    public interface IMessageService
    {
        Task<MessageDto> GetById(Guid id);

        List<MessageDto> GetAll();

        List<MessageDto> GetAllGroupMessages(string groupId);

        List<MessageDto> GetRange(int page, int itemsPerPage);

        Task<MessageDto> Create(MessageDto item);

        Task<MessageDto> Update(MessageDto item);

        Task<MessageDto> Delete(Guid id);

        List<MessageDto> GetAllUserMessages(string userId);

        List<MessageDto> GetRangeOfUsersMessages(string userId, int page, int itemsPerPage);
    }
}
