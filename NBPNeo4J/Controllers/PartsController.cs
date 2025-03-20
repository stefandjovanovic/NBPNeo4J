using Microsoft.AspNetCore.Mvc;
using NBPNeo4J.DTOs;
using NBPNeo4J.Services;

namespace NBPNeo4J.Controllers
{
    [ApiController]
    [Route("api/parts")]
    public class PartsController : ControllerBase
    {
        private readonly ILogger<PartsController> _logger;
        private readonly IPartsService _partsService;
        public PartsController(ILogger<PartsController> logger, IPartsService service)
        {
            _logger = logger;
            _partsService = service;
        }
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreatePartAsync([FromBody] CreatePartDTO part)
        {
            //return Ok();
            var result = await _partsService.CreatePart(part);
            return CreatedAtAction(nameof(CreatePartAsync), new { id = result.SerialCode }, result);

        }
    }
}
