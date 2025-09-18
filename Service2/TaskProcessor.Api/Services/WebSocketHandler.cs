using System;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskProcessor.Data;
using TaskProcessor.Data.Models;
using TaskProcessor.Logic.Interfaces;

namespace TaskProcessor.Api.Services
{
    public class WebSocketHandler
    {
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
                    var service = scope.ServiceProvider.GetRequiredService<ITaskService>();

                    try
                    {
                        var doc = JsonDocument.Parse(msg);
                        if (!doc.RootElement.TryGetProperty("action", out var actionProp)) continue;
                        var action = actionProp.GetString();

                        switch (action)
                        {
                            case "AddTask":
                                var addCmd = JsonSerializer.Deserialize<AddTaskCommand>(msg);
                                if (addCmd?.Task != null)
                                {
                                    await service.AddTaskAsync(addCmd.Task);
                                }
                                break;

                            case "UpdateTask":
                                var updateCmd = JsonSerializer.Deserialize<UpdateStatusCommand>(msg);
                                if (!string.IsNullOrEmpty(updateCmd?.Id))
                                {
                                    await service.UpdateTaskAsync(updateCmd.Id, updateCmd.IsActive);
                                }
                                break;

                            case "DeleteTask":
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
    
        // Optional for sending messages back to the client
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