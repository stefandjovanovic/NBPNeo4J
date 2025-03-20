using NBPNeo4J.Models;
using Neo4j.Driver;

namespace NBPNeo4J.Repositories
{

    public interface IVehicleRepository
    {
        Task<Vehicle> CreateVehicleAsync(Vehicle vehicle);
        Task<Vehicle> GetVehicleAsync(string id);
        Task<Vehicle> UpdateVehicleAsync(Vehicle vehicle);
        Task DeleteVehicleAsync(string id);

    }

    public class VehicleRepository : IVehicleRepository
    {
        public readonly IDriver _driver;

        public VehicleRepository(IDriver driver)
        {
            _driver = driver;
        }

        public async Task<Vehicle> CreateVehicleAsync(Vehicle vehicle)
        {
            string query = @"
                CREATE (v:Vehicle { 
                    Id: $Id, 
                    Model: $Model, 
                    LicensePlateNumber: $LicensePlateNumber, 
                    MotorType: $MotorType, 
                    CubicCapacity: $CubicCapacity, 
                    Power: $Power 
                })
                RETURN v.Id AS Id, v.Model AS Model, v.LicensePlateNumber AS LicensePlateNumber, v.MotorType AS MotorType, v.CubicCapacity AS CubicCapacity, v.Power AS Power";
            var parameters = new
            {
                vehicle.Id,
                vehicle.Model,
                vehicle.LicensePlateNumber,
                vehicle.MotorType,
                vehicle.CubicCapacity,
                vehicle.Power
            };
            var (queryResults, _) = await _driver
                .ExecutableQuery(query)
                .WithParameters(parameters)
                .ExecuteAsync();
            return new Vehicle
            {
                Id = queryResults[0]["Id"].As<string>(),
                Model = queryResults[0]["Model"].As<string>(),
                LicensePlateNumber = queryResults[0]["LicensePlateNumber"].As<string>(),
                MotorType = queryResults[0]["MotorType"].As<string>(),
                CubicCapacity = queryResults[0]["CubicCapacity"].As<int>(),
                Power = queryResults[0]["Power"].As<int>()
            };
        }

        public async Task<Vehicle> GetVehicleAsync(string id)
        {
            string query = @"
                MATCH (v:Vehicle { Id: $Id })
                RETURN v.Id AS Id, v.Model AS Model, v.LicensePlateNumber AS LicensePlateNumber, v.MotorType AS MotorType, v.CubicCapacity AS CubicCapacity, v.Power AS Power";
            var parameters = new { Id = id };
            var (queryResults, _) = await _driver
                .ExecutableQuery(query)
                .WithParameters(parameters)
                .ExecuteAsync();
            return new Vehicle
            {
                Id = queryResults[0]["Id"].As<string>(),
                Model = queryResults[0]["Model"].As<string>(),
                LicensePlateNumber = queryResults[0]["LicensePlateNumber"].As<string>(),
                MotorType = queryResults[0]["MotorType"].As<string>(),
                CubicCapacity = queryResults[0]["CubicCapacity"].As<int>(),
                Power = queryResults[0]["Power"].As<int>()
            };
        }

        public async Task<Vehicle> UpdateVehicleAsync(Vehicle vehicle)
        {
            string query = @"
                MATCH (v:Vehicle { Id: $Id })
                SET v.Model = $Model, 
                    v.LicensePlateNumber = $LicensePlateNumber, 
                    v.MotorType = $MotorType, 
                    v.CubicCapacity = $CubicCapacity, 
                    v.Power = $Power
                RETURN v.Id AS Id, v.Model AS Model, v.LicensePlateNumber AS LicensePlateNumber, v.MotorType AS MotorType, v.CubicCapacity AS CubicCapacity, v.Power AS Power";
            var parameters = new
            {
                vehicle.Id,
                vehicle.Model,
                vehicle.LicensePlateNumber,
                vehicle.MotorType,
                vehicle.CubicCapacity,
                vehicle.Power
            };
            var (queryResults, _) = await _driver
                .ExecutableQuery(query)
                .WithParameters(parameters)
                .ExecuteAsync();
            return new Vehicle
            {
                Id = queryResults[0]["Id"].As<string>(),
                Model = queryResults[0]["Model"].As<string>(),
                LicensePlateNumber = queryResults[0]["LicensePlateNumber"].As<string>(),
                MotorType = queryResults[0]["MotorType"].As<string>(),
                CubicCapacity = queryResults[0]["CubicCapacity"].As<int>(),
                Power = queryResults[0]["Power"].As<int>()
            };
        }

        public async Task DeleteVehicleAsync(string id)
        {
            string query = @"
                MATCH (v:Vehicle { Id: $Id })
                DETACH DELETE v";
            var parameters = new { Id = id };
            await _driver
                .ExecutableQuery(query)
                .WithParameters(parameters)
                .ExecuteAsync();
        }


    }
}
