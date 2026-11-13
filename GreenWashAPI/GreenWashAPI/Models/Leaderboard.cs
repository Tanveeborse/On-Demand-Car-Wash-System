using System;
using System.Collections.Generic;

namespace GreenWashAPI.Models;

public partial class Leaderboard
{
    public int LeaderboardId { get; set; }

    public int? UserId { get; set; }

    public decimal WaterSavedGallons { get; set; }

    public int Rank { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual User? User { get; set; }
}
