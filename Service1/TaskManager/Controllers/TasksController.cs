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
            var cmd = new { action = "AddTask", task };
            await _ws.SendCommandAsync(cmd);
            return Accepted(new { message = "AddTask command sent" });
        }
    
    
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(string id, [FromBody] StatusDto status)
        {
            var cmd = new { action = "UpdateTask", id, isActive = status.IsActive };
            await _ws.SendCommandAsync(cmd);
            return Accepted(new { message = "UpdateTask command sent" });
        }
    
    
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(string id)
        {
            var cmd = new { action = "DeleteTask", id };
            await _ws.SendCommandAsync(cmd);
            return Accepted(new { message = "DeleteTask command sent" });
        }
    }
}