namespace NBPNeo4J.Models
{
    public class ServiceConnectedToHub<T> : Relationship<T>
    {
        public double Distance { get; set; }
        public ServiceConnectedToHub(T target, double distance) : base(target, "SERVICE_CONNECTED_TO_HUB")
        {
            Distance = distance;
        }
    }
}
