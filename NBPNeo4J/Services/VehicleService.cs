using NBPNeo4J.Models;
using NBPNeo4J.Repositories;
using NBPNeo4J.DTOs;

namespace NBPNeo4J.Services
{

    public interface IVehicleService
    {
        Task<Vehicle> CreateVehicleAsync(CreateVehicleDTO createVehicleDTO);
        Task<Vehicle> GetVehicleAsync(string id);
        Task<Vehicle> UpdateVehicleAsync(string vehicleId, CreateVehicleDTO createVehicleDTO);
        Task DeleteVehicleAsync(string id);
    }
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;

        public VehicleService(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task<Vehicle> CreateVehicleAsync(CreateVehicleDTO createVehicleDTO)
        {
            var vehicle = new Vehicle
            {
                Id = Guid.NewGuid().ToString(),
                Model = createVehicleDTO.Model,
                LicensePlateNumber = createVehicleDTO.LicensePlateNumber,
                MotorType = createVehicleDTO.MotorType,
                CubicCapacity = createVehicleDTO.CubicCapacity,
                Power = createVehicleDTO.Power
            };
            Vehicle createdVehicle = await _vehicleRepository.CreateVehicleAsync(vehicle);

            return createdVehicle;
        }

        public async Task<Vehicle> UpdateVehicleAsync(string vehicleId, CreateVehicleDTO createVehicleDTO)
        {
            var vehicle = new Vehicle
            {
                Id = vehicleId,
                Model = createVehicleDTO.Model,
                LicensePlateNumber = createVehicleDTO.LicensePlateNumber,
                MotorType = createVehicleDTO.MotorType,
                CubicCapacity = createVehicleDTO.CubicCapacity,
                Power = createVehicleDTO.Power
            };
            Vehicle updatedVehicle = await _vehicleRepository.UpdateVehicleAsync(vehicle);
            return updatedVehicle;
        }

        public async Task DeleteVehicleAsync(string id)
        {
            await _vehicleRepository.DeleteVehicleAsync(id);
        }

        public async Task<Vehicle> GetVehicleAsync(string id)
        {
            return await _vehicleRepository.GetVehicleAsync(id);
        }
    }
}
