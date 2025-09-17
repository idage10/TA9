﻿using Microsoft.EntityFrameworkCore;
using TaskProcessor.Data.Interfaces;

namespace TaskProcessor.Data.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TasksDbContext _context;
        public TaskRepository(TasksDbContext context) => _context = context;

        public async Task AddTaskAsync(TaskEntityDto task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
        }

        public async Task<TaskEntityDto?> GetTaskByIdAsync(string id) =>
            await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);

        public async Task UpdateTaskAsync(TaskEntityDto task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTaskAsync(string id)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }
        }
    }
}