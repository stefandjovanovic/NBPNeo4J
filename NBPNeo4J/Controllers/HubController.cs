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

        [HttpPost("create")]
        public async Task<ActionResult<ReturnHubDTO>> CreateHubAsync([FromBody] CreateHubDTO hub)
        {
            ReturnHubDTO cretedHubDTO = await _hubService.CreateHub(hub);
            return cretedHubDTO;
        }

        [HttpPut("update/{hubId}")]
        public async Task<ActionResult<ReturnHubDTO>> UpdateHubAsync(string hubId, [FromBody] CreateHubDTO hub)
        {
            ReturnHubDTO updatedHubDTO = await _hubService.UpdateHub(hubId, hub);
            return updatedHubDTO;
        }

        [HttpDelete("delete/{hubId}")]
        public async Task<ActionResult> DeleteHubAsync(string hubId)
        {
            await _hubService.DeleteHub(hubId);
            return Ok();
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<ReturnHubDTO>>> GetAllHubsAsync()
        {
            List<ReturnHubDTO> hubs = await _hubService.GetAllHubs();
            return hubs;
        }

    }
}
