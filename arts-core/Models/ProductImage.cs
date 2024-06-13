namespace arts_core.Models
{
    public class ProductImage
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public string ImageName { get; set; } = string.Empty;
    }
}
