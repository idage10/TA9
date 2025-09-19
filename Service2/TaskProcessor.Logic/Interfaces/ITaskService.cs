using TaskProcessor.Data;

namespace TaskProcessor.Logic.Interfaces
{
    public interface ITaskService
    {
        Task CreateTaskAsync(TaskEntityDto taskDto);
        Task UpdateTaskAsync(string id, bool isActive);
        Task DeleteTaskAsync(string id);
        Task<int?> GetTaskLevelAsync(string id);
    }
}