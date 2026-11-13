using GreenWashAPI.Models;
using GreenWashAPI.VMClass;

namespace GreenWashAPI.Interface
{
    public interface IOrders
    {
        Task<Order> CreateOrderAsync(Order order);
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<IEnumerable<Order>> GetOrdersByUserAsync(int userId);
        Task<IEnumerable<Order>> GetOrdersByWasherAsync(int washerId);
        Task<IEnumerable<Order>> GetOrdersByStatusAsync(string status);
        Task<Order> UpdateOrderAsync(Order order);
        Task AssignWasherToOrderAsync(int orderId, int washerId);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order> CancelOrderAsync(int orderId);
        Task<Order> RescheduleOrderAsync(int orderId, DateTime newSchedule, string newLocation);
        Task UpdatePaymentStatusAsync(int orderId, string paymentStatus);
        Task<Order> AssignRandomWasherAsync(int orderId);
       // Task<IEnumerable<Order>> GetCurrentOrdersByStatusForUserAsync(int userId);
        Task<IEnumerable<Order>> GetPastOrdersByStatusForUserAsync(int userId);
        Task<Order> AssignRandomWasherOnTimeoutAsync(int orderId);
        Task<IEnumerable<Order>> GetOrdersForAdminReportAsync(string orderNumber,string washerName,string type,string service,DateTime? startDate,DateTime? endDate);
        Task<IEnumerable<OrderDetails>> GetAllPendingOrdersAsync();
        Task<IEnumerable<OrderDetails>> GetCurrentOrdersByStatusForWasherAsync();
        Task<List<OrderHistoryDTO>> GetOrderHistoryAsync(int userId);
        Task UpdatePaymentStatus(int orderId);
        Task<IEnumerable<Order>> GetCurrentOrdersByStatusUserAsync(int userId);
        Task UpdateOrderStatusAsync(int orderId, string status);
        Task UpdateStartTimeAsync(int orderId);
        Task UpdateEndTimeAsync(int orderId);
        Task<IEnumerable<OrderDetails>> GetPastOrdersByStatusForWasherAsync(int userId);
        Task<IEnumerable<OrderDetails>> GetCurrentOrdersByStatusForWasherAsync(int userId);
    }
}
