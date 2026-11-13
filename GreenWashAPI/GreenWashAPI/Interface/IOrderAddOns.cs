using GreenWashAPI.Models;

namespace GreenWashAPI.Interface
{
    public interface IOrderAddOns
    {
        Task<OrderAddOn> CreateOrderAddOnAsync(int orderId, int addonId);
        // Get an OrderAddOn record by its primary key
        Task<OrderAddOn> GetOrderAddOnByIdAsync(int orderAddOnId);
        // Get all OrderAddOns associated with a particular Order
       // Task<IEnumerable<OrderAddOn>> GetOrderAddOnsByOrderIdAsync(int orderId);
        // Get all OrderAddOns (if needed for admin/reporting)
        Task<IEnumerable<OrderAddOn>> GetAllOrderAddOnsAsync();
        // Update an existing OrderAddOn record

        Task<PaymentReceipt> GenerateReceiptAsync(int orderId);
        Task<decimal> CalculateFinalTotalAmountAsync(int orderId, string promoCode = null);
        Task<Invoice> GenerateInvoiceAsync(int orderId, string promoCode = null);
        Task<List<string>> GetAddOnsByOrderIdAsync(int orderId);
    }
}
