namespace NBPNeo4J.DTOs
{
    public class ReturnServiceDTO
    {
        public ReturnServiceDTO() { }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string ConnectedToHubId { get; set; }
        public double HubDistance { get; set; }
        
        

    }
}
