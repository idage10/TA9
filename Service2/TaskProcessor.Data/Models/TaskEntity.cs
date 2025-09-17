using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskProcessor.Data.Models;

public class TaskEntity
{
    [Key]
    [Column("id")] // column name in DB
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Column("parent_id")] // snake_case column
    public string? ParentId { get; set; }

    [ForeignKey(nameof(ParentId))]
    public TaskEntity? Parent { get; set; }

    public List<TaskEntity> Children { get; set; } = new();

    [Column("is_active")] // snake_case column
    public bool IsActive { get; set; } = true;

    [Column("name")] // snake_case column
    public string Name { get; set; } = string.Empty;
}