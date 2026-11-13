using System;
using System.Text.Json.Serialization;
using System.Text.Json;
using GreenWashAPI.Interface;
using GreenWashAPI.Models;
using GreenWashAPI.VMClass;

using Microsoft.EntityFrameworkCore;

namespace GreenWashAPI.Repository
{
    public class OrderRepo : IOrders
    {
        private readonly GreenWashDbContext _context;
        public OrderRepo(GreenWashDbContext context)
        {
            _context = context;
        }

        public OrderRepo()
        {
        }


        // Create a new order
        public async Task<Order> CreateOrderAsync(Order order)
        {
           
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }

        // Retrieve all orders (useful for admin reporting)
        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders.ToListAsync();
        }

        // Retrieve an order by its Id along with related entities
        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        // Retrieve orders for a specific customer
        public async Task<IEnumerable<Order>> GetOrdersByUserAsync(int userId)
        {
            return await _context.Orders.Where(o => o.UserId == userId).ToListAsync();
        }

        // Retrieve orders for a specific washer
        public async Task<IEnumerable<Order>> GetOrdersByWasherAsync(int washerId)
        {
            return await _context.Orders.Where(o => o.WasherId == washerId).ToListAsync();

        }

        // Retrieve orders by status
        public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(string status)
        {
            var orders = await _context.Orders.ToListAsync();
            return orders.Where(o => string.Equals(o.Status, status, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        // Update an existing order
        public async Task<Order> UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return order;
        }

        // Assign a washer to an order
        public async Task AssignWasherToOrderAsync(int orderId, int washerId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                throw new Exception("Order not found.");
            }
            order.WasherId = washerId;
            order.Status = "Accepted";
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<OrderDetails>> GetCurrentOrdersByStatusForWasherAsync()
        {
            // Define the allowed statuseso
            var allowedStatuses = new List<string> { "Accepted", "In-Process" , "Completed" ,"Canceled" };
            var orders = await _context.Orders
            .Where(o => allowedStatuses.Contains(o.Status))
            .Include(o => o.Car) 
            .Include(o => o.User)
            .Include(o => o.Package)
            .Include(o => o.OrderAddOns) 
            .Select(o => new OrderDetails
            {
                OrderId = o.OrderId,
                WasherId = o.WasherId,
                OrderStatus = o.Status,
                CustomerName = o.User.Fullname,
                CarName = o.Car.Carmake + " " + o.Car.Carmodel, 
                PackageName = o.Package.PackageName,
                AddOns = o.OrderAddOns.Select(ao => ao.Addon.AddonName).ToList(), 
                TotalAmount = o.TotalAmount,
                ScheduledDateTime = o.ScheduledDatetime,
                ActualStartTime = o.ActualStartTime,
                ActualEndTime = o.ActualEndTime
            })
            .ToListAsync();

            return orders;
        }

        public async Task UpdatePaymentStatus(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order != null)
            {
                order.PaymentStatus = "Paid";
                _context.Orders.Update(order);
            }
            var payment = await _context.Payments.FirstOrDefaultAsync(p => p.OrderId == orderId);
            if (payment != null)
            {
                payment.PaymentStatus = "Paid";
                _context.Payments.Update(payment);
            }
            await _context.SaveChangesAsync();
        }




        public async Task<IEnumerable<OrderDetails>> GetCurrentOrdersByStatusForWasherAsync(int userId)
        {
            // Define the allowed statuses
            var allowedStatuses = new List<string> { "Accepted", "In-Process" };
            var orders = await _context.Orders
            .Where(o => o.WasherId == userId && allowedStatuses.Contains(o.Status))
            .Include(o => o.Car) 
            .Include(o => o.User)
            .Include(o => o.Package) 
            .Include(o => o.OrderAddOns) 
            .Select(o => new OrderDetails
            {
                OrderId = o.OrderId,
                OrderStatus = o.Status,
                CustomerName = o.User.Fullname,
                CarName = o.Car.Carmake + " " + o.Car.Carmodel,
                PackageName = o.Package.PackageName,
                AddOns = o.OrderAddOns.Select(ao => ao.Addon.AddonName).ToList(), 
                TotalAmount = o.TotalAmount,
                ScheduledDateTime = o.ScheduledDatetime,
                ActualStartTime = o.ActualStartTime,
                ActualEndTime = o.ActualEndTime
            })
            .ToListAsync();
            return orders;
        }
       
        public async Task<IEnumerable<OrderDetails>> GetPastOrdersByStatusForWasherAsync(int userId)
        {
            var allowedPastStatuses = new[] { "Completed", "Rejected" };
            var orders = await _context.Orders
            .Where(o => o.WasherId == userId && allowedPastStatuses.Contains(o.Status))
            .Include(o => o.Car) 
            .Include(o => o.User)
            .Include(o => o.Package) 
            .Include(o => o.OrderAddOns)
            .Select(o => new OrderDetails
            {
                OrderId = o.OrderId,
                OrderStatus = o.Status,
                CustomerName = o.User.Fullname,
                CarName = o.Car.Carmake + " " + o.Car.Carmodel,
                PackageName = o.Package.PackageName,
                AddOns = o.OrderAddOns.Select(ao => ao.Addon.AddonName).ToList(),
                ScheduledDateTime = o.ScheduledDatetime,
                ActualStartTime = o.ActualStartTime,
                ActualEndTime = o.ActualEndTime
            })
            .ToListAsync();
            return orders;
        }

        public async Task<IEnumerable<OrderDetails>> GetAllPendingOrdersAsync()

        {

            // Define the allowed statuses

            var allowedStatuses = new List<string> { "Pending" };

            var orders = await _context.Orders

            .Where(o => allowedStatuses.Contains(o.Status))

            .Include(o => o.Car)

            .Include(o => o.User)

            .Include(o => o.Package) 

            .Include(o => o.OrderAddOns)

            .Select(o => new OrderDetails

            {

                OrderId = o.OrderId,

                OrderStatus = o.Status,

                CustomerName = o.User.Fullname,

                CarName = o.Car.Carmake + " " + o.Car.Carmodel,

                PackageName = o.Package.PackageName,

                AddOns = o.OrderAddOns.Select(ao => ao.Addon.AddonName).ToList(), 

                TotalAmount = o.TotalAmount,

                ScheduledDateTime = o.ScheduledDatetime,

            })

            .ToListAsync();

            return orders;

        }

        public async Task<Order> CancelOrderAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                throw new Exception("Order not found.");
            }
            order.Status = "Canceled";
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return order;
        }
        public async Task<Order> RescheduleOrderAsync(int orderId, DateTime newSchedule, string newLocation)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                throw new Exception("Order not found.");
            }
            order.ScheduledDatetime = newSchedule;
            order.Location = newLocation;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return order;
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
        public async Task<Order> AssignRandomWasherAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                throw new Exception("Order not found.");
            }

           
            var availableWashers = await _context.Users
                .Where(u => u.Role == "Washer" && (bool)u.IsActive == true)
                .ToListAsync();
            if (availableWashers.Count == 0)
            {
                throw new Exception("No available washers.");
            }
            var random = new Random();
            var randomWasher = availableWashers[random.Next(availableWashers.Count)];
            order.WasherId = randomWasher.UserId;
            order.Status = "Accepted";
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

           
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };

            string json = JsonSerializer.Serialize(order, options);
            Console.WriteLine(json); 

            return order;
        }

        public async Task<IEnumerable<Order>> GetCurrentOrdersByStatusUserAsync(int userId)
        { 
            
            var allowedStatuses = new[] { "Pending", "Accepted", "In-Process" }; 
            return await _context.Orders .Where(o => o.UserId == userId && allowedStatuses
            .Contains(o.Status, StringComparer.OrdinalIgnoreCase)) 
            .Include(o => o.Car) .Include(o => o.Package) .ToListAsync();
        }

      
        public async Task<IEnumerable<Order>> GetPastOrdersByStatusForUserAsync(int userId)
        {
           
            var allowedPastStatuses = new[] { "Completed", "Rejected" };
            return await _context.Orders
                .Where(o => o.UserId == userId && allowedPastStatuses.Contains(o.Status, StringComparer.OrdinalIgnoreCase))
                .Include(o => o.Car)
                .Include(o => o.Package)
                .ToListAsync();
        }

        public async Task UpdateOrderStatusAsync(int orderId, string status)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                throw new Exception("Order not found.");
            }
            order.Status = status;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }
        
        public async Task UpdateStartTimeAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                throw new Exception("Order not found.");
            }
            order.ActualStartTime = DateTime.UtcNow;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }
        
        public async Task UpdateEndTimeAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                throw new Exception("Order not found.");
            }
            order.ActualEndTime = DateTime.UtcNow;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        //Assigning Random Washer 
        public async Task<Order> AssignRandomWasherOnTimeoutAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
                throw new Exception("Order not found.");

          
            if (order.Status.Equals("Pending", StringComparison.OrdinalIgnoreCase) &&
                order.ScheduledDatetime < DateTime.UtcNow)
            {
                var availableWashers = await _context.Users
                    .Where(u => u.Role == "Washer" && (bool)u.IsActive == true)
                    .ToListAsync();
                if (!availableWashers.Any())
                    throw new Exception("No available washers to assign.");
                var random = new Random();
                var randomWasher = availableWashers[random.Next(availableWashers.Count)];
                order.WasherId = randomWasher.UserId;

              
                order.Status = "Accepted";
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Order is either not pending or the scheduled time has not expired.");
            }
            return order;
        }

        public async Task<IEnumerable<Order>> GetOrdersForAdminReportAsync(string orderNumber,string washerName,string type,string service,DateTime? startDate,DateTime? endDate)
        {
            var query = _context.Orders
                .Include(o => o.User)
                .Include(o => o.Washer)
                .Include(o => o.Car)
                .Include(o => o.Package)
                .AsQueryable();

           
            if (!string.IsNullOrEmpty(orderNumber))
            {
                
                query = query.Where(o => o.OrderId.ToString().Contains(orderNumber));
            }
           
            if (!string.IsNullOrEmpty(washerName))
            {
                query = query.Where(o => o.Washer != null &&
                                         o.Washer.Fullname.ToLower().Contains(washerName.ToLower()));
            }
           
           
            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(o => o.Status != null &&
                                         o.Status.ToLower().Contains(type.ToLower()));
            }
          
            if (!string.IsNullOrEmpty(service))
            {
                query = query.Where(o => o.Package != null &&
                                         o.Package.PackageName.ToLower().Contains(service.ToLower()));
            }
           
            if (startDate.HasValue)
            {
                query = query.Where(o => o.CreatedAt >= startDate.Value);
            }
            if (endDate.HasValue)
            {
                query = query.Where(o => o.CreatedAt <= endDate.Value);
            }
            return await query.ToListAsync();
        }

        public async Task<List<OrderHistoryDTO>> GetOrderHistoryAsync(int userId)
        {
            var orderHistory = await _context.Orders
                .Where(o => o.UserId == userId) 
                .Include(o => o.Package)
                .Include(o => o.Car) 
                .Include(o => o.OrderAddOns) 
                    .ThenInclude(oa => oa.Addon) 
                .Select(o => new OrderHistoryDTO
                {
                    OrderId = o.OrderId,
                    Date = o.CreatedAt ?? DateTime.Now, 
                    PackageName = o.Package.PackageName,
                    Status = o.Status,
                    AddonNames = o.OrderAddOns.Select(oa => oa.Addon.AddonName).ToList(),
                    CarMake = o.Car.Carmake,
                    CarModel = o.Car.Carmodel,
                    TotalPrice = o.TotalAmount,
                })
                .ToListAsync();

            return orderHistory;
        }
    }
}

// DTO class to hold the result
public class OrderHistoryDTO
{
    public int OrderId { get; set; }
    public DateTime Date { get; set; }
    public string PackageName { get; set; }
    public string Status { get; set; }
    public List<string> AddonNames { get; set; }
    public string CarMake { get; set; }
    public string CarModel { get; set; }
    public decimal TotalPrice { get; set; }
}













