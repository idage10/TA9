using TaskProcessor.Data.Models;

namespace TaskProcessor.Data.Interfaces
{
    public interface ITaskRepository
    {
        Task AddTaskAsync(TaskEntity task);
        Task<TaskEntity?> GetTaskByIdAsync(string id);
        Task UpdateTaskAsync(TaskEntity task);
        Task DeleteTaskAsync(string id);
    }
}