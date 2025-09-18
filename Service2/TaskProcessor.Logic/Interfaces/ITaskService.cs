using TaskProcessor.Data;

namespace TaskProcessor.Logic.Interfaces
{
    public interface ITaskService
    {
        Task AddTaskAsync(TaskEntityDto taskDto);
        Task<TaskEntityDto?> GetTaskByIdAsync(string id);
        Task UpdateTaskAsync(string id, bool isActive);
        Task DeleteTaskAsync(string id);
        Task<int?> GetTaskLevelAsync(string id);
    }
}