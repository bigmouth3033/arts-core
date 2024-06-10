namespace arts_core.Models
{
    public class Exchange
    {
        public int Id { get; set; }
        public int OriginalOrderId { get; set; }
        public Order? OriginalOrder { get; set; }
        public int NewOrderId { get; set; }
        public Order? NewOrder { get; set; }
    }
}
