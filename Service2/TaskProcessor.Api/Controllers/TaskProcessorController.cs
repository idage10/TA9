using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;

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
        public async Task<string?> GetTaskLevelFromService3(string id)
        {
            var client = _httpClientFactory.CreateClient("Service3");
            var response = await client.GetAsync($"/analyze-task/{id}/level");

            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            return json;
        }
    }
}
