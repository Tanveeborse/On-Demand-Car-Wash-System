using System;
using System.Collections.Generic;

namespace GreenWashAPI.Models;

public partial class OrderAddOn
{
    public int OrderAddonId { get; set; }

    public int? OrderId { get; set; }

    public int? AddonId { get; set; }

    public virtual AddOn? Addon { get; set; }

    public virtual Order? Order { get; set; }
}
