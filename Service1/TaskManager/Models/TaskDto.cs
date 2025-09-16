namespace TaskManager.Models
{
    public class TaskDto
    {
        public string Id { get; set; }
        public string? ParentId { get; set; }
        public List<string> ChildrenIds { get; set; } = new List<string>();
        public bool IsActive { get; set; }
        public string? Title { get; set; }
    }

    public class StatusDto { public bool IsActive { get; set; } }
}