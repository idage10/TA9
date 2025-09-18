using Microsoft.AspNetCore.Mvc;
using TaskProcessor.Logic.Interfaces;

namespace Service3.Controllers
{
    [ApiController]
    [Route("analyze-task")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet("{id}/level")]
        // Calculate tree level from root entity task to maximum lowest child in the parent-child chain
        public async Task<IActionResult> GetTaskLevel(string id)
        {
            var level = await _taskService.GetTaskLevelAsync(id);
            if (level == null)
            {
                return NotFound("Task not found");
            }

            return Ok($"The maximum tree level for this task is {level}.");
        }
    }
}