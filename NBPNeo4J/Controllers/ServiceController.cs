using Microsoft.AspNetCore.Mvc;
using NBPNeo4J.DTOs;
using NBPNeo4J.Services;

namespace NBPNeo4J.Controllers
{
    [ApiController]
    [Route("api/service")]
    public class ServiceController : ControllerBase
    {
        private readonly ILogger<ServiceController> _logger;
        private readonly IServiceStationService _serviceService;
        public ServiceController(ILogger<ServiceController> logger, IServiceStationService service)
        {
            _logger = logger;
            _serviceService = service;
        }
        [HttpPost("create")]
        public async Task<ActionResult<ReturnServiceDTO>> CreateServiceAsync([FromBody] CreateServiceDTO service)
        {
            ReturnServiceDTO cretedServiceDTO = await _serviceService.CreateService(service);
            return cretedServiceDTO;
        }
        [HttpPut("update/{serviceId}")]
        public async Task<ActionResult<ReturnServiceDTO>> UpdateServiceAsync(string serviceId, [FromBody] CreateServiceDTO service)
        {
            ReturnServiceDTO updatedServiceDTO = await _serviceService.UpdateService(serviceId, service);
            return updatedServiceDTO;
        }
        [HttpDelete("delete/{serviceId}")]
        public async Task<ActionResult> DeleteServiceAsync(string serviceId)
        {
            await _serviceService.DeleteService(serviceId);
            return Ok();
        }
        [HttpGet("all")]
        public async Task<ActionResult<List<ReturnServiceDTO>>> GetAllServicesAsync()
        {
            List<ReturnServiceDTO> services = await _serviceService.GetAllServices();
            return services;
        }
    }
}
