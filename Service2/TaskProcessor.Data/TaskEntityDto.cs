using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TaskProcessor.Data.Models;

namespace TaskProcessor.Data
{
    public class TaskEntityDto
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [JsonPropertyName("parentId")]
        public string? ParentId { get; set; }

        public TaskEntity? Parent { get; set; }

        public List<TaskEntity> Children { get; set; } = new();

        public bool IsActive { get; set; } = true;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }
}
