using System;
using System.Collections.Generic;

namespace GreenWashAPI.Models;

public  class Package
{
    public int PackageId { get; set; }

    public string PackageName { get; set; } 

    public string? PackageDescription { get; set; }

    public decimal Price { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
