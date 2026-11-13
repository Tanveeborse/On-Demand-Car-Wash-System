using GreenWashAPI.Interface;
using GreenWashAPI.Models;
using GreenWashAPI.VMClass;
using Microsoft.EntityFrameworkCore;

namespace GreenWashAPI.Repository
{
    public class OrderAddOnRepo : IOrderAddOns
    {
        private readonly GreenWashDbContext _context;
        public OrderAddOnRepo(GreenWashDbContext context)
        {
            _context = context;
        }

        public OrderRepo _orderRepo = null;
        

        public OrderRepo OrderRepo { get => _orderRepo; set => _orderRepo = value; }

        // Create a new OrderAddOn record
        //public async Task<OrderAddOn> CreateOrderAddOnAsync(OrderAddOn orderAddOn)
        //{
        //    await _context.OrderAddOns.AddAsync(orderAddOn);
        //    await _context.SaveChangesAsync();
        //    return orderAddOn;
        //}

        public async Task<OrderAddOn> CreateOrderAddOnAsync(int orderId, int addonId)
        {
            // Create the OrderAddOn instance
            var orderAddOn = new OrderAddOn
            {
                OrderId = orderId,
                AddonId = addonId
            };
            // Add the OrderAddOn to the database
            await _context.OrderAddOns.AddAsync(orderAddOn);
            await _context.SaveChangesAsync();
            // Return the created OrderAddOn object
            return orderAddOn;
        }
        // Get an OrderAddOn record by its primary key
        public async Task<OrderAddOn> GetOrderAddOnByIdAsync(int orderAddOnId)
        {
            var orderAddOn = await _context.OrderAddOns
                .Where(oa => oa.OrderAddonId == orderAddOnId)
                .Select(oa => new OrderAddOn
                {
                    OrderAddonId = oa.OrderAddonId,
                    OrderId = oa.OrderId,
                    AddonId = oa.AddonId,
                    Addon = new AddOn
                    {
                        AddonId = oa.Addon.AddonId,
                        AddonName = oa.Addon.AddonName  // only load necessary fields from Addon
                    },
                    Order = new Order
                    {
                        OrderId = oa.Order.OrderId
                    }
                })
                .FirstOrDefaultAsync();

            return orderAddOn;
        }

        // Get all OrderAddOns associated with a particular Order
        //public async Task<IEnumerable<OrderAddOn>> GetOrderAddOnsByOrderIdAsync(int orderId)
        //{
        //    // Fetch the OrderAddOns for the specified orderId
        //    var orderAddOns = await _context.OrderAddOns
        //        .Where(oa => oa.OrderId == orderId)
        //        .Select(oa => new OrderAddOn
        //        {
        //            OrderAddonId = oa.OrderAddonId,
        //            OrderId = oa.OrderId,
        //            AddonId = oa.AddonId,
        //            // Explicitly load necessary fields from Addon
        //            Addon = new AddOn
        //            {
        //                AddonId = oa.Addon.AddonId,
        //                AddonName = oa.Addon.AddonName  // Select only the fields you need from Addon
        //            }
        //        })
        //        .ToListAsync();

        //    return orderAddOns;
        //}

        //// Get all OrderAddOns (if needed for admin/reporting)
        public async Task<IEnumerable<OrderAddOn>> GetAllOrderAddOnsAsync()
        {
            return await _context.OrderAddOns
                .Include(oa => oa.Addon)  // include Addon, but avoid deep loading the entire Addon entity
                .Include(oa => oa.Order)  // include Order, but avoid deep loading the entire Order entity
                .Select(oa => new OrderAddOn
                {
                    OrderAddonId = oa.OrderAddonId,
                    OrderId = oa.OrderId,
                    AddonId = oa.AddonId,
                    // Here, we limit the fields loaded from related entities to prevent circular references
                    Addon = new AddOn
                    {
                        AddonId = oa.Addon.AddonId,
                        AddonName = oa.Addon.AddonName // Only select the necessary fields, not the entire Addon object
                    },
                    Order = new Order
                    {
                        OrderId = oa.Order.OrderId
                         // Only select the necessary fields, not the entire Order object
                    }
                })
                .ToListAsync();
        }

        public async Task<decimal> CalculateFinalTotalAmountAsync(int orderId, string promoCode = null)
        {
            // Retrieve the order without a promo code field.
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
            if (order == null)
                throw new Exception("Order not found.");
            // Assume the order's base amount (e.g., package price) is stored in TotalAmount
            decimal baseAmount = order.TotalAmount;
            // Sum up the prices of all add-ons associated with this order.
            // This assumes that the OrderAddOn join entity has a navigation property 'Addon'
            // with a Price property.
            decimal addOnTotal = await _context.OrderAddOns
                .Where(oa => oa.OrderId == orderId)
                .Include(oa => oa.Addon)
                .SumAsync(oa => oa.Addon.Price);
            // Total before discount is the sum of base amount and add-on total.
            decimal totalBeforeDiscount = baseAmount + addOnTotal;
            decimal finalAmount = totalBeforeDiscount;
            // If a promo code is provided, retrieve its details from the PromoCodes table.
            if (!string.IsNullOrEmpty(promoCode))
            {
                var promo = await _context.PromoCodes
                    .FirstOrDefaultAsync(p => p.Code == promoCode &&
                                              p.IsActive == true &&
                                              p.ExpiryDate > DateTime.UtcNow);
                if (promo != null)
                {
                    // Apply discount percentage from promo code on the totalBeforeDiscount
                    finalAmount = totalBeforeDiscount - (totalBeforeDiscount * promo.DiscountPercentage / 100);
                }
            }
            // Update the order's TotalAmount with the final calculated value.
            // (You might want to decide whether to update the order in this method or only calculate the value.)
            order.TotalAmount = finalAmount;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return finalAmount;
        }

        public async Task<Invoice> GenerateInvoiceAsync(int orderId, string promoCode = null)
        {
            var order = await _context.Orders
                .Include(o => o.Car)
                .Include(o => o.Package)
                .Include(o => o.OrderAddOns)
                    .ThenInclude(oa => oa.Addon)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
            if (order == null)
                throw new Exception("Order not found for invoice generation.");


            string packageDetails = order.Package != null
                ? $"{order.Package.PackageName} - {order.Package.PackageDescription}"
                : "No package selected";

            string addOnDetails = (order.OrderAddOns != null && order.OrderAddOns.Any(oa => oa.Addon != null))
        ? string.Join(", ", order.OrderAddOns
            .Where(oa => oa.Addon != null && !string.IsNullOrEmpty(oa.Addon.AddonName))  // Ensure valid Addons
            .Select(oa => oa.Addon.AddonName)) 
        : "No add-ons selected";

            // Create the Invoice record.
            var invoice = new Invoice
            {
                OrderId = order.OrderId,
                InvoiceNumber = $"INV-{order.OrderId}-{DateTime.UtcNow.Ticks}",
                Carmake = order.Car.Carmake,
                PackageDetails = packageDetails,
                AddOnDetails = addOnDetails,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                CreatedAt = DateTime.UtcNow
            };
           
            return invoice;
        }

        public async Task<PaymentReceipt> GenerateReceiptAsync(int orderId)
        {
            // Retrieve the order along with its related Car, Package, and OrderAddOns (with AddOn details)
            var order = await _context.Orders
                .Include(o => o.Car)
                .Include(o => o.Package)
                .Include(o => o.OrderAddOns)
                    .ThenInclude(oa => oa.Addon)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
            if (order == null)
                throw new Exception("Order not found.");
            // Check that payment is completed (e.g., payment_status is 'Paid')
            if (!string.Equals(order.PaymentStatus, "Paid", StringComparison.OrdinalIgnoreCase))
                throw new Exception("Payment is not completed for this order.");
            // Build package details string from the Package entity.
            string packageDetails = order.Package != null
                ? $"{order.Package.PackageName} - {order.Package.PackageDescription}"
                : "No package selected";
            // Build add-on details by concatenating the names of all selected add-ons.
            string addOnDetails = (order.OrderAddOns != null && order.OrderAddOns.Any())
                ? string.Join(", ", order.OrderAddOns
                                      .Where(oa => oa.Addon != null && !string.IsNullOrEmpty(oa.Addon.AddonName))
                                      .Select(oa => oa.Addon.AddonName))
                : "No add-ons selected";
            // Get the car make from the related Car entity.
            string carMake = order.Car != null ? order.Car.Carmake : "No car information";
            // Create the PaymentReceipt record.
            var receipt = new PaymentReceipt
            {
                OrderId = order.OrderId,
                ReceiptNumber = $"RCPT-{order.OrderId}-{DateTime.UtcNow.Ticks}",
                CarMake = carMake,
                PackageDetails = packageDetails,
                AddOnDetails = addOnDetails,
                TotalAmount = order.TotalAmount,
                CreatedAt = DateTime.UtcNow
            };
           
            return receipt;
        }

        public async Task<List<string>> GetAddOnsByOrderIdAsync(int orderId)
        {
            // Use the DbContext to retrieve the add-ons for the given order
            var addOnNames = await _context.OrderAddOns
                                            .Where(oao => oao.OrderId == orderId)
                                            .Include(oao => oao.Addon) // Include AddOn details
                                            .Select(oao => oao.Addon.AddonName) // Select the AddOn names
                                            .ToListAsync();
            return addOnNames;
        }
    }
}

