using TaskProcessor.Data.Models;

namespace TaskProcessor.Data.Interfaces
{
    public interface ITaskRepository
    {
        Task AddTaskAsync(TaskEntityDto task);
        Task<TaskEntityDto?> GetTaskByIdAsync(string id);
        Task UpdateTaskAsync(TaskEntityDto task);
        Task DeleteTaskAsync(string id);
    }
}