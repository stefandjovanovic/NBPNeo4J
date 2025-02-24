namespace NBPNeo4J.Models
{
    public class ServiceStation
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public ServiceConnectedToHub<Hub>? ConnectedHub { get; set; }
        public List<ServiceHasVehicle<Vehicle>>? Vehicles { get; set; } = new();
    }
}
