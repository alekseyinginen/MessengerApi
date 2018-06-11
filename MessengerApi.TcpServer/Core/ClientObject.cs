using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using MessengerApi.TcpServer.Models;
using MessengerApi.TcpServer.Helpers;

namespace MessengerApi.TcpServer.Core
{
    public class ClientObject
    {
        protected internal NetworkStream Stream { get; private set; }

        private readonly TcpClient client;
        private readonly ServerObject server;

        private User user;
        public User User => user;

        public string ConnectionId { get; }

        public ClientObject(TcpClient tcpClient, ServerObject serverObject)
        {
            ConnectionId = Guid.NewGuid().ToString();
            client = tcpClient;
            server = serverObject;
            serverObject.AddConnection(this);
        }

        public async Task Process()
        {
            try
            {
                Stream = client.GetStream();
                var transportModel = RecieveTransportModel();

                user = JsonFormatter.Deserialize<User>(transportModel.JsonData);
                await server.Connect(user.Id, ConnectionId);

                while (true)
                {
                    try
                    {
                        transportModel = RecieveTransportModel();
                        await ParseTransportMethodAndSend(transportModel);
                    }
                    catch (Exception)
                    {
                        await server.Disconnect(ConnectionId);
                        break;
                    }
                }
            }
            finally
            {
                server.RemoveConnection(ConnectionId);
                Close();
            }
        }
        
        
        
        protected internal void Close()
        {
            if (Stream != null)
            {
                Stream.Close();
            }
            if (client != null)
            {
                client.Close();
            } 
        }

        #region private methods

        private async Task ParseTransportMethodAndSend(TransportModel model)
        {
            if (model.Method == "addMessage")
            {
                var message = JsonFormatter.Deserialize<BroadcastMessage>(model.JsonData);
                await server.SendMessage(message.MessageText, message.GroupId, ConnectionId);
            }
            else if (model.Method == "userDisconnected")
            {
                var details = JsonFormatter.Deserialize<EventDetails>(model.JsonData);
                await server.Disconnect(ConnectionId);
            }
        }

        private TransportModel RecieveTransportModel()
        {
            var json = GetMessageFromStream();

            var transportModel = JsonFormatter.Deserialize<TransportModel>(json);

            return transportModel;
        }

        private string GetMessageFromStream()
        {
            byte[] data = new byte[64];
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            do
            {
                bytes = Stream.Read(data, 0, data.Length);
                builder.Append(Encoding.UTF8.GetString(data, 0, bytes));
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

        #endregion
    }
}
