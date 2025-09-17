namespace TaskManager.Models
{
    public class TaskEntityDto
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string? ParentId { get; set; }

        public TaskEntityDto? Parent { get; set; }

        public List<TaskEntityDto> Children { get; set; } = new();

        public bool IsActive { get; set; } = true;

        public string Name { get; set; } = string.Empty;
    }

    public class StatusDto { public bool IsActive { get; set; } }
}