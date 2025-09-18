using TaskProcessor.Data.Models;

namespace TaskProcessor.Data.Interfaces
{
    public interface ITaskRepository
    {
        Task AddTaskAsync(TaskEntityDto task);
        Task<TaskEntityDto?> GetTaskByIdWithChildrenAsync(string id);
        Task UpdateTaskAsync(string id, bool isActive);
        Task DeleteTaskAsync(string id);
    }
}