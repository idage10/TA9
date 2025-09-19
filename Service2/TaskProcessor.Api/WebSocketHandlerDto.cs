using System.Text.Json.Serialization;
using TaskProcessor.Api.Enums;
using TaskProcessor.Data;

namespace TaskProcessor.Api
{
    public class CreateTaskCommand
    {
        [JsonPropertyName("action")]
        public TaskAction Action { get; set; } = TaskAction.CreateTask;

        [JsonPropertyName("task")]
        public TaskEntityDto Task { get; set; } = default!;
    }

    public class UpdateStatusCommand
    {
        [JsonPropertyName("action")]
        public TaskAction Action { get; set; } = TaskAction.UpdateTask;

        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("isActive")]
        public bool IsActive { get; set; }
    }

    public class DeleteTaskCommand
    {
        [JsonPropertyName("action")]
        public TaskAction Action { get; set; } = TaskAction.DeleteTask;

        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
    }
}
