using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace TaskManager.Api.Services
{
    public class WebSocketClientService
    {
        private ClientWebSocket? _socket;
        private readonly Uri _serverUri = new Uri("ws://localhost:6000/ws");
        private readonly TimeSpan _reconnectDelay = TimeSpan.FromSeconds(3);
        private readonly CancellationTokenSource _cts = new();
    
    
        public async Task ConnectAsync()
        {
            while (!_cts.IsCancellationRequested)
            {
                try
                {
                    _socket = new ClientWebSocket();
                    await _socket.ConnectAsync(_serverUri, CancellationToken.None);
                    _ = ReceiveLoop(_socket); // keep receiving
                    return;
                }
                catch
                {
                    await Task.Delay(_reconnectDelay);
                }
            }
        }
    
        // Listen for websocket responses
        private async Task ReceiveLoop(ClientWebSocket socket)
        {
            var buffer = new byte[4096];
            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close) break;
                // For responses, Handle ack messages here
            }
        }
    
        private async Task EnsureConnected()
        {
            if (_socket == null || _socket.State != WebSocketState.Open)
            {
                await ConnectAsync();
                // Small delay
                await Task.Delay(200);
            }
        }
    
        public async Task SendCommandAsync(object cmd)
        {
            await EnsureConnected();
            if (_socket?.State != WebSocketState.Open) throw new Exception("WebSocket not connected");
            JsonSerializerOptions _jsonOptions = new JsonSerializerOptions {
                // Define using camelcase for properties naming when sending json requests and start property name with first lower letter
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                // Define websocket json serialize/deserialize option to convert enum to string or string to enum when sending requests
                Converters = { new JsonStringEnumConverter() } 
            };
            var json = JsonSerializer.Serialize(cmd, _jsonOptions);
            var bytes = Encoding.UTF8.GetBytes(json);
            await _socket!.SendAsync(buffer: new ArraySegment<byte>(bytes), messageType: WebSocketMessageType.Text, endOfMessage: true, cancellationToken: CancellationToken.None);
        }
    }
}