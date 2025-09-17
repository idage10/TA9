using System.Text.Json.Serialization;
using TaskProcessor.Data;
using TaskProcessor.Data.Models;

namespace TaskProcessor.Api
{
    public class UpdateStatusCommand
    {
        public string Action { get; set; } = "UpdateTask";
        public string Id { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    public class AddTaskCommand
    {
        public string Action { get; set; } = "AddTask";
        public TaskEntityDto Task { get; set; } = default!;
    }

    public class DeleteTaskCommand
    {
        [JsonPropertyName("action")]
        public string Action { get; set; } = "DeleteTask";

        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
    }
}
