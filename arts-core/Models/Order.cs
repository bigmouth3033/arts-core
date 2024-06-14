namespace arts_core.Models
{
    public class Order
    {
        public string? Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public int MyProperty { get; set; }
        public string? Address { get; set; } 
        public string? PhoneNumber { get; set; }
        public int OrderStatus { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Exchange>? Exchanges { get; set; } = null;
        public int PaymentId { get; set; }
        public Payment? Payment { get; set; }        
    }
}
