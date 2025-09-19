using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskProcessor.Data.Models;

public class TaskEntity
{
    [Key]
    [Column("id")] // column name in DB
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Column("parent_id")]
    public string? ParentId { get; set; }

    [ForeignKey(nameof(ParentId))]
    public TaskEntity? Parent { get; set; }

    public List<TaskEntity> Children { get; set; } = new();

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("name")]
    public string Name { get; set; } = string.Empty;
}