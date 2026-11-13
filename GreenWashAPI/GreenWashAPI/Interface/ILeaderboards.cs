using GreenWashAPI.Models;

namespace GreenWashAPI.Interface
{
    public interface ILeaderboards
    {
        Task<Leaderboard> SubmitWaterSavedAsync(int userId, decimal waterGallons);
        Task UpdateRankingsAsync();
        Task<IEnumerable<Leaderboard>> GetTop3Async();
    }
}
