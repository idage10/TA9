using TaskProcessor.Data;

namespace TaskProcessor.Logic.Interfaces
{
    public interface ITaskService
    {
        Task AddTaskAsync(TaskEntityDto task);
        Task<TaskEntityDto?> GetTaskByIdAsync(string id);
        Task UpdateTaskAsync(TaskEntityDto task);
        Task DeleteTaskAsync(string id);
    }
}