using System.Text.Json.Serialization;
using TaskProcessor.Data;
using TaskProcessor.Data.Models;

namespace TaskProcessor.Api
{
    public class AddTaskCommand
    {
        [JsonPropertyName("action")]
        public string Action { get; set; } = "AddTask";

        [JsonPropertyName("task")]
        public TaskEntityDto Task { get; set; } = default!;
    }

    public class UpdateStatusCommand
    {
        [JsonPropertyName("action")]
        public string Action { get; set; } = "UpdateTask";

        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("isActive")]
        public bool IsActive { get; set; }
    }

    public class DeleteTaskCommand
    {
        [JsonPropertyName("action")]
        public string Action { get; set; } = "DeleteTask";

        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
    }
}
