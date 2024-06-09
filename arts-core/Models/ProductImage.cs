namespace arts_core.Models
{
    public class ProductImage
    {
        public int Id { get; set; }
        public string ProductId { get; set; } = string.Empty;
        public Product? Product { get; set; }
        public string ImageName { get; set; } = string.Empty;
    }
}
