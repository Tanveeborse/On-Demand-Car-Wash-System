using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GreenWashAPI.Models;

public partial class User
{
    [Column("user_id")]
    public int UserId { get; set; }

    [Column("fullname")]
    public string Fullname { get; set; } = null!;

    [Column("email_id")]
    public string EmailId { get; set; } = null!;

    [Column("password")]
    public string Password { get; set; } 
    [Column("phone_number")]
    public string PhoneNumber { get; set; } = null!;

    [Column("profile_picture")]
    public string? ProfilePicture { get; set; }

    [Column("address")]
    public string? Address { get; set; }
    [Column("rating")]
    public decimal? Rating { get; set; }

    [Column("role")]
    public string Role { get; set; } = null!;

    [Column("is_active")]
    public bool? IsActive { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();

    public virtual ICollection<Leaderboard> Leaderboards { get; set; } = new List<Leaderboard>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    [JsonIgnore]
    public virtual ICollection<Order> OrderUsers { get; set; } = new List<Order>();

    public virtual ICollection<Order> OrderWashers { get; set; } = new List<Order>();

    public virtual ICollection<Review> ReviewUsers { get; set; } = new List<Review>();

    public virtual ICollection<Review> ReviewWashers { get; set; } = new List<Review>();
}
