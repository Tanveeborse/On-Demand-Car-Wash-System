using GreenWashAPI.Interface;
using GreenWashAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace GreenWashAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
   // [Authorize]
    public class LeaderboardsController : ControllerBase
    {
        private readonly ILeaderboards _leaderboardRepo;
        public LeaderboardsController(ILeaderboards leaderboardRepo)
        {
            _leaderboardRepo = leaderboardRepo;
        }
       
        [HttpPost("submit")]
        // [Authorize(Roles = "Washer,Admin")]
        public async Task<IActionResult> SubmitWaterSaved([FromQuery] int userId, [FromQuery] decimal waterGallons)
        {
            try
            {
                var record = await _leaderboardRepo.SubmitWaterSavedAsync(userId, waterGallons);
                return Ok(record);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
       
        [HttpGet("top3")]
        //[Authorize(Roles = "Admin,Washer")]
        public async Task<IActionResult> GetTop3()
        {
            try
            {
                IEnumerable<Leaderboard> top3 = await _leaderboardRepo.GetTop3Async();
                return Ok(top3);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
