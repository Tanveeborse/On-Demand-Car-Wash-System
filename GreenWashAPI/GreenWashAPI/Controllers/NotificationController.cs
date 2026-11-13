using GreenWashAPI.Interface;
using GreenWashAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenWashAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly INotifications _notificationRepo;
        public NotificationsController(INotifications notificationRepo)
        {
            _notificationRepo = notificationRepo;
        }
        // POST: api/notifications
        [HttpPost]
        public async Task<IActionResult> CreateNotification([FromBody] Notification notification)
        {
            try
            {
                var createdNotification = await _notificationRepo.CreateNotificationAsync(notification);
                return CreatedAtAction(nameof(GetNotificationById), new { notificationId = createdNotification.NotificationId }, createdNotification);
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                return BadRequest(new { error = ex.Message });
            }
        }
        // GET: api/notifications/{notificationId}
        [HttpGet("{notificationId}")]
        public async Task<IActionResult> GetNotificationById(int notificationId)
        {
            try
            {
                var notification = await _notificationRepo.GetNotificationByIdAsync(notificationId);
                if (notification == null)
                    return NotFound();
                return Ok(notification);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // GET: api/notifications
        [HttpGet]
       // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllNotifications()
        {
            try
            {
                IEnumerable<Notification> notifications = await _notificationRepo.GetAllNotificationsAsync();
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        // PUT: api/notifications/{notificationId}
        [HttpPut("{notificationId}")]
        public async Task<IActionResult> UpdateNotification(int notificationId, [FromBody] Notification notification)
        {
            try
            {
                if (notificationId != notification.NotificationId)
                    return BadRequest("Notification ID mismatch.");
                var updatedNotification = await _notificationRepo.UpdateNotificationAsync(notification);
                return Ok(updatedNotification);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        // DELETE: api/notifications/{notificationId}
        [HttpDelete("{notificationId}")]
        public async Task<IActionResult> DeleteNotification(int notificationId)
        {
            try
            {
                await _notificationRepo.DeleteNotificationAsync(notificationId);
                return Ok(new { message = "Notification deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
