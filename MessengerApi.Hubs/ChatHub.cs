using AutoMapper;
using MessengerApi.BLL.Interfaces;
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

        public ChatHub(IConnectedUsersService connectedUsersService)
        {
            this.connectedUsersService = connectedUsersService;
        }
        
        public async Task Connect()
        {
            //Clients.Client       
        }

        public async Task Disconnect()
        {

        }
    }
}
