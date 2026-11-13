using GreenWashAPI.Interface;
using GreenWashAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GreenWashAPI.Repository
{   
        public class LeaderboardRepo : ILeaderboards
        {
            private readonly GreenWashDbContext _context;
            public LeaderboardRepo(GreenWashDbContext context)
            {
                _context = context;
            }

            /// <summary>
            /// Submits water saved for a washer. If a leaderboard record exists, adds to it;
            /// otherwise, creates a new record. Then, rankings are updated.
            /// </summary>
            public async Task<Leaderboard> SubmitWaterSavedAsync(int userId, decimal waterGallons)
            {
                // Retrieve the leaderboard record for the washer.
                var record = await _context.Leaderboards.FirstOrDefaultAsync(l => l.UserId == userId);
                if (record == null)
                {
                    record = new Leaderboard
                    {
                        UserId = userId,
                        WaterSavedGallons = waterGallons,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    await _context.Leaderboards.AddAsync(record);
                }
                else
                {
                    record.WaterSavedGallons += waterGallons;
                    record.UpdatedAt = DateTime.UtcNow;
                    _context.Leaderboards.Update(record);
                }
                await _context.SaveChangesAsync();
                // Update rankings for all records after submission.
                await UpdateRankingsAsync();
                return record;
            }

            /// <summary>
            /// Recalculates and updates the rankings for all leaderboard entries based on the water saved (descending order).
            /// </summary>
            public async Task UpdateRankingsAsync()
            {
                // Retrieve all leaderboard records ordered by water saved descending.
                var leaderboardList = await _context.Leaderboards
                    .OrderByDescending(l => l.WaterSavedGallons)
                    .ToListAsync();
                int rank = 1;
                foreach (var record in leaderboardList)
                {
                    record.Rank = rank;
                    record.UpdatedAt = DateTime.UtcNow;
                    rank++;
                }
                _context.Leaderboards.UpdateRange(leaderboardList);
                await _context.SaveChangesAsync();
            }

            /// <summary>
            /// Retrieves the top 3 leaderboard entries based on the rank.
            /// </summary>
            public async Task<IEnumerable<Leaderboard>> GetTop3Async()
            {
                return await _context.Leaderboards
                    .OrderBy(l => l.Rank)
                    .Take(3)
                    .ToListAsync();
            }
        }
    
}
