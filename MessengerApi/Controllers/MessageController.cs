using AutoMapper;
using MessengerApi.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using MessengerApi.Models;
using MessengerApi.BLL.Dto;
using System.ComponentModel.DataAnnotations;

namespace MessengerApi.Controllers
{
    public class MessageController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IMessageService _messageService;
        private readonly IUserService _userService;

        public MessageController(
            IMapper mapper, 
            IMessageService messageService,
            IUserService userService)
        {
            _mapper = mapper;
            _messageService = messageService;
            _userService = userService;
        }

        [HttpGet]
        [Route("api/messages/all")]
        public async Task<IActionResult> GetAllMessages()
        {
            List<MessageDto> messageDtos = _messageService.GetAll();
            List<MessageModel> messages = await GetMessages(messageDtos);
            return Ok(messages);
        }

        [HttpGet]
        [Authorize]
        [Route("api/range-messages/{page}/{itemsPerPage}")]
        public async Task<IActionResult> GetRangeOfMessages([Required]int page, [Required]int itemsPerPage)
        {
            List<MessageDto> messageDtos = _messageService.GetRange(page, itemsPerPage);
            List<MessageModel> messages = await GetMessages(messageDtos);
            return Ok(messages);
        }

        [HttpGet]
        [Authorize]
        [Route("api/user-messages/{username}")]
        public async Task<IActionResult> GetUserMessages([Required]string username) {
            string userId = await _userService.GetUserId(username);
            List<MessageDto> messageDtos = _messageService.GetAllUserMessages(userId);
            List<MessageModel> messages = await GetMessages(messageDtos);
            return Ok(messages);
        }

        private async Task<List<MessageModel>> GetMessages(List<MessageDto> messageDtos) 
        {
            List<MessageModel> messages = new List<MessageModel>();
            foreach (var messageDto in messageDtos) 
            {
                MessageModel message = await GetFullInfoMessageModel(messageDto);
                messages.Add(message);
            }
            return messages;
        }

        private async Task<MessageModel> GetFullInfoMessageModel(MessageDto messageDto)
        {
            MessageModel message = _mapper.Map<MessageDto, MessageModel>(messageDto);
            message.SenderUsername = await GetMessageSenderUsername(messageDto.ApplicationUserId);
            return message;
        }

        private async Task<string> GetMessageSenderUsername(string userId)
        {
            return (await _userService.GetUserById(userId)).Username;
        }
    }
}
