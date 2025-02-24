using NBPNeo4J.Models;

namespace NBPNeo4J.DTOs
{
    public class AddVehicleToServiceStationDTO
    {
        public Vehicle Vehicle { get; set; }
        public List<Part> Parts { get; set; } = new();
        public DateTime Date { get; set; }
    }
}
