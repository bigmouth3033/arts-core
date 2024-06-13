namespace arts_core.Models
{
    public class Variant
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public string VariantImage { get; set; } = string.Empty;
        public int Quanity { get; set; }
        public float Price { get; set; }
        public float SalePrice { get; set; }
        public DateTime CreatedAt => DateTime.Now;
        public bool Active { get; set; } = true;

        public ICollection<Stock>? Stocks { get; set; }
    }
}
