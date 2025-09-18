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

        public async Task AddTaskAsync(TaskEntityDto taskDto) =>
            await _repo.AddTaskAsync(taskDto);

        public async Task<TaskEntityDto?> GetTaskByIdAsync(string id) =>
            await _repo.GetTaskByIdAsync(id);

        public async Task UpdateTaskAsync(string id, bool isActive) =>
            await _repo.UpdateTaskAsync(id, isActive);

        public async Task DeleteTaskAsync(string id) =>
            await _repo.DeleteTaskAsync(id);

        /// <summary>
        /// Calculates the depth of a task in the parent-child chain.
        /// </summary>
        public async Task<int?> GetTaskLevelAsync(string id)
        {
            var task = await _repo.GetTaskByIdAsync(id);
            if (task == null) return null;

            int level = 0;
            var current = task;

            while (current.ParentId != null)
            {
                var parent = await _repo.GetTaskByIdAsync(current.ParentId);
                if (parent == null) break; // corrupted chain

                level++;
                current = parent;
            }

            return level;
        }
    }
}