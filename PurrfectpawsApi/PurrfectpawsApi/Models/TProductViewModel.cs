using System.ComponentModel.DataAnnotations;

namespace PurrfectpawsApi.Models
{
    public class TProductViewModel
    {
        public int ProductId { get; set; }
        [Required]
        public int ProductDetailsId { get; set; }
        public int SizeId { get; set; }
        public int LeadLengthId { get; set; }
        public int VariationId { get; set; }
        public int ProductQuantity { get; set; }
    }
}
