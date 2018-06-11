using AutoMapper;
using MessengerApi.BLL.Dto;
using MessengerApi.BLL.Interfaces;
using MessengerApi.DAL.Entities;
using MessengerApi.TcpServer.Helpers;
using MessengerApi.TcpServer.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MessengerApi.TcpServer.Core
{
    public class ServerObject : IHostedService
    {
        private readonly IMapper _mapper;
        private readonly IConnectedUsersService connectedUsersService;
        private readonly IMessageService messageService;
        private readonly IUserService userService;

        private static TcpListener tcpListener;
        private List<ClientObject> Clients { get; }

        public ServerObject(IServiceProvider services)
        {
            using (var scope = services.CreateScope())
            {
                _mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
                userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                messageService = scope.ServiceProvider.GetRequiredService<IMessageService>();
                connectedUsersService = scope.ServiceProvider.GetRequiredService<IConnectedUsersService>();
            }

            Clients = new List<ClientObject>();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task task = Task.Factory.StartNew(() => Listen());
            return task;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Task task = Task.Factory.StartNew(() => StopListening());
            return task;
        }

        protected internal void AddConnection(ClientObject clientObject)
        {
            Clients.Add(clientObject);
        }

        protected internal void RemoveConnection(string id)
        {
            ClientObject client = Clients.FirstOrDefault(c => c.ConnectionId == id);
            if (client != null)
            {
                Clients.Remove(client);
            }
        }

        public void Listen()
        {
            try
            {
                tcpListener = new TcpListener(IPAddress.Any, 8888);
                tcpListener.Start();
                while (true)
                {
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();
                    ClientObject clientObject = new ClientObject(tcpClient, this);
                    Task.Run(() => clientObject.Process());
                }
            }
            catch (Exception)
            {
                StopListening();
            }
        }

        public async Task Connect(string userId, string connectionId)
        {
            var user = connectedUsersService.GetByUserId(userId);

            if (user == null)
            {
                await connectedUsersService.Connect(userId, connectionId);

                var connectedUser = userService.GetUserById(userId);

                await GetRelatedConnectionsForUserAndSendDetails(user, "userConnected", connectedUser);
            }
            else
            {
                await connectedUsersService.UpdateConnection(user, connectionId);
            }
        }

        public async Task Disconnect(string connectionId)
        {
            await OnDisconnectedAsync(connectionId);
        }

        public async Task SendMessage(string message, string groupId, string connectionId)
        {
            var user = connectedUsersService.GetByConnectionId(connectionId);

            if (user != null)
            {
                var messageItem = GetMessageDto(user, message, groupId);

                await messageService.Create(messageItem);

                await GetRelatedConnectionsForUserAndSendDetails(user, "addMessage", messageItem);
            }
        }

        public async Task OnDisconnectedAsync(string connectionId)
        {
            var user = connectedUsersService.GetByConnectionId(connectionId);

            if (user != null)
            {
                await connectedUsersService.Disconnect(user.ApplicationUserId);

                var disconnectedUser = userService.GetUserById(user.ApplicationUserId);

                await GetRelatedConnectionsForUserAndSendDetails(user, "userDisconnected", disconnectedUser);
            }
        }

        public void StopListening()
        {
            tcpListener.Stop();

            for (int i = 0; i < Clients.Count; i++)
            {
                Clients[i].Close();
            }
            Environment.Exit(0);
        }

        #region private methods

        private async Task GetRelatedConnectionsForUserAndSendDetails(ConnectedUser user, string method, object objectToSend)
        {
            var connectionsIds = await connectedUsersService.GetRelatedConnectionIds(user.ApplicationUserId);

            SendToClients(connectionsIds, method, objectToSend);
        }

        private void SendToClients<T>(List<string> connectionIds, string method, T data)
            where T : class
        {
            var clientsToSend = Clients.Where(x => connectionIds.Contains(x.ConnectionId)).ToList();

            var model = GetTransportModel(method, data);

            byte[] bytes = Encoding.UTF8.GetBytes(JsonFormatter.Serialize(model));

            foreach (var client in clientsToSend)
            {
                client.Stream.Write(bytes, 0, bytes.Length);
            }
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

        private TransportModel GetTransportModel<T>(string method, T data)
            where T : class
        {
            return new TransportModel
            {
                Method = method,
                JsonData = JsonFormatter.Serialize(data)
            };
        }
        
        #endregion
    }
}
