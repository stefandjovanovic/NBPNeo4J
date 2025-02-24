namespace NBPNeo4J.Models
{
    public class ServiceHasVehicle<T> : Relationship<T>
    {
        public List<Part> Parts { get; set; } = new();
        public DateTime Date { get; set; }
        public Hub Hub { get; set; }
        public ServiceHasVehicle(T target, List<Part> parts, DateTime date, Hub hub) : base(target, "SERVICE_HAS_CAR")
        {
            Parts = parts;
            Date = date;
            Hub = hub;
        }
    }
}
