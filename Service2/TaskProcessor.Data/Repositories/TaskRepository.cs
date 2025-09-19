using Microsoft.EntityFrameworkCore;
using TaskProcessor.Data.Interfaces;
using TaskProcessor.Data.Models;

namespace TaskProcessor.Data.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TasksDbContext _context;
        public TaskRepository(TasksDbContext context) => _context = context;

        public async Task CreateTaskAsync(TaskEntityDto taskDto)
        {
            TaskEntity entity = new TaskEntity
            {
                Id = taskDto.Id,
                ParentId = taskDto.ParentId,
                Name = taskDto.Name,

            };
            _context.Tasks.Add(entity);
            await _context.SaveChangesAsync();
        }

        // Get task by id with its children. Load all tasks once from the Database, build tree in memory including children.
        public async Task<TaskEntityDto?> GetTaskByIdWithChildrenAsync(string id)
        {
            // Load all tasks
            var allTasks = await _context.Tasks.ToListAsync();

            TaskEntity? rootEntity = allTasks.FirstOrDefault(t => t.Id == id);

            if (rootEntity == null) return null;
            
            // Populate the children for every task
            AttachChildren(rootEntity, allTasks);

            // Map the task entity to task entity Dto including all the children
            return MapEntityToDto(rootEntity);
        }

        // Build tree in memory and find all children for every task
        void AttachChildren(TaskEntity node, List<TaskEntity> allTasks)
        {
            // Set children list for every task
            node.Children = allTasks.Where(t => t.ParentId == node.Id).ToList();
            foreach (var child in node.Children)
            {
                AttachChildren(child, allTasks);
            }
        }

        private TaskEntityDto MapEntityToDto(TaskEntity entity)
        {
            return new TaskEntityDto
            {
                Id = entity.Id,
                ParentId = entity.ParentId,
                IsActive = entity.IsActive,
                Name = entity.Name,
                Children = entity.Children?.Select(MapEntityToDto).ToList() ?? new List<TaskEntityDto>()
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