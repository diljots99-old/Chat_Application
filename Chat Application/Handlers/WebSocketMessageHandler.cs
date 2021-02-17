using Chat_Application.SocketsManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Chat_Application.Handlers
{
    public class WebSocketMessageHandler:SocketHandler
    {
        public WebSocketMessageHandler(ConnectionManager connections): base(connections)
        {

        }

        public override async Task OnConnected(WebSocket socket)
        {
            await base.OnConnected(socket);

            var socketId = Connections.GetID(socket);
            await SendMessageToAll($"Welcome, {socketId}" );
                 

        }

        public override async Task Receive(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            var socketID = Connections.GetID(socket);
            var message = $"{socketID} said: {Encoding.UTF8.GetString(buffer, 0, result.Count)}";

            await SendMessageToAll(message);

        }
    }
   
}
