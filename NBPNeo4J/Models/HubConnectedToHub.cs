namespace NBPNeo4J.Models
{
    public class HubConnectedToHub<T> : Relationship<T>
    {
        public double Distance { get; set; }

        public HubConnectedToHub(T target, double distance) : base(target, "HUB_CONNECTED_TO_HUB")
        {
            Distance = distance;
        }
    }
}
