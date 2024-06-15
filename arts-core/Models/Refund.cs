﻿namespace arts_core.Models
{
    public class Refund
    {
        public int Id { get; set; }
        public string? OrderId { get; set; } 
        public Order? Order { get; set; }
        public int PaymentId { get; set; }
        public Payment? Payment { get; set; }
        public string? ReasonRefund { get; set; }
        public string? ResponseRefund { get; set; }
    }
}
