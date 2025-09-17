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

        public async Task UpdateTaskAsync(string id, bool isActive)
        {
            await _repo.UpdateTaskAsync(id, isActive);
        }

        public async Task DeleteTaskAsync(string id) =>
            await _repo.DeleteTaskAsync(id);
    }
}