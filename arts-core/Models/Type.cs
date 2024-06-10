namespace arts_core.Models
{
    public class Type
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string NameType { get; set; } = string.Empty;
        public ICollection<User>? Users { get; set; }
        public ICollection<Payment>? Payments { get; set; }
        public ICollection<Event>? Events { get; set; }
    }
}
