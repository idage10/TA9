using Microsoft.EntityFrameworkCore;
using TaskProcessor.Data.Interfaces;
using TaskProcessor.Data.Models;

namespace TaskProcessor.Data.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TasksDbContext _context;
        public TaskRepository(TasksDbContext context) => _context = context;

        public async Task AddTaskAsync(TaskEntityDto taskDto)
        {
            TaskEntity entity = new TaskEntity
            {
                ParentId = taskDto.ParentId,
                Name = taskDto.Name,

            };
            _context.Tasks.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<TaskEntityDto?> GetTaskByIdAsync(string id)
        {
            TaskEntity? entity = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);

            if (entity == null) 
            {
                return null;
            }

            return new TaskEntityDto
            {
                Id = entity.Id,
                ParentId = entity.ParentId,
                Parent = entity.Parent,
                Children = entity.Children,
                IsActive = entity.IsActive,
                Name = entity.Name
            };
        }

        public async Task UpdateTaskAsync(string id, bool isActive)
        {
            var entity = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);

            if (entity == null) return;

            entity.IsActive = isActive;

            _context.Tasks.Update(entity);
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