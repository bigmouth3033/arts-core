namespace arts_core.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Fullname { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        public bool Verifired { get; set; } = false;        
        public DateTime CreatedAt => DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public bool Active { get; set; } = false ;
        public int RoleTypeId { get; set; }
        public Type? RoleType { get; set; }
        public int RestrictedTypeId { get; set; }
        public Type? RestrictedType { get; set; }
        public ICollection<Order>? Orders { get; set; }
        public ICollection<Message>? Messages { get; set; }

    }
}
