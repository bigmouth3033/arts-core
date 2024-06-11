namespace arts_core.Models
{
    public class Exchange
    {
        public int Id { get; set; }
        public string OriginalOrderId { get; set; } = string.Empty;
        public Order? OriginalOrder { get; set; }
        public string NewOrderId { get; set; } = string.Empty;
        public Order? NewOrder { get; set; }
        public string ReasonExchange { get; set; } = string.Empty;
        public DateTime ExchangeDate { get; set; }
        public string Status { get; set; } = "Pending";
    }
}
