using Microsoft.AspNetCore.Mvc;
using NBPNeo4J.DTOs;
using NBPNeo4J.Services;

namespace NBPNeo4J.Controllers
{
    [ApiController]
    [Route("api/hub")]
    public class HubController: ControllerBase
    {
        private readonly ILogger<HubController> _logger;
        private readonly IHubService _hubService;

        public HubController(ILogger<HubController> logger, IHubService service)
        {
            _logger = logger;
            _hubService = service;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateHubAsync([FromBody] CreateHubDTO hub)
        {
            return Ok();
        }

    }
}
