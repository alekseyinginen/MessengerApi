using AutoMapper;
using MessengerApi.BLL.Dto;
using MessengerApi.BLL.Interfaces;
using MessengerApi.DAL.Entities;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MessengerApi.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IConnectedUsersService connectedUsersService;
        private readonly IMessageService messageService;
        private readonly IUserService userService;

        public ChatHub(IConnectedUsersService connectedUsersService, IMessageService messageService, IUserService userService)
        {
            this.connectedUsersService = connectedUsersService;
            this.userService = userService;
            this.messageService = messageService;
        }

        public async Task Connect(string userId)
        {
            var id = Context.ConnectionId;

            var user = connectedUsersService.GetByUserId(userId);

            if (user == null)
            {
                await connectedUsersService.Connect(userId, id);

                var connectedUser = userService.GetUserById(userId);

                await GetRelatedConnectionsForUserAndSendDetails(user, "userConnected", connectedUser);
            }
            else
            {
                await connectedUsersService.UpdateConnection(user, id);
            }
        }

        public async Task Disconnect()
        {
            await OnDisconnectedAsync(new Exception("user disconnected..."));
        }

        public async Task SendMessage(string message, string groupId)
        {
            var id = Context.ConnectionId;

            var user = connectedUsersService.GetByConnectionId(id);

            if (user != null)
            {
                var messageItem = GetMessageDto(user, message, groupId);

                await messageService.Create(messageItem);

                await GetRelatedConnectionsForUserAndSendDetails(user, "addMessage", messageItem);
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var user = connectedUsersService.GetByConnectionId(Context.ConnectionId);

            if (user != null)
            {
                await connectedUsersService.Disconnect(user.ApplicationUserId);

                var disconnectedUser = userService.GetUserById(user.ApplicationUserId);

                await GetRelatedConnectionsForUserAndSendDetails(user, "userDisconnected", disconnectedUser);
            }

            await base.OnDisconnectedAsync(exception);
        }

        #region private methods
        private async Task GetRelatedConnectionsForUserAndSendDetails(ConnectedUser user, string method, object objectToSend)
        {
            var connectionsIds = await connectedUsersService.GetRelatedConnectionIds(user.ApplicationUserId);

            await Clients.Clients(connectionsIds).SendAsync(method, objectToSend);
        }

        private MessageDto GetMessageDto(ConnectedUser user, string message, string groupId)
        {
            return new MessageDto
            {
                ApplicationUserId = user.ApplicationUserId,
                GroupId = groupId,
                MessageText = message,
                PublishTime = DateTime.Now
            };
        }
        #endregion
    }
}
