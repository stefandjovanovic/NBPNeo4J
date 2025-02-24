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
        Task<List<Vehicle>> GetVehiclesOnServiceStationAsync(string id);
        //Kod dodavanja vozila na servis, potrebno je dodati i delove !!
        Task<Vehicle> AddVehicleToServiceStationAsync(string serviceStationId, Vehicle vehicle);
        Task<Vehicle> RemoveVehicleFromServiceStationAsync(string serviceStationId, string vehicleId);

    }
    public class ServiceStationRepository : IServiceStationRepository
    {
        private readonly IDriver _driver;
        
        public ServiceStationRepository(IDriver driver)
        {
            _driver = driver;
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
                RETURN s.Id AS Id, s.Name AS Name, s.Address AS Address, 
                       s.City AS City, s.Latitude AS Latitude, s.Longitude AS Longitude";

            var parameters = new { Id = id };

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
                DELETE s
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
                RETURN s.Id AS Id, s.Name AS Name, s.Address AS Address, 
                       s.City AS City, s.Latitude AS Latitude, s.Longitude AS Longitude";

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

            return serviceStations;
        }



        public async Task<List<Vehicle>> GetVehiclesOnServiceStationAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Vehicle> AddVehicleToServiceStationAsync(string serviceStationId, Vehicle vehicle)
        {
            throw new NotImplementedException();
        }

        public async Task<Vehicle> RemoveVehicleFromServiceStationAsync(string serviceStationId, string vehicleId)
        {
            throw new NotImplementedException();
        }

        
    }
}
