namespace arts_core.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public float ShipFee { get; set; }
        public int PaymentTypeId { get; set; }
        public Type? PaymentType { get; set; }
        public int DeliveryTypeId { get; set; }
        public Type? DeliveryType { get; set; }
        public int PaymentStatusTypeId { get; set; }
        public Type? PaymentStatusType { get; set; }
        public bool isReturn { get; set; }
        public ICollection<Order>? Orders { get; set; }
        public ICollection<Refund>? Refunds { get; set; }
    }
}
