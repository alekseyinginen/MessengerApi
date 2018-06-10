using AutoMapper;
using MessengerApi.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using MessengerApi.Models;
using MessengerApi.BLL.Dto;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MessengerApi.Controllers
{
    public class MessageController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IMessageService _messageService;
        private readonly IUserService _userService;
        private readonly IGroupService _groupService;
        private readonly IGroupUserService _groupUserService;

        public MessageController(
            IMapper mapper, 
            IMessageService messageService,
            IUserService userService,
            IGroupService groupService,
            IGroupUserService groupUserService)
        {
            _mapper = mapper;
            _messageService = messageService;
            _userService = userService;
            _groupService = groupService;
            _groupUserService = groupUserService;
        }

        [HttpGet]
        [Route("api/group-messages/{id}")]
        [Authorize]
        public async Task<IActionResult> GetAllGroupMessages([Required]string id)
        {
            var group = await _groupService.GetGroupById(id);

            if (group != null && _groupUserService.CheckForUserExistingInGroup(id, User.Identity.Name))
            {
                var messages = _messageService.GetAllGroupMessages(id);

                return Ok(_mapper.Map<List<MessageModel>>(messages));
            }
            return BadRequest();

        }
        

        [HttpGet]
        [Route("api/messages/all")]
        public async Task<IActionResult> GetAllMessages()
        {
            List<MessageDto> messageDtos = _messageService.GetAll();
            List<MessageModel> messages = await GetMessageModelsFromListOfDto(messageDtos);
            return Ok(messages);
        }

        [HttpGet]
        [Authorize]
        [Route("api/range-messages/{page}/{itemsPerPage}")]
        public async Task<IActionResult> GetRangeOfMessages([Required]int page, [Required]int itemsPerPage)
        {
            List<MessageDto> messageDtos = _messageService.GetRange(page, itemsPerPage);
            List<MessageModel> messages = await GetMessageModelsFromListOfDto(messageDtos);
            return Ok(messages);
        }

        [HttpGet]
        [Authorize]
        [Route("api/user-messages/{username}")]
        public async Task<IActionResult> GetUserMessages([Required]string username) {
            string userId = await _userService.GetUserId(username);
            List<MessageDto> messageDtos = _messageService.GetAllUserMessages(userId);
            List<MessageModel> messages = await GetMessageModelsFromListOfDto(messageDtos);
            return Ok(messages);
        }

        [HttpPost]
        [Authorize]
        [Route("api/messages/add-message")]
        public async Task<IActionResult> AddMessage([Required][FromBody]MessageModel message)
        {
            if (ModelState.IsValid)
            {
                MessageDto messageDto = await GetFullInfoMessageDto(message);
                messageDto = await _messageService.Create(messageDto);
                return Ok(_mapper.Map<MessageDto, MessageModel>(messageDto));
            }
            return BadRequest(ModelState);
        }

        private async Task<List<MessageModel>> GetMessageModelsFromListOfDto(List<MessageDto> messageDtos) 
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

        private async Task<MessageDto> GetFullInfoMessageDto(MessageModel messageModel)
        {
            MessageDto message = _mapper.Map<MessageModel, MessageDto>(messageModel);
            message.ApplicationUserId = await GetMessageSenderId(messageModel.SenderUsername);
            return message;
        }

        private async Task<string> GetMessageSenderUsername(string userId)
        {
            return (await _userService.GetUserById(userId)).Username;
        }

        private async Task<string> GetMessageSenderId(string username)
        {
            return await _userService.GetUserId(username);
        }
    }
}
