using System;
using System.Collections.Generic;

namespace GreenWashAPI.Models;

public partial class AddOn
{
    public int AddonId { get; set; }

    public string AddonName { get; set; } = null!;

    public string? AddonDescription { get; set; }

    public decimal Price { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<OrderAddOn> OrderAddOns { get; set; } = new List<OrderAddOn>();
}
