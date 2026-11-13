using System;
using System.Collections.Generic;

namespace GreenWashAPI.Models;

public partial class Car
{
    public int CarId { get; set; }

    public int? UserId { get; set; }

    public string Carmake { get; set; } = null!;

    public string Carmodel { get; set; } = null!;

    public string? Carimage { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual User? User { get; set; }
}
