using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using TaskProcessor.Api.Enums;
using TaskProcessor.Logic.Interfaces;

namespace TaskProcessor.Api.Services
{
    public class WebSocketHandler
    {
        // Define json serialize/deserialize option to convert enum to string or string to enum
        private readonly JsonSerializerOptions _options = new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } };
        private readonly ILogger<WebSocketHandler> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly Dictionary<WebSocket, string> _clients = new();

        public WebSocketHandler(ILogger<WebSocketHandler> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }
    
        public async Task HandleAsync(HttpContext context, WebSocket webSocket)
        {
            _clients[webSocket] = Guid.NewGuid().ToString();
            _logger.LogInformation("New WebSocket client connected");
    
            var buffer = new byte[4096];
            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(buffer, CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    _logger.LogInformation("Client disconnected");
                    _clients.Remove(webSocket);
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                }
                else
                {
                    var msg = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    using var scope = _scopeFactory.CreateScope();
                    // Get logic layer task service
                    var service = scope.ServiceProvider.GetRequiredService<ITaskService>();

                    try
                    {
                        var doc = JsonDocument.Parse(msg);
                        if (!doc.RootElement.TryGetProperty("action", out var actionProp)) continue;
                        var actionStr = actionProp.GetString();
                        if (!Enum.TryParse<TaskAction>(actionStr, ignoreCase: true, out var action))
                            continue;

                        switch (action)
                        {
                            case TaskAction.CreateTask:
                                var addCmd = JsonSerializer.Deserialize<CreateTaskCommand>(msg, _options);
                                if (addCmd?.Task != null)
                                {
                                    await service.CreateTaskAsync(addCmd.Task);
                                }
                                break;

                            case TaskAction.UpdateTask:
                                var updateCmd = JsonSerializer.Deserialize<UpdateStatusCommand>(msg);
                                if (!string.IsNullOrEmpty(updateCmd?.Id))
                                {
                                    await service.UpdateTaskAsync(updateCmd.Id, updateCmd.IsActive);
                                }
                                break;

                            case TaskAction.DeleteTask:
                                var deleteCmd = JsonSerializer.Deserialize<DeleteTaskCommand>(msg);
                                if (!string.IsNullOrEmpty(deleteCmd?.Id))
                                {
                                    await service.DeleteTaskAsync(deleteCmd.Id);
                                }
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error processing WebSocket message");
                    }
                }
            }
        }
    
        // Optional for sending websocket messages back to the client
        private async Task BroadcastAsync(string msg)
        {
            var bytes = Encoding.UTF8.GetBytes(msg);
            var seg = new ArraySegment<byte>(bytes);
            foreach (var socket in _clients.Keys.ToList())
            {
                if (socket.State == WebSocketState.Open)
                {
                    await socket.SendAsync(seg, WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }
    }
}