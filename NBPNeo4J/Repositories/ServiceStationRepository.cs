using Neo4j.Driver;
using System.Runtime.CompilerServices;
using NBPNeo4J.Models;

namespace NBPNeo4J.Repositories
{

// Metode:
//  create servis
//  delete servis
//  update servis
//  vrati sve servise(id, naziv, koordinate, naziv nadleznog huba)
//  vrati sve automobile na datom servisu(podaci o automobilu, potrebni delovi, datum prijema)
//  Dodaj automobil na servis

//    Vrsi se pretraga za delom, ako se u nadleznom hubu ne nalazi dovoljna kolicina datog dela, 
//	pretrazuje se graf tako da se nadje najkraca putanja po dostavnim vezama izmedju hubova
//  Ukloni automobil sa servisa kad je zavrsen

    public interface IServiceStationRepository
    {
        Task<ServiceStation> CreateServiceStationAsync(ServiceStation serviceStation);
        Task<ServiceStation> GetServiceStationAsync(string id);
        Task<ServiceStation> UpdateServiceStationAsync(ServiceStation serviceStation);
        Task<bool> DeleteServiceStationAsync(string id);
        Task<List<ServiceStation>> GetAllServiceStationsAsync();
        Task ConnectServiceToHub(string serviceStationId, string hubId, double distance);
        Task DisconnectServiceFromHub(string serviceStationId);
        Task<List<Vehicle>> GetVehiclesOnServiceStationAsync(string serviceId);
        //Kod dodavanja vozila na servis, potrebno je dodati i delove !!
        Task<Vehicle> AddVehicleToServiceStationAsync(string serviceStationId, Vehicle vehicle, List<String> partsIds, DateTime date);
        Task<Vehicle> RemoveVehicleFromServiceStationAsync(string serviceStationId, string vehicleId);

    }
    public class ServiceStationRepository : IServiceStationRepository
    {
        private readonly IDriver _driver;
        private readonly IVehicleRepository _vehicleRepository;

        public ServiceStationRepository(IDriver driver, IVehicleRepository vehicleRepository)
        {
            _driver = driver;
            _vehicleRepository = vehicleRepository;
        }

        public async Task<ServiceStation> CreateServiceStationAsync(ServiceStation serviceStation)
        {
            string query = @"
                CREATE (s:ServiceStation { 
                    Id: $Id, 
                    Name: $Name, 
                    Address: $Address, 
                    City: $City, 
                    Latitude: $Latitude, 
                    Longitude: $Longitude 
                })
                RETURN s.Id AS Id, s.Name AS Name, s.Address AS Address, s.City AS City, s.Latitude AS Latitude, s.Longitude AS Longitude";

            var parameters = new
            {
                serviceStation.Id,
                serviceStation.Name,
                serviceStation.Address,
                serviceStation.City,
                serviceStation.Latitude,
                serviceStation.Longitude
            };

            var (queryResults, _) = await _driver
                .ExecutableQuery(query)
                .WithParameters(parameters)
                .ExecuteAsync();

            return new ServiceStation
            {
                Id = queryResults[0]["Id"].As<string>(),
                Name = queryResults[0]["Name"].As<string>(),
                Address = queryResults[0]["Address"].As<string>(),
                City = queryResults[0]["City"].As<string>(),
                Latitude = queryResults[0]["Latitude"].As<double>(),
                Longitude = queryResults[0]["Longitude"].As<double>()
            };
        }

        public async Task<ServiceStation?> GetServiceStationAsync(string id)
        {
            string query = @"
                MATCH (s:ServiceStation { Id: $Id })
                OPTIONAL MATCH (s)-[r:SERVICE_CONNECTED_TO_HUB]->(hub:Hub)
                RETURN s.Id AS Id, s.Name AS Name, s.Address AS Address, 
                       s.City AS City, s.Latitude AS Latitude, s.Longitude AS Longitude,
                        r AS Relationship, hub AS ConnectedToHub";

            var parameters = new { Id = id };

            var (queryResults, _) = await _driver
                .ExecutableQuery(query)
                .WithParameters(parameters)
                .ExecuteAsync();

            if (!queryResults.Any())
                return null;
            var relationship = queryResults[0]["Relationship"].As<IRelationship>();
            var connectedHubNode = queryResults[0]["ConnectedToHub"].As<INode>();

            Hub connectedHub = new Hub
            {
                Id = connectedHubNode["Id"].As<string>(),
                Name = connectedHubNode["Name"].As<string>(),
                Address = connectedHubNode["Address"].As<string>(),
                City = connectedHubNode["City"].As<string>(),
                Latitude = connectedHubNode["Latitude"].As<double>(),
                Longitude = connectedHubNode["Longitude"].As<double>()
            };


            return new ServiceStation
            {
                Id = queryResults[0]["Id"].As<string>(),
                Name = queryResults[0]["Name"].As<string>(),
                Address = queryResults[0]["Address"].As<string>(),
                City = queryResults[0]["City"].As<string>(),
                Latitude = queryResults[0]["Latitude"].As<double>(),
                Longitude = queryResults[0]["Longitude"].As<double>(),
                ConnectedHub = new ServiceConnectedToHub<Hub>(connectedHub, relationship["Distance"].As<double>())
            };
        }

        public async Task<ServiceStation?> UpdateServiceStationAsync(ServiceStation serviceStation)
        {
            string query = @"
                MATCH (s:ServiceStation { Id: $Id })
                SET s.Name = $Name, 
                    s.Address = $Address, 
                    s.City = $City, 
                    s.Latitude = $Latitude, 
                    s.Longitude = $Longitude
                RETURN s.Id AS Id, s.Name AS Name, s.Address AS Address, 
                       s.City AS City, s.Latitude AS Latitude, s.Longitude AS Longitude";

            var parameters = new
            {
                serviceStation.Id,
                serviceStation.Name,
                serviceStation.Address,
                serviceStation.City,
                serviceStation.Latitude,
                serviceStation.Longitude
            };

            var (queryResults, _) = await _driver
                .ExecutableQuery(query)
                .WithParameters(parameters)
                .ExecuteAsync();

            if (!queryResults.Any())
                return null;

            return new ServiceStation
            {
                Id = queryResults[0]["Id"].As<string>(),
                Name = queryResults[0]["Name"].As<string>(),
                Address = queryResults[0]["Address"].As<string>(),
                City = queryResults[0]["City"].As<string>(),
                Latitude = queryResults[0]["Latitude"].As<double>(),
                Longitude = queryResults[0]["Longitude"].As<double>()
            };
        }

        public async Task<bool> DeleteServiceStationAsync(string id)
        {
            string query = @"
                MATCH (s:ServiceStation { Id: $Id })
                DETACH DELETE s
                RETURN COUNT(s) AS DeletedCount";

            var parameters = new { Id = id };

            var (queryResults, _) = await _driver
                .ExecutableQuery(query)
                .WithParameters(parameters)
                .ExecuteAsync();

            return queryResults[0]["DeletedCount"].As<int>() > 0;
        }

        public async Task<List<ServiceStation>> GetAllServiceStationsAsync()
        {
            string query = @"
                MATCH (s:ServiceStation)
                OPTIONAL MATCH (s)-[r:SERVICE_CONNECTED_TO_HUB]->(hub:Hub)
                RETURN s.Id AS Id, s.Name AS Name, s.Address AS Address, 
                       s.City AS City, s.Latitude AS Latitude, s.Longitude AS Longitude,
                        r AS Relationship, hub as Hub";

            var (queryResults, _) = await _driver
                .ExecutableQuery(query)
                .ExecuteAsync();

            var serviceStations = queryResults.Select(record => new ServiceStation
            {
                Id = record["Id"].As<string>(),
                Name = record["Name"].As<string>(),
                Address = record["Address"].As<string>(),
                City = record["City"].As<string>(),
                Latitude = record["Latitude"].As<double>(),
                Longitude = record["Longitude"].As<double>()
            }).ToList();

            for(int i =0; i<queryResults.Count; i++)
            {
                var relationship = queryResults[i]["Relationship"].As<IRelationship>();
                var connectedHubNode = queryResults[i]["Hub"].As<INode>();

                Hub connectedHub = new Hub
                {
                    Id = connectedHubNode["Id"].As<string>(),
                    Name = connectedHubNode["Name"].As<string>(),
                    Address = connectedHubNode["Address"].As<string>(),
                    City = connectedHubNode["City"].As<string>(),
                    Latitude = connectedHubNode["Latitude"].As<double>(),
                    Longitude = connectedHubNode["Longitude"].As<double>()
                };

                serviceStations[i].ConnectedHub = new ServiceConnectedToHub<Hub>(connectedHub, relationship["Distance"].As<double>());
            }


            return serviceStations;
        }

        public async Task ConnectServiceToHub(string serviceStationId, string hubId, double distance)
        {
            string query = @"
                MATCH (s:ServiceStation { Id: $ServiceStationId }), (h:Hub { Id: $HubId })
                CREATE (s)-[:SERVICE_CONNECTED_TO_HUB { Distance: $Distance }]->(h)";

            var parameters = new
            {
                ServiceStationId = serviceStationId,
                HubId = hubId,
                Distance = distance
            };

            await _driver
                .ExecutableQuery(query)
                .WithParameters(parameters)
                .ExecuteAsync();
        }

        public async Task DisconnectServiceFromHub(string serviceStationId)
        {
            string query = @"
                MATCH (s:ServiceStation { Id: $ServiceStationId })-[r:SERVICE_CONNECTED_TO_HUB]->(h:Hub)
                DELETE r";
            var parameters = new { ServiceStationId = serviceStationId };
            await _driver
                .ExecutableQuery(query)
                .WithParameters(parameters)
                .ExecuteAsync();

        }


        public async Task<List<Vehicle>> GetVehiclesOnServiceStationAsync(string serviceId)
        {
            string query = @"
                MATCH (s:ServiceStation { Id: $ServiceId })-[:SERVICE_HAS_CAR]->(v:Vehicle)
                RETURN v.Id AS Id, v.Model AS Model, v.LicensePlateNumber AS LicensePlateNumber, 
                       v.MotorType AS MotorType, v.CubicCapacity AS CubicCapacity, v.Power AS Power";

            var parameters = new { ServiceId = serviceId };

            var (queryResults, _) = await _driver
                .ExecutableQuery(query)
                .WithParameters(parameters)
                .ExecuteAsync();

            return queryResults
                .Select(v => new Vehicle
                {
                    Id = v["Id"].As<string>(),
                    Model = v["Model"].As<string>(),
                    LicensePlateNumber = v["LicensePlateNumber"].As<string>(),
                    MotorType = v["MotorType"].As<string>(),
                    CubicCapacity = v["CubicCapacity"].As<int?>(),
                    Power = v["Power"].As<int?>()
                })
                .ToList();
        }

        public async Task<Vehicle> AddVehicleToServiceStationAsync(string serviceStationId, Vehicle vehicle, List<String> partsIds, DateTime date)
        {            
            Vehicle createdVehicle = await _vehicleRepository.CreateVehicleAsync(vehicle);

            string query = @"
                MATCH (s:ServiceStation { Id: $ServiceStationId }), (v:Vehicle { Id: $VehicleId })
                CREATE (s)-[:SERVICE_HAS_CAR { 
                    Parts: $Parts, 
                    Date: $Date 
                }]->(v)";

            var parameters = new
            {
                ServiceStationId = serviceStationId,
                VehicleId = createdVehicle.Id,
                Parts = partsIds,
                Date = date.ToString("o") 
            };

            await _driver
                .ExecutableQuery(query)
                .WithParameters(parameters)
                .ExecuteAsync();

            return createdVehicle;
        }


        public async Task<Vehicle> RemoveVehicleFromServiceStationAsync(string serviceStationId, string vehicleId)
        {
            
            string query = @"
                MATCH (s:ServiceStation { Id: $ServiceStationId })-[r:SERVICE_HAS_CAR]->(v:Vehicle { Id: $VehicleId })
                DELETE r
                RETURN v.Id AS Id, v.Model AS Model, v.LicensePlateNumber AS LicensePlateNumber, 
                       v.MotorType AS MotorType, v.CubicCapacity AS CubicCapacity, v.Power AS Power";

            var parameters = new
            {
                ServiceStationId = serviceStationId,
                VehicleId = vehicleId
            };

            var (queryResults, _) = await _driver
                .ExecutableQuery(query)
                .WithParameters(parameters)
                .ExecuteAsync();

            if (queryResults.Count == 0)
            {
                throw new Exception("Vehicle not found or not linked to the service station.");
            }

            
            return new Vehicle
            {
                Id = queryResults[0]["Id"].As<string>(),
                Model = queryResults[0]["Model"].As<string>(),
                LicensePlateNumber = queryResults[0]["LicensePlateNumber"].As<string>(),
                MotorType = queryResults[0]["MotorType"].As<string>(),
                CubicCapacity = queryResults[0]["CubicCapacity"].As<int?>(),
                Power = queryResults[0]["Power"].As<int?>()
            };
        }



    }
}
