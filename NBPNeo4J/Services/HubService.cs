using NBPNeo4J.DTOs;
using NBPNeo4J.Models;
using NBPNeo4J.Repositories;

namespace NBPNeo4J.Services
{
    public interface IHubService
    {
        Task<ReturnHubDTO> CreateHub(CreateHubDTO createHubDTO);
        Task<ReturnHubDTO> UpdateHub(string hubId, CreateHubDTO createHubDTO);
        Task DeleteHub(string hubId);
        Task<List<ReturnHubDTO>> GetAllHubs();
    }

    public class HubService : IHubService
    {
        private readonly IHubRepository _hubRepository;

        public HubService(IHubRepository hubRepository)
        {
            _hubRepository = hubRepository;
        }

        public async Task<ReturnHubDTO> CreateHub(CreateHubDTO createHubDTO)
        {
            var hub = new Hub
            {
                Id = Guid.NewGuid().ToString(),
                Name = createHubDTO.Name,
                Address = createHubDTO.Address,
                City = createHubDTO.City,
                Latitude = createHubDTO.Latitude,
                Longitude = createHubDTO.Longitude
            };

            Hub createdHub = await _hubRepository.CreateHubAsync(hub);

            foreach (var connectedHubId in createHubDTO.ConnectedHubsIds)
            {
                Hub connectedHub = await _hubRepository.GetHubAsync(connectedHubId);
                var distance = CalculateDistance(hub, connectedHub);
                var hubConnectedToHub = new HubConnectedToHub<Hub>(connectedHub, distance);
                await _hubRepository.ConnectTwoHubs(hub.Id, connectedHubId, distance);
                if (createdHub.ConnectedHubs == null)
                {
                    createdHub.ConnectedHubs = new List<HubConnectedToHub<Hub>>();
                }
                createdHub.ConnectedHubs.Add(hubConnectedToHub);
            }

            ReturnHubDTO returnHubDTO = new ReturnHubDTO();
            returnHubDTO.Id = createdHub.Id;
            returnHubDTO.Name = createdHub.Name;
            returnHubDTO.Address = createdHub.Address;
            returnHubDTO.City = createdHub.City;
            returnHubDTO.Latitude = createdHub.Latitude;
            returnHubDTO.Longitude = createdHub.Longitude;
            foreach (var connectedHub in createdHub.ConnectedHubs)
            {
                returnHubDTO.ConnectedHubs.Add(new ConnectedHubDto
                {
                    Id = connectedHub.Target.Id,
                    Distance = connectedHub.Distance
                });
            }

            return returnHubDTO;
        }

        private double CalculateDistance(Hub hub, Hub connectedHub)
        {
            double R = 6371; // Radius of the earth in km
            double convertDegreesToRadians = Math.PI / 180;
            double dLat = convertDegreesToRadians * (connectedHub.Latitude - hub.Latitude);
            double dLon = convertDegreesToRadians * (connectedHub.Longitude - hub.Longitude);
            double a =
                Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(convertDegreesToRadians * hub.Latitude) * Math.Cos(convertDegreesToRadians * connectedHub.Latitude) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distance = R * c; // Distance in km
            return distance;
        }

        public async Task DeleteHub(string hubId)
        {
            await _hubRepository.DeleteHubAsync(hubId);
        }

        public async Task<List<ReturnHubDTO>> GetAllHubs()
        {
            List<Hub> hubs = await _hubRepository.GetAllHubsAsync();
            List<ReturnHubDTO> returnHubs = new();
            foreach (Hub hub in hubs)
            {
                ReturnHubDTO returnHub = new ReturnHubDTO
                {
                    Id = hub.Id,
                    Name = hub.Name,
                    Address = hub.Address,
                    City = hub.City,
                    Latitude = hub.Latitude,
                    Longitude = hub.Longitude
                };
                if (hub.ConnectedHubs != null)
                {
                    foreach (var connectedHub in hub.ConnectedHubs)
                    {
                        returnHub.ConnectedHubs.Add(new ConnectedHubDto
                        {
                            Id = connectedHub.Target.Id,
                            Distance = connectedHub.Distance
                        });
                    }
                }
                returnHubs.Add(returnHub);
            }
            return returnHubs;
        }

        public async Task<ReturnHubDTO> UpdateHub(string hubId, CreateHubDTO createHubDTO)
        {
            var hub = new Hub
            {
                Id = hubId,
                Name = createHubDTO.Name,
                Address = createHubDTO.Address,
                City = createHubDTO.City,
                Latitude = createHubDTO.Latitude,
                Longitude = createHubDTO.Longitude
            };
            //updates only information about hub
            Hub updatedHub = await _hubRepository.UpdateHubInformationAsync(hub);

            //updates connections between hubs
            //first deletes all connections

            foreach (var connectedHubId in createHubDTO.ConnectedHubsIds)
            {
                Hub connectedHub = await _hubRepository.GetHubAsync(connectedHubId);
                var distance = CalculateDistance(hub, connectedHub);
                var hubConnectedToHub = new HubConnectedToHub<Hub>(connectedHub, distance);
                await _hubRepository.ConnectTwoHubs(hub.Id, connectedHubId, distance);
                if (updatedHub.ConnectedHubs == null)
                {
                    updatedHub.ConnectedHubs = new List<HubConnectedToHub<Hub>>();
                }
                updatedHub.ConnectedHubs.Add(hubConnectedToHub);
            }

            ReturnHubDTO returnHubDTO = new ReturnHubDTO();
            returnHubDTO.Id = updatedHub.Id;
            returnHubDTO.Name = updatedHub.Name;
            returnHubDTO.Address = updatedHub.Address;
            returnHubDTO.City = updatedHub.City;
            returnHubDTO.Latitude = updatedHub.Latitude;
            returnHubDTO.Longitude = updatedHub.Longitude;
            foreach (var connectedHub in updatedHub.ConnectedHubs)
            {
                returnHubDTO.ConnectedHubs.Add(new ConnectedHubDto
                {
                    Id = connectedHub.Target.Id,
                    Distance = connectedHub.Distance
                });
            }

            return returnHubDTO;
        }


    }


}
