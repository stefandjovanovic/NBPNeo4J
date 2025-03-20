namespace NBPNeo4J.DTOs
{
    public class CreatePartDTO
    {
        public string? SerialCode { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public double Price { get; set; }
        public string? PartCategoryId { get; set; }
    }
}
