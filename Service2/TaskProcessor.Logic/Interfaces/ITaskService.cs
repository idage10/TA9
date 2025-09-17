using TaskProcessor.Data;

namespace TaskProcessor.Logic.Interfaces
{
    public interface ITaskService
    {
        Task AddTaskAsync(TaskEntityDto taskDto);
        Task<TaskEntityDto?> GetTaskByIdAsync(string id);
        Task UpdateTaskAsync(TaskEntityDto taskDto);
        Task DeleteTaskAsync(string id);
    }
}