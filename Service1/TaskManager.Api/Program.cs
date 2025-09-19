using Microsoft.OpenApi.Models;
using TaskManager.Api.Services;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSingleton<WebSocketClientService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TaskManager API", Version = "v1" });
});

// Enable Cross-origin resource sharing (CORS)
builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrigin", options =>
         options.AllowAnyOrigin(). // Allow any header, including content-type
         AllowAnyHeader()
        .AllowAnyMethod()
    );
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskManager API v1"));
app.MapControllers();

// Start websocket client on startup for sending requests to Service 2
var wsService = app.Services.GetRequiredService<WebSocketClientService>();
_ = wsService.ConnectAsync(); // fire-and-forget (it reconnects internally)

app.UseCors("AllowOrigin");
app.Run();