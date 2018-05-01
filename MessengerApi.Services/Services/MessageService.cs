using AutoMapper;
using MessengerApi.DAL.Entities;
using MessengerApi.DAL.Interfaces;
using MessengerApi.BLL.Dto;
using MessengerApi.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessengerApi.BLL.Services
{
    public class MessageService : IMessageService 
    {
        private readonly IUnitOfWork _database;
        private readonly IMapper _mapper;

        public MessageService(IUnitOfWork unitOfWork, IMapper mapper) 
        {
            _database = unitOfWork;
            _mapper = mapper;
        }

        public IQueryable<Message> Query() 
        {
            return _database.MessageRepository.Query();
        }

        public async Task<MessageDto> GetById(Guid id) 
        {
            Message message = await _database.MessageRepository.GetById(id);
            return _mapper.Map<Message, MessageDto>(message);
        }

        public List<MessageDto> GetAll() 
        {
            IEnumerable<Message> messages = _database.MessageRepository.GetAll().OrderBy(m => m.PublishTime);
            return _mapper.Map<List<Message>, List<MessageDto>>(messages.ToList());
        }

        public List<MessageDto> GetRange(int page, int itemsPerPage) 
        {
            IEnumerable<Message> messages = _database.MessageRepository.GetRange(page, itemsPerPage).OrderBy(m => m.PublishTime);
            return _mapper.Map<List<Message>, List<MessageDto>>(messages.ToList());
        }

        public async Task<MessageDto> Create(MessageDto item) 
        {
            Message message = _mapper.Map<MessageDto, Message>(item);
            message = await _database.MessageRepository.Create(message);
            return _mapper.Map<Message, MessageDto>(message);
        }

        public async Task<MessageDto> Update(MessageDto item) 
        {
            Message message = _mapper.Map<MessageDto, Message>(item);
            message = await _database.MessageRepository.Update(message);
            return _mapper.Map<Message, MessageDto>(message);
        }

        public async Task<MessageDto> Delete(Guid id)
        {
            Message message = await _database.MessageRepository.Delete(id);
            return _mapper.Map<Message, MessageDto>(message);
        }

        public List<MessageDto> GetAllUserMessages(string userId)
        {
            IEnumerable<Message> messages = _database.MessageRepository.GetAllUsersMessages(userId);
            return _mapper.Map<List<Message>, List<MessageDto>>(messages.ToList());
        }

        public List<MessageDto> GetRangeOfUsersMessages(string userId, int page, int itemsPerPage) 
        {
            IEnumerable<Message> messages = _database.MessageRepository.GetRangeOfUsersMessages(userId, page, itemsPerPage);
            return _mapper.Map<List<Message>, List<MessageDto>>(messages.ToList());
        }
    }
}
