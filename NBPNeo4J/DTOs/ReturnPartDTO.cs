namespace NBPNeo4J.DTOs
{
    public class ReturnPartDTO
    {
        //public string? Id { get; set; }
        public string? SerialCode { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public double Price { get; set; }
        public PartCategoryDTO PartCategory { get; set; } = new();
        public string? PartCategoryId { get; set; }
    }
    
}
