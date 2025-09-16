using TaskProcessor.Data.Interfaces;
using TaskProcessor.Data.Models;
using TaskProcessor.Logic.Interfaces;

namespace TaskProcessor.Logic.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _repo;
        public TaskService(ITaskRepository repo) => _repo = repo;

        public async Task AddTaskAsync(TaskEntity task) =>
            await _repo.AddTaskAsync(task);

        public async Task UpdateTaskStatusAsync(string id, bool isActive)
        {
            var task = await _repo.GetTaskByIdAsync(id);
            if (task == null) return;
            task.IsActive = isActive;
            await _repo.UpdateTaskAsync(task);
        }

        public async Task DeleteTaskAsync(string id) =>
            await _repo.DeleteTaskAsync(id);
    }
}