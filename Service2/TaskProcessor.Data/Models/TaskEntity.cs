using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskProcessor.Data.Models;

public class TaskEntity
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string? ParentId { get; set; }

    [ForeignKey("ParentId")]
    public TaskEntity? Parent { get; set; }

    public List<TaskEntity> Children { get; set; } = new();

    public bool IsActive { get; set; } = true;

    public string Name { get; set; } = string.Empty;
}