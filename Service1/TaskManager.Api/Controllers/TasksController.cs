using Microsoft.AspNetCore.Mvc;
using TaskManager.Api.Enums;
using TaskManager.Api.Models;
using TaskManager.Api.Services;


namespace TaskManager.Api.Controllers
{
    [ApiController]
    [Route("tasks")]
    public class TasksController : ControllerBase
    {
        private readonly WebSocketClientService _ws;
        public TasksController(WebSocketClientService ws) 
        { 
            _ws = ws;
        }
 
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskEntityDto task)
        {
            try
            {
                var cmd = new { Action = TaskAction.CreateTask.ToString(), Task = task };
                await _ws.SendCommandAsync(cmd);
                return Accepted(new { message = "CreateTask command sent" });
            }
            catch (Exception ex)
            {
                return Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Error while creating task"
                );
            }
        }
    
    
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(string id, [FromBody] StatusDto status)
        {
            try
            {
                var cmd = new { Action = TaskAction.UpdateTask, Id = id, IsActive = status.IsActive };
                await _ws.SendCommandAsync(cmd);
                return Accepted(new { message = "UpdateTask command sent" });
            }
            catch (Exception ex)
            {
                return Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Error while updating task"
                );
            }
        }
    
    
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(string id)
        {
            try
            {
                var cmd = new { Action = TaskAction.DeleteTask, Id = id };
                await _ws.SendCommandAsync(cmd);
                return Accepted(new { message = "DeleteTask command sent" });
            }
            catch (Exception ex)
            {
                return Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Error while deleting task"
                );
            }
        }
    }
}