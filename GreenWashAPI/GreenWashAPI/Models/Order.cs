using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GreenWashAPI.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int? UserId { get; set; }

    public int? WasherId { get; set; }

    public int? CarId { get; set; }

    public int? PackageId { get; set; }

    public string Status { get; set; } = null!;

    public string PaymentStatus { get; set; } = null!;

    public string Location { get; set; } = null!;

    public DateTime? ScheduledDatetime { get; set; }

    public DateTime? ActualStartTime { get; set; }

    public DateTime? ActualEndTime { get; set; }

    public decimal TotalAmount { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Car? Car { get; set; }

    public virtual ICollection<OrderAddOn> OrderAddOns { get; set; } = new List<OrderAddOn>();

    public virtual Package? Package { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual User? User { get; set; }

    [JsonIgnore]
    public virtual User? Washer { get; set; }
}
