namespace NBPNeo4J.Models
{
    public class PartBelongsToCategory<T> : Relationship<T>
    {
        public PartBelongsToCategory(T target) : base(target, "PART_BELONGS_TO_CATEGORY")
        {

        }
    }
}
