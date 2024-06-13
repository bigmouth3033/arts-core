namespace arts_core.Models
{
    public class Product
    {
        public int Id { get; set; } 
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;        
        public DateTime CreatedAt =>  DateTime.Now;
        public DateTime ActiveDate { get; set; } = DateTime.Now;
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }
        public ICollection<Variant>? Variants { get; set; }
        public ICollection<ProductImage>? ProductImages { get; set; }

      
    }
}
