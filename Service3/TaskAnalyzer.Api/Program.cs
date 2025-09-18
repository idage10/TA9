using Microsoft.EntityFrameworkCore;
using TaskProcessor.Data;
using TaskProcessor.Data.Interfaces;
using TaskProcessor.Data.Repositories;
using TaskProcessor.Logic.Interfaces;
using TaskProcessor.Logic.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Register DbContext
builder.Services.AddDbContext<TasksDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection")));

// Register Repository & Service
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITaskService, TaskService>();

var app = builder.Build();

app.MapControllers();

app.Run();