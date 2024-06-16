namespace arts_core.Models
{
    public class Exchange
    {
        public int Id { get; set; }
        public string? OriginalOrderId { get; set; }
        public Order? OriginalOrder { get; set; }
        public string? NewOrderId { get; set; }
        public Order? NewOrder { get; set; }
        public string? ReasonExchange { get; set; }
        public string? ResponseExchange { get; set; }
        public DateTime ExchangeDate { get; set; }
        public string Status { get; set; } = "Pending";
    }
}
