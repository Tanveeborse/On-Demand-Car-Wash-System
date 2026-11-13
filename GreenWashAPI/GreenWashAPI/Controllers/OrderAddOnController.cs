using GreenWashAPI.Interface;
using GreenWashAPI.Models;
using GreenWashAPI.Repository;
using GreenWashAPI.VMClass;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenWashAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderAddOnsController : ControllerBase
    {
        private readonly IOrderAddOns _orderAddOnRepo;
        public OrderAddOnsController(IOrderAddOns orderAddOnRepo)
        {
            _orderAddOnRepo = orderAddOnRepo;
        }


        [HttpPost("add")]
        [Authorize(Policy = "AdminCustomerPolicy")]
        public async Task<IActionResult> CreateOrderAddOn([FromBody] OrderAddOnVM request)
        {
            try
            {
               
                var createdOrderAddOn = await _orderAddOnRepo.CreateOrderAddOnAsync((int)request.OrderId, (int)request.AddonId);
             
                return Ok();
            }
            catch (Exception ex)
            {
               
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{orderAddOnId}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetOrderAddOnById(int orderAddOnId)
        {
            try
            {
                var orderAddOn = await _orderAddOnRepo.GetOrderAddOnByIdAsync(orderAddOnId);
                if (orderAddOn == null)
                    return NotFound();
                return Ok(orderAddOn);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        

        [HttpGet]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetAllOrderAddOns()
        {
            try
            {
                var orderAddOns = await _orderAddOnRepo.GetAllOrderAddOnsAsync();
                return Ok(orderAddOns);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

   
        [HttpGet("{orderId}/invoice")]
        [Authorize(Policy = "AdminCustomerPolicy")]
        public async Task<IActionResult> GenerateInvoice(int orderId)
        {
            try
            {
                var invoice = await _orderAddOnRepo.GenerateInvoiceAsync(orderId);
                return Ok(invoice);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{orderId}/generate")]
     
        public async Task<IActionResult> GenerateReceipt(int orderId)
        {
            try
            {
                var receipt = await _orderAddOnRepo.GenerateReceiptAsync(orderId);
                return Ok(receipt);
            }
            catch (Exception ex)
            {
             
                return BadRequest(new { error = ex.Message });
            }
        }

      
        [HttpGet("order/{orderId}")]
   
        public async Task<IActionResult> GetOrderAddOnsByOrderId(int orderId)
        {
            try
            {
                var orderAddOns = await _orderAddOnRepo.GetAddOnsByOrderIdAsync(orderId);
                return Ok(orderAddOns);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
   
