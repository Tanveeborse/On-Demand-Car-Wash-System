using GreenWashAPI.Interface;
using GreenWashAPI.Models;
using GreenWashAPI.VMClass;

namespace GreenWashAPI.Repository
{
    public class PaymentsRepo
    {
        private readonly GreenWashDbContext _context;
        public PaymentsRepo(GreenWashDbContext context)
        {
            _context = context;
        }
        public async Task UpdatePaymentStatusAsync(int orderId, string paymentStatus)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                throw new Exception("Order not found.");
            }
            order.PaymentStatus = paymentStatus;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }
    }
}

