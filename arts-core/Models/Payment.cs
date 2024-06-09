namespace arts_core.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public float   ShipFee{ get; set; }        
        public int PaymentTypeId { get; set; }
        public Type? PaymentType { get; set; }
        public int DeliveryTypeId { get; set; }
        public Type? DeleveryType { get; set; }

    }
}
