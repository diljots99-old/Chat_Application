using System;
using System.Net.WebSockets;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace Chat_Application.SocketsManger
{
    public class ConnectionManager
    {
        private ConcurrentDictionary<string, WebSocket> _connections = new ConcurrentDictionary<string, WebSocket>();
        
        public WebSocket GetSocketByID(string id)
        {
            return _connections.FirstOrDefault(x => x.Key == id).Value;

        }

        public ConcurrentDictionary<string,WebSocket> GetAllConnections()
        {
            return _connections;
        }
        public string GetID(WebSocket socket)
        {
            return _connections.FirstOrDefault(x => x.Value == socket).Key;
        }

        public async Task RemoveSocketAsync(string id)
        {
            _connections.TryRemove(id, out var socket);
            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "socket connection closed ",CancellationToken.None);

        }

        public void AddSocket(WebSocket socket)
        {
            _connections.TryAdd(GetConnectionID(), socket);
        }
        private string GetConnectionID()
        {
        
            return Guid.NewGuid().ToString("D");

        }


    }
}
