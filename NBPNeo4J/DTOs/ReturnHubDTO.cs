namespace NBPNeo4J.DTOs
{
    public class ReturnHubDTO
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public List<ConnectedHubDto> ConnectedHubs { get; set; } = new();

    }

    public class  ConnectedHubDto
    {
        public string? Id { get; set; }
        //public string? Name { get; set; }
        //public string? Address { get; set; }
        //public string? City { get; set; }
        //public double Latitude { get; set; }
        //public double Longitude { get; set; }
        public double Distance { get; set; }
    }
}
