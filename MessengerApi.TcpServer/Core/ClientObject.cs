﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using MessengerApi.TcpServer.Models;
using MessengerApi.TcpServer.Helpers;

namespace MessengerApi.TcpServer.Core
{
    public class ClientObject
    {
        protected internal string Id { get; private set; }
        protected internal NetworkStream Stream { get; private set; }

        private readonly TcpClient client;
        private readonly ServerObject server;

        private User user;

        public ClientObject(TcpClient tcpClient, ServerObject serverObject)
        {
            Id = Guid.NewGuid().ToString();
            client = tcpClient;
            server = serverObject;
            serverObject.AddConnection(this);
        }

        private BroadcastMessage GetBroadcastMessage(string username, string messageText)
        {
            return new BroadcastMessage
            {
                SenderUsername = username,
                MessageText = messageText,
                PublishTime = DateTime.Now
            };
        }

        private void BroadcastEventDetails(string message)
        {
            EventDetails details = new EventDetails { Message = message };
            string json = JsonFormatter.Serialize(details);
            server.BroadcastMessage(json, Id);
        }

        private async Task BroadcastMessage(string message)
        {
            BroadcastMessage broadcastMessage = GetBroadcastMessage(user.Username, message);
            await server.AddMessageToDatabase(broadcastMessage);
            string json = JsonFormatter.Serialize(broadcastMessage);
            server.BroadcastMessage(json, Id);
        }

        public async Task Process()
        {
            try
            {
                Stream = client.GetStream();
                user = JsonFormatter.Deserialize<User>(GetMessage());
                BroadcastEventDetails(user.Username + " entered chat");
                
                while (true)
                {
                    try
                    {
                        string json = GetMessage();
                        await BroadcastMessage(json);
                    }
                    catch (Exception)
                    {
                        BroadcastEventDetails(String.Format("{0}: left chat", user.Username));
                        break;
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                server.RemoveConnection(Id);
                Close();
            }
        }
        
        private string GetMessage()
        {
            byte[] data = new byte[64];
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            do
            {
                bytes = Stream.Read(data, 0, data.Length);
                builder.Append(Encoding.UTF8.GetString(data, 0, bytes)); // Unicode as default
            }
            while (Stream.DataAvailable);

            return ThrowIfStringNullOrEmpty(builder.ToString());
        }

        private string ThrowIfStringNullOrEmpty(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new Exception("string is null or empty");
            }
            return str;
        }
        
        protected internal void Close()
        {
            if (Stream != null)
                Stream.Close();
            if (client != null)
                client.Close();
        }
    }
}