namespace NBPNeo4J.Models
{
    public class Relationship<T>
    {
        public T Target { get; set; }
        public string Type { get; }

        public Relationship(T target, string type)
        {
            Target = target;
            Type = type;
        }
    }
}
