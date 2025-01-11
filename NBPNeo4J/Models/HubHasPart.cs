namespace NBPNeo4J.Models
{
    public class HubHasPart<T> : Relationship<T>
    {
        public int Quantity { get; set; }

        public HubHasPart(T target, int quantity) : base(target, "HUB_HAS_PART")
        {
            Quantity = quantity;
        }
    }
}
