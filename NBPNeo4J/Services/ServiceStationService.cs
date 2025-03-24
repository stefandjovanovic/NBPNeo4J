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

        Task ConnectServiceToHub(string serviceId, string hubId);

        Task<Vehicle> AddVehicleToServiceStationAsync(string serviceStationId, Vehicle vehicle, List<String> partsIds, DateTime date);
        Task<Vehicle> RemoveVehicleFromServiceStationAsync(string serviceStationId, string vehicleId);
        Task<List<Vehicle>> GetAllVehicles(string serviceId);

    }
    public class ServiceStationService : IServiceStationService
    {
        private readonly IServiceStationRepository _serviceStationRepository;
        private readonly IHubRepository _hubRepository;

        public ServiceStationService(IServiceStationRepository serviceStationRepository, IHubRepository hubRepository)
        {
            _serviceStationRepository = serviceStationRepository;
            _hubRepository = hubRepository;
        }

        public async Task<ReturnServiceDTO> CreateService(CreateServiceDTO serviceDto)
        {
            var service = new ServiceStation
            {
                Id = Guid.NewGuid().ToString(),
                Name = serviceDto.Name,
                Address = serviceDto.Address,
                City = serviceDto.City,
                Latitude = serviceDto.Latitude,
                Longitude = serviceDto.Longitude
            };

            
            await _serviceStationRepository.CreateServiceStationAsync(service);

            if (!string.IsNullOrEmpty(serviceDto.HubId))
            {
                Hub connectedHub = await _hubRepository.GetHubAsync(serviceDto.HubId);
                if (connectedHub == null)
                {
                    throw new Exception("Hub not found");
                }

                try
                {
                    double distance = CalculateDistance(service, connectedHub);

                    
                    await _serviceStationRepository.ConnectServiceToHub(service.Id, connectedHub.Id, distance);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Service created, but failed to connect to hub: {ex.Message}");
                }
            }

            return new ReturnServiceDTO
            {
                Id = service.Id,
                Name = service.Name,
                Address = service.Address,
                City = service.City,
                Latitude = service.Latitude,
                Longitude = service.Longitude
            };
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

        public async Task ConnectServiceToHub(string serviceId, string hubId)
        {
            ServiceStation service = await _serviceStationRepository.GetServiceStationAsync(serviceId);
            if (service == null ) {
                throw new Exception("Service station not found");
            }

            Hub connectedHub = await _hubRepository.GetHubAsync(hubId);
            if (connectedHub == null)
            {
                throw new Exception("Hub not found");
            }

            double distance = CalculateDistance(service, connectedHub);

            if(service.ConnectedHub != null)
            {
                //Moze da se napravi da automatski predje na drugi hub, disconnect pa connect
                throw new Exception("Service station already connected to a hub");
            }

            await _serviceStationRepository.ConnectServiceToHub(serviceId, hubId, distance);

        }

        private double CalculateDistance(ServiceStation station, Hub connectedHub)
        {
            double R = 6371; // Radius of the earth in km
            double convertDegreesToRadians = Math.PI / 180;
            double dLat = convertDegreesToRadians * (connectedHub.Latitude - station.Latitude);
            double dLon = convertDegreesToRadians * (connectedHub.Longitude - station.Longitude);
            double a =
                Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(convertDegreesToRadians * station.Latitude) * Math.Cos(convertDegreesToRadians * connectedHub.Latitude) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distance = R * c; // Distance in km
            return distance;
        }

        public async Task<Vehicle> AddVehicleToServiceStationAsync(string serviceStationId, Vehicle vehicle, List<String> partsIds, DateTime date)
        {
            ServiceStation serviceStation = await _serviceStationRepository.GetServiceStationAsync(serviceStationId);
            if (serviceStation == null)
            {
                throw new Exception("Service station not found");
            }
            vehicle.Id = Guid.NewGuid().ToString();
            
            await _serviceStationRepository.AddVehicleToServiceStationAsync(serviceStationId, vehicle, partsIds, date);
            return vehicle;
        }

        public async Task<Vehicle> RemoveVehicleFromServiceStationAsync(string serviceStationId, string vehicleId)
        {
            ServiceStation serviceStation = await _serviceStationRepository.GetServiceStationAsync(serviceStationId);
            if (serviceStation == null)
            {
                throw new Exception("Service station not found");
            }
            Vehicle vehicle = await _serviceStationRepository.RemoveVehicleFromServiceStationAsync(serviceStationId, vehicleId);
            return vehicle;

        }
            
        public async Task<List<Vehicle>> GetAllVehicles(string serviceId)
        {
            ServiceStation serviceStation = await _serviceStationRepository.GetServiceStationAsync(serviceId);
            if (serviceStation == null)
            {
                throw new Exception("Service station not found");
            }
            List<Vehicle> vehicles = await _serviceStationRepository.GetVehiclesOnServiceStationAsync(serviceId);
            return vehicles;
        }
    }
}
