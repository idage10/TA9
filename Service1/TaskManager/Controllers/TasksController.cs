using Microsoft.AspNetCore.Mvc;
using TaskManager.Models;
using TaskManager.Services;


namespace TaskManager.Controllers
{
    [ApiController]
    [Route("tasks")]
    public class TasksController : ControllerBase
    {
        private readonly WebSocketClientService _ws;
        public TasksController(WebSocketClientService ws) { _ws = ws; }
    
    
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskDto task)
        {
            var cmd = new { action = "create", task };
            await _ws.SendCommandAsync(cmd);
            return Accepted(new { message = "Create command sent" });
        }
    
    
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(string id, [FromBody] StatusDto status)
        {
            var cmd = new { action = "update-status", id, isActive = status.IsActive };
            await _ws.SendCommandAsync(cmd);
            return Accepted(new { message = "Update-status command sent" });
        }
    
    
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(string id)
        {
            var cmd = new { action = "delete", id };
            await _ws.SendCommandAsync(cmd);
            return Accepted(new { message = "Delete command sent" });
        }
    }
}