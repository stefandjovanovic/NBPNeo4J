namespace NBPNeo4J.Models
{
    public class Hub
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public List<HubHasPart<Part>> Parts { get; set; } = new();
        public List<HubConnectedToHub<Hub>>? ConnectedHubs { get; set; } = new();
    }
}
