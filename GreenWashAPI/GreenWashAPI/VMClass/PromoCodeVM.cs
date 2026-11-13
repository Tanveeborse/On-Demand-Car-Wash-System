namespace GreenWashAPI.VMClass
{
    public class PromoCodeVM
    {
        public int PromoCodeId { get; set; }

        public string Code { get; set; } = null!;

        public string? Description { get; set; }

        public decimal DiscountPercentage { get; set; }

        public DateTime ExpiryDate { get; set; }

        public bool? IsActive { get; set; }
    }
}
