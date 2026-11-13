using GreenWashAPI.Interface;
using GreenWashAPI.Models;
using GreenWashAPI.VMClass;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GreenWashAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrders _orderRepo;
        public OrdersController(IOrders orderRepo)
        {
            _orderRepo = orderRepo;
        }
        

        [HttpPost("add")]
        [Authorize(Policy = "AdminCustomerPolicy")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderVM order)
        {
            Order orderObj = new Order()
            {
                CarId = order.CarId,
                UserId = order.UserId,
                WasherId = order.WasherId,
                PackageId = order.PackageId,
                Status = order.Status,
                PaymentStatus = order.PaymentStatus,
                Location =order.Location,
                TotalAmount=order.TotalAmount,
                ScheduledDatetime=order.ScheduledDatetime
            };
            orderObj.ActualStartTime = DateTime.UtcNow;
            orderObj.ActualEndTime = DateTime.UtcNow;
            try
            {
                var createdOrder = await _orderRepo.CreateOrderAsync(orderObj);
                return Ok(new { orderId = orderObj.OrderId });
             
            }
            catch (Exception ex)
            {
               
                return BadRequest(new { error = ex.Message });
            }
        }
       

        [HttpGet("{orderId}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            try
            {
                var order = await _orderRepo.GetOrderByIdAsync(orderId);
                if (order == null)
                    return NotFound();
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
       

        [HttpGet("user/{userId}")]
        [Authorize(Policy = "AdminCustomerPolicy")]
        public async Task<IActionResult> GetOrdersByUser(int userId)
        {
            try
            {
                var orders = await _orderRepo.GetOrdersByUserAsync(userId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("update-payment-status/{orderId}")]
     
        public async Task<IActionResult> UpdatePaymentStatus(int orderId)
        {
            try
            {
                await _orderRepo.UpdatePaymentStatus(orderId);
                return Ok(new { message = "Payment status updated to Paid" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating payment status", error = ex.Message });
            }
        }

        [HttpGet("washer/{washerId}")]
        [Authorize(Policy = "AdminWasherPolicy")]
        public async Task<IActionResult> GetOrdersByWasher(int washerId)
        {
            try
            {
                var orders = await _orderRepo.GetOrdersByWasherAsync(washerId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


        [HttpPut("{orderId}")]
        
        public async Task<IActionResult> UpdateOrder(int orderId, [FromBody] Order order)
        {

            try
            {
                if (orderId != order.OrderId)
                    return BadRequest("Order ID mismatch.");
                var updatedOrder = await _orderRepo.UpdateOrderAsync(order);
                return Ok(updatedOrder);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        

        [HttpPut("{orderId}/assign/{washerId}")]
        [Authorize(Policy = "AdminWasherPolicy")]
        public async Task<IActionResult> AssignWasherToOrder(int orderId, int washerId)
        {
            try
            {
                await _orderRepo.AssignWasherToOrderAsync(orderId, washerId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
       

        [HttpGet("all")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                var orders = await _orderRepo.GetAllOrdersAsync();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("all_status")]
        [Authorize(Policy = "AdminWasherPolicy")]
        public async Task<ActionResult<IEnumerable<Order>>> GetCurrentOrdersByStatusForWasher()
        {
            var orders = await _orderRepo.GetCurrentOrdersByStatusForWasherAsync();
            if (orders == null || !orders.Any())
            {
                return NotFound();
            }
            return Ok(orders);
        }

        [HttpGet("pending_all")]
        [Authorize(Policy = "AdminWasherPolicy")]
        public async Task<ActionResult<IEnumerable<OrderDetails>>> GetAllPendingOrders()
        {
            var orders = await _orderRepo.GetAllPendingOrdersAsync();
            if (orders == null || !orders.Any())
            {
                return NotFound();
            }
            return Ok(orders);
        }

        [HttpDelete("{orderId}/cancel")]
     
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            try
            {
                var order = await _orderRepo.CancelOrderAsync(orderId);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
       

        [HttpPut("{orderId}/reschedule")]
        [Authorize(Policy = "CustomerPolicy")]
        public async Task<IActionResult> RescheduleOrder(int orderId, [FromBody] RescheduleOrderRequest request)
        {
            try
            {
                var order = await _orderRepo.RescheduleOrderAsync(orderId, request.NewSchedule, request.NewLocation);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
       

        [HttpPut("{orderId}/paymentstatus")]
      
        public async Task<IActionResult> UpdatePaymentStatus(int orderId, [FromBody] PaymentStatusRequest request)
        {
            try
            {
                await _orderRepo.UpdatePaymentStatusAsync(orderId, request.PaymentStatus);
                return Ok("Payment status updated.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
       

        [HttpPut("{orderId}/assignrandom")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> AssignRandomWasher(int orderId)
        {
            try
            {
                var order = await _orderRepo.AssignRandomWasherAsync(orderId);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
       

        [HttpPut("{orderId}/assignrandomontimeout")]
      
        public async Task<IActionResult> AssignRandomWasherOnTimeout(int orderId)
        {
            try
            {
                var order = await _orderRepo.AssignRandomWasherOnTimeoutAsync(orderId);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
      

        [HttpGet("adminreport")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetOrdersForAdminReport([FromQuery] AdminReportRequest request)
        {
            try
            {
                var orders = await _orderRepo.GetOrdersForAdminReportAsync(
                    request.OrderNumber,
                    request.WasherName,
                    request.Type,
                    request.Service,
                    request.StartDate,
                    request.EndDate);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

       
        [HttpGet("orderhistory/{userId}")]
        public async Task<ActionResult<List<OrderHistoryDTO>>> GetOrderHistory(int userId)
        {
            
            var orderHistory = await _orderRepo.GetOrderHistoryAsync(userId);

            if (orderHistory == null || orderHistory.Count == 0)
            {
                return NotFound("No orders found for the given user.");
            }

            // Return the order history in the response
            return Ok(orderHistory);
        }

        [HttpPatch("{orderId}/startWash")]
        [Authorize(Policy = "WasherPolicy")]
        public async Task<IActionResult> UpdateStartTime(int orderId)
        {
            try
            {
                await _orderRepo.UpdateStartTimeAsync(orderId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPatch("{orderId}/endWash")]
        [Authorize(Policy = "WasherPolicy")]
        public async Task<IActionResult> UpdateEndTime(int orderId)
        {
            try
            {
                await _orderRepo.UpdateEndTimeAsync(orderId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{orderId}/update/{orderStatus}")]
        [Authorize(Policy = "WasherPolicy")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, string orderStatus)
        {
            try
            {
                await _orderRepo.UpdateOrderStatusAsync(orderId, orderStatus);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

       
        [HttpGet("washer_current/{userId}")]
        [Authorize(Policy = "AdminWasherPolicy")]
        public async Task<ActionResult<IEnumerable<OrderDetails>>> GetCurrentOrdersByStatusForWasher(int userId)
        {
            var orders = await _orderRepo.GetCurrentOrdersByStatusForWasherAsync(userId);
            if (orders == null || !orders.Any())
            {
                return NotFound();
            }
            return Ok(orders);
        }
      
        [HttpGet("washer_past/{userId}")]
        [Authorize(Policy = "AdminWasherPolicy")]
        public async Task<ActionResult<IEnumerable<OrderDetails>>> GetPastOrdersByStatusForWasher(int userId)
        {
            var orders = await _orderRepo.GetPastOrdersByStatusForWasherAsync(userId);
            if (orders == null || !orders.Any())
            {
                return NotFound();
            }
            return Ok(orders);
        }
    }
}
    

























