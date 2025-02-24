namespace NBPNeo4J.DTOs
{
    public class CreateVehicleDTO
    {
        public string? Model { get; set; }
        public string? LicensePlateNumber { get; set; }
        public string? MotorType { get; set; }
        public int? CubicCapacity { get; set; }
        public int? Power { get; set; }
    }
}
