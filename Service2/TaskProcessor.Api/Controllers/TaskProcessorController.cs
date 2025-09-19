using Microsoft.AspNetCore.Mvc;

namespace TaskProcessor.Api.Controllers
{
    [Route("task-processor")]
    [ApiController]
    public class TaskProcessorController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public TaskProcessorController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("{id}")]
        // Send http request to Service3
        public async Task<IActionResult?> GetTaskLevelFromService3(string id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("Service3");
                var response = await client.GetAsync($"/analyze-task/{id}/level");

                if (!response.IsSuccessStatusCode)
                {
                    return Problem(
                        detail: "Failed to get http client response",
                        statusCode: StatusCodes.Status500InternalServerError,
                        title: "Error while getting task tree level"
                    );
                }

                var jsonString = await response.Content.ReadAsStringAsync();
                return Accepted(new { message = jsonString });
            }
            catch (Exception ex)
            {
                return Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Error while getting task tree level"
                );
            }
        }
    }
}
