﻿namespace arts_core.Models
{
    public class VariantAttribute
    {
        public int Id { get; set; }
        public int VariantId { get; set; }
        public Variant? Variant { get; set; }
        public int AttributeTypeId { get; set; }
        public Type? AttributeType { get; set; }
    }
}
