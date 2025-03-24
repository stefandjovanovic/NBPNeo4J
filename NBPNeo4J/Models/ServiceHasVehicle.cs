namespace NBPNeo4J.Models
{
    public class ServiceHasVehicle<T> : Relationship<T>
    {
        public List<String> PartsIds { get; set; } = new();
        public DateTime Date { get; set; }
       // public Hub Hub { get; set; }
        public ServiceHasVehicle(T target, List<String> partsIds, DateTime date, Hub hub) : base(target, "SERVICE_HAS_CAR")
        {
            PartsIds = partsIds;
            Date = date;
           // Hub = hub;
        }
    }
}
