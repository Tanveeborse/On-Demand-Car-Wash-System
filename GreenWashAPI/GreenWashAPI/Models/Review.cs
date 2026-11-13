using System;
using System.Collections.Generic;

namespace GreenWashAPI.Models;

public partial class Review
{
    public int ReviewId { get; set; }

    public int? UserId { get; set; }

    public int? WasherId { get; set; }

    public int? OrderId { get; set; }

    public decimal Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Order? Order { get; set; }

    public virtual User? User { get; set; }

    public virtual User? Washer { get; set; }
}
