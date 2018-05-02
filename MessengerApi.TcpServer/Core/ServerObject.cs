using AutoMapper;
using MessengerApi.BLL.Dto;
using MessengerApi.BLL.Interfaces;
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
        private readonly IUserService _userService;
        private readonly IMessageService _messageService;
        private static TcpListener tcpListener;
        private List<ClientObject> clients = new List<ClientObject>();

        public ServerObject(IServiceProvider services)
        {
            using (var scope = services.CreateScope())
            {
                _mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
                _userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                _messageService = scope.ServiceProvider.GetRequiredService<IMessageService>();
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task task = Task.Factory.StartNew(() => Listen());
            return task;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Task task = Task.Factory.StartNew(() => Disconnect());
            return task;
        }

        protected internal void AddConnection(ClientObject clientObject)
        {
            clients.Add(clientObject);
        }

        protected internal void RemoveConnection(string id)
        {
            ClientObject client = clients.FirstOrDefault(c => c.Id == id);
            if (client != null)
            {
                clients.Remove(client);
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
                    Task.Factory.StartNew(() => clientObject.Process());
                }
            }
            catch (Exception)
            {
                Disconnect();
            }
        }

        protected internal void BroadcastMessage(string message, string id)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i].Id != id)
                {
                    clients[i].Stream.Write(data, 0, data.Length);
                }
            }
        }

        public void Disconnect()
        {
            tcpListener.Stop();

            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].Close();
            }
            Environment.Exit(0);
        }

        public async Task AddMessageToDatabase(BroadcastMessage message)
        {
            MessageDto messageDto = _mapper.Map<BroadcastMessage, MessageDto>(message);
            messageDto.ApplicationUserId = await _userService.GetUserId(message.SenderUsername);
            await _messageService.Create(messageDto);
        }
    }
}
