namespace arts_core.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public string ProductId { get; set; } = string.Empty;
        public Product? Product { get; set; }
    }
}
