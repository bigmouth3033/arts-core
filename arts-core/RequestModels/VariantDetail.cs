namespace arts_core.RequestModels
{
    public class VariantDetail
    {
        public int SellPrice { get; set; }

        public int ComparePrice { get; set; }

        public int? Image {  get; set; }

        public int Inventory {  get; set; }

        public float BeginFund { get; set; }

        public IEnumerable<string>? Variant {  get; set; }
    }
}
