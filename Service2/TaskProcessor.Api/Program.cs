using Microsoft.EntityFrameworkCore;
using TaskProcessor.Api.Services;
using TaskProcessor.Data;
using TaskProcessor.Data.Interfaces;
using TaskProcessor.Data.Repositories;
using TaskProcessor.Logic.Interfaces;
using TaskProcessor.Logic.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TasksDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection")));

builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddSingleton<WebSocketHandler>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddLogging();
builder.Services.AddHttpClient("Service3", client =>
{
    client.BaseAddress = new Uri("http://localhost:7000"); // service3 URL
});

var app = builder.Build();

app.MapControllers();
app.UseWebSockets();

app.Map("/ws", async (HttpContext context, WebSocketHandler handler) =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        var socket = await context.WebSockets.AcceptWebSocketAsync();
        await handler.HandleAsync(context, socket);
    }
    else
    {
        context.Response.StatusCode = 400;
    }
});

app.Run();