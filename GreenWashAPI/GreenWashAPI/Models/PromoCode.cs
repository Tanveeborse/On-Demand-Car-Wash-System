using System;
using System.Collections.Generic;

namespace GreenWashAPI.Models;

public partial class PromoCode
{
    public int PromoCodeId { get; set; }

    public string Code { get; set; } = null!;

    public string? Description { get; set; }

    public decimal DiscountPercentage { get; set; }

    public DateTime ExpiryDate { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
