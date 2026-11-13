using GreenWashAPI.Models;

namespace GreenWashAPI.Interface
{
    public interface INotifications
    {
        Task<Notification> CreateNotificationAsync(Notification notification);
        // Retrieve a notification by its ID
        Task<Notification> GetNotificationByIdAsync(int notificationId);
       
        // Retrieve all notifications (e.g., for admin purposes)
        Task<IEnumerable<Notification>> GetAllNotificationsAsync();
        // Update a notification (e.g., mark as read)
        Task<Notification> UpdateNotificationAsync(Notification notification);
        // Delete a notification by its ID
        Task DeleteNotificationAsync(int notificationId);
    }
}
