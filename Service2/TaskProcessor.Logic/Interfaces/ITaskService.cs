using TaskProcessor.Data.Models;

namespace TaskProcessor.Logic.Interfaces
{
    public interface ITaskService
    {
        Task AddTaskAsync(TaskEntity task);
        Task UpdateTaskStatusAsync(string id, bool isActive);
        Task DeleteTaskAsync(string id);
    }
}