using static System.Runtime.InteropServices.JavaScript.JSType;

namespace arts_core.RequestModels
{
    public class CreateProduct
    {
        public string ProductName {  get; set; } 

        public int Category {  get; set; }

        public string? Description { get; set; } 

        public int Price {  get; set; }

        public int SalePrice { get; set; }

        public string? Unit {  get; set; }

        public string? ActiveDay {  get; set; }

        public ICollection<IFormFile>? Images { get; set; }

        public ICollection<object>? Variants {  get; set; } = new List<object>();
        public ICollection<string>? VariantsJSON { get; set; }

        public ICollection<VariantDetail>? VariantDetails { get; set; }= new List<VariantDetail>();

        public ICollection<string>? VariantDetailsJSON {  get; set; }
    }
}

