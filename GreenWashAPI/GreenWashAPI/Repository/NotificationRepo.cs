using GreenWashAPI.Interface;
using GreenWashAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GreenWashAPI.Repository
{
    public class NotificationRepo:INotifications
    {
        private readonly GreenWashDbContext _context;
        public NotificationRepo(GreenWashDbContext context)
        {
            _context = context;
        }
        // Create a new notification record.
        public async Task<Notification> CreateNotificationAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
            return notification;
        }
        // Retrieve a single notification by its ID.
        public async Task<Notification> GetNotificationByIdAsync(int notificationId)
        {
            return await _context.Notifications
                .FirstOrDefaultAsync(n => n.NotificationId == notificationId);
        }
    
        // Retrieve all notifications (useful for admin or reporting).
        public async Task<IEnumerable<Notification>> GetAllNotificationsAsync()
        {
            return await _context.Notifications
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }
        // Update a notification (e.g., marking it as read).
        public async Task<Notification> UpdateNotificationAsync(Notification notification)
        {
            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync();
            return notification;
        }
        // Delete a notification by its ID.
        public async Task DeleteNotificationAsync(int notificationId)
        {
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.NotificationId == notificationId);
            if (notification != null)
            {
                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();
            }
        }
    }
}
