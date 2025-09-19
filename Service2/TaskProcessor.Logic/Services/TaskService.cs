using TaskProcessor.Data;
using TaskProcessor.Data.Interfaces;
using TaskProcessor.Data.Models;
using TaskProcessor.Logic.Interfaces;

namespace TaskProcessor.Logic.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _repo;
        public TaskService(ITaskRepository repo) => _repo = repo;

        public async Task CreateTaskAsync(TaskEntityDto taskDto) =>
            await _repo.CreateTaskAsync(taskDto);

        public async Task UpdateTaskAsync(string id, bool isActive) =>
            await _repo.UpdateTaskAsync(id, isActive);

        public async Task DeleteTaskAsync(string id) =>
            await _repo.DeleteTaskAsync(id);

        /// <summary>
        /// Calculate tree level from root entity task to maximum lowest child in the parent-child chain (downwards from parent to children)
        /// </summary>
        public async Task<int?> GetTaskLevelAsync(string id)
        {
            var rootTaskEntity = await _repo.GetTaskByIdWithChildrenAsync(id); // include children
            if (rootTaskEntity == null)
            {
                return null;
            }

            return CalculateLevel(rootTaskEntity);
        }

        // Calculate tree level from root entity task to maximum lowest child in the parent-child chain
        private int CalculateLevel(TaskEntityDto task)
        {
            if (task.Children == null || !task.Children.Any())
            {
                return 1; // leaf node level is 1
            }

            // 1 (current node) + max depth of children
            return 1 + task.Children.Max(t => CalculateLevel(t));
        }
    }
}