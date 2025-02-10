﻿namespace NBPNeo4J.DTOs
{
    public class CreateHubDTO
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public List<string> ConnectedHubsIds { get; set; } = new();

    }
}
