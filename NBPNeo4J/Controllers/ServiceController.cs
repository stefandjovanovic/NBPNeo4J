using Microsoft.AspNetCore.Mvc;
using NBPNeo4J.DTOs;
using NBPNeo4J.Models;
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
            try
            {
                ReturnServiceDTO createdServiceDTO = await _serviceService.CreateService(service);
                return Ok(createdServiceDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
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

        [HttpPost("{serviceId}/connect-to-hub/{hubId}")]
        public async Task<IActionResult> ConnectServiceToHub(string serviceId, string hubId)
        {
            try
            {
                await _serviceService.ConnectServiceToHub(serviceId, hubId);
                return Ok(new { Message = "Service station connected to hub successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("{serviceStationId}/add-vehicle")]
        public async Task<IActionResult> AddVehicleToServiceStationAsync(string serviceStationId, [FromBody] AddVehicleToServiceStationDTO dto)
        {
            if (dto == null || dto.Vehicle == null)
                return BadRequest("Invalid input data");

            Vehicle vehicle = await _serviceService.AddVehicleToServiceStationAsync(serviceStationId, dto.Vehicle, dto.PartsIds, dto.Date);
            return Ok(vehicle);
        }

        [HttpDelete("{serviceStationId}/remove-vehicle/{vehicleId}")]
        public async Task<ActionResult<Vehicle>> RemoveVehicleFromServiceStationAsync(string serviceStationId, string vehicleId)
        {
            try
            {
                Vehicle removedVehicle = await _serviceService.RemoveVehicleFromServiceStationAsync(serviceStationId, vehicleId);
                return Ok(removedVehicle);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("{serviceId}/vehicles")]
        public async Task<ActionResult<List<Vehicle>>> GetVehicles(string serviceId)
        {
            try
            {
                List<Vehicle> vehicles = await _serviceService.GetAllVehicles(serviceId);
                return Ok(vehicles);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


    }
}
