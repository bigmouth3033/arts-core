namespace arts_core.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public int MyProperty { get; set; }
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public int OrderStatus { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt => DateTime.Now;
    }
}
