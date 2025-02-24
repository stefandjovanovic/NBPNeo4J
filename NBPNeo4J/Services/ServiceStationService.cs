using NBPNeo4J.Models;
using NBPNeo4J.Repositories;
using NBPNeo4J.DTOs;

namespace NBPNeo4J.Services
{
    public interface IServiceStationService
    {
        Task<ReturnServiceDTO> CreateService(CreateServiceDTO createServiceDTO);
        Task<ReturnServiceDTO> UpdateService(string serviceId, CreateServiceDTO createServiceDTO);
        Task DeleteService(string serviceId);

        Task<ReturnServiceDTO> GetService(string serviceId);
        Task<List<ReturnServiceDTO>> GetAllServices();


        
    }
    public class ServiceStationService : IServiceStationService
    {
        private readonly IServiceStationRepository _serviceStationRepository;

        public ServiceStationService(IServiceStationRepository serviceStationRepository)
        {
            _serviceStationRepository = serviceStationRepository;
        }

        public async Task<ReturnServiceDTO> CreateService(CreateServiceDTO createServiceDTO)
        {
            var serviceStation = new ServiceStation
            {
                Id = Guid.NewGuid().ToString(),
                Name = createServiceDTO.Name,
                Address = createServiceDTO.Address,
                City = createServiceDTO.City,
                Latitude = createServiceDTO.Latitude,
                Longitude = createServiceDTO.Longitude
            };
            ServiceStation createdServiceStation = await _serviceStationRepository.CreateServiceStationAsync(serviceStation);
            
            ReturnServiceDTO returnServiceDTO = new ReturnServiceDTO();
            returnServiceDTO.Id = createdServiceStation.Id;
            returnServiceDTO.Name = createdServiceStation.Name;
            returnServiceDTO.Address = createdServiceStation.Address;
            returnServiceDTO.City = createdServiceStation.City;
            returnServiceDTO.Latitude = createdServiceStation.Latitude;
            returnServiceDTO.Longitude = createdServiceStation.Longitude;
            
            return returnServiceDTO;
        }

        public async Task<ReturnServiceDTO> UpdateService(string serviceId, CreateServiceDTO createServiceDTO)
        {
            var serviceStation = new ServiceStation
            {
                Id = serviceId,
                Name = createServiceDTO.Name,
                Address = createServiceDTO.Address,
                City = createServiceDTO.City,
                Latitude = createServiceDTO.Latitude,
                Longitude = createServiceDTO.Longitude
            };
            ServiceStation updatedServiceStation = await _serviceStationRepository.UpdateServiceStationAsync(serviceStation);

            ReturnServiceDTO returnServiceDTO = new ReturnServiceDTO();
            returnServiceDTO.Id = updatedServiceStation.Id;
            returnServiceDTO.Name = updatedServiceStation.Name;
            returnServiceDTO.Address = updatedServiceStation.Address;
            returnServiceDTO.City = updatedServiceStation.City;
            returnServiceDTO.Latitude = updatedServiceStation.Latitude;
            returnServiceDTO.Longitude = updatedServiceStation.Longitude;

            return returnServiceDTO;
        }

        public async Task DeleteService(string serviceId)
        {
            await _serviceStationRepository.DeleteServiceStationAsync(serviceId);
        }

        public async Task<ReturnServiceDTO> GetService(string serviceId)
        {
            ServiceStation serviceStation = await _serviceStationRepository.GetServiceStationAsync(serviceId);
            ReturnServiceDTO returnServiceDTO = new ReturnServiceDTO();
            returnServiceDTO.Id = serviceStation.Id;
            returnServiceDTO.Name = serviceStation.Name;
            returnServiceDTO.Address = serviceStation.Address;
            returnServiceDTO.City = serviceStation.City;
            returnServiceDTO.Latitude = serviceStation.Latitude;
            returnServiceDTO.Longitude = serviceStation.Longitude;
            return returnServiceDTO;
        }

        public async Task<List<ReturnServiceDTO>> GetAllServices()
        {
            List<ServiceStation> serviceStations = await _serviceStationRepository.GetAllServiceStationsAsync();
            List<ReturnServiceDTO> returnServiceDTOs = new List<ReturnServiceDTO>();
            foreach (ServiceStation serviceStation in serviceStations)
            {
                ReturnServiceDTO returnServiceDTO = new ReturnServiceDTO();
                returnServiceDTO.Id = serviceStation.Id;
                returnServiceDTO.Name = serviceStation.Name;
                returnServiceDTO.Address = serviceStation.Address;
                returnServiceDTO.City = serviceStation.City;
                returnServiceDTO.Latitude = serviceStation.Latitude;
                returnServiceDTO.Longitude = serviceStation.Longitude;
                returnServiceDTOs.Add(returnServiceDTO);
            }
            return returnServiceDTOs;
        }
    }
}
