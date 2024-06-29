﻿namespace arts_core.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public int VariantId { get; set; }
        public Variant? Variant { get; set; }
        public int Quanity { get; set; }
        public int OrderStatusId { get; set; }
        public virtual Type? OrderStatusType { get; set; }
        public float? TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<Exchange>? Exchanges { get; set; } = null;
        public int PaymentId { get; set; }
        public Payment? Payment { get; set; }

        public bool IsCancel { get; set; } = false;

        public string? CancelReason {  get; set; }
    }
}
