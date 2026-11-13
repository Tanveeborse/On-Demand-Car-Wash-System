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
    public class PromoCodeController : ControllerBase
    {
        private readonly IPromoCodes _promoRepo;
        public PromoCodeController(IPromoCodes promoRepo)
        {
            _promoRepo = promoRepo;
        }
       

        [HttpGet("all")]
        [Authorize(Policy = "AdminWasherCustomerPolicy")]
        public async Task<IActionResult> GetAllPromoCodes()
        {
            try
            {
                var promos = await _promoRepo.GetAllPromoCodesAsync();
                return Ok(promos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
      

        [HttpGet("{id}")]
        [Authorize(Policy = "AdminWasherCustomerPolicy")]
        public async Task<IActionResult> GetPromoCodeById(int id)
        {
            try
            {
                var promo = await _promoRepo.GetPromoCodeByIdAsync(id);
                if (promo == null)
                    return NotFound("Promo Code not found.");
                return Ok(promo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
      

        [HttpGet("searchByCode")]
        [Authorize(Policy = "AdminWasherCustomerPolicy")]
        public async Task<IActionResult> SearchPromoCodesByCode([FromQuery] string code)
        {
            try
            {
                var promos = await _promoRepo.SearchPromoCodesByCodeAsync(code);
                return Ok(promos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
       
        [HttpGet("active")]
        [Authorize(Policy = "AdminWasherCustomerPolicy")]
        public async Task<IActionResult> GetActivePromoCodes()
        {
            try
            {
                var promos = await _promoRepo.GetActivePromoCodesAsync();
                return Ok(promos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        
        [HttpPost("add")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> AddPromoCode([FromBody] PromoCodeVM promo)
        {
            if (promo == null)
                return BadRequest("Promo Code details are required.");

            PromoCode pObj = new PromoCode()
            {
                Code=promo.Code,
                Description=promo.Description,
                DiscountPercentage=promo.DiscountPercentage,
                ExpiryDate=promo.ExpiryDate,
                IsActive=promo.IsActive
            };

            try
            {
                var createdPromo = await _promoRepo.AddPromoCodeAsync(pObj);
                return Ok();
               // return CreatedAtAction(nameof(GetPromoCodeById), new { id = createdPromo.PromoCodeId }, createdPromo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
       

        [HttpPut("update")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> UpdatePromoCode([FromBody] PromoCodeVM promo)
        {
            if (promo == null)
                return BadRequest("Promo Code details are required.");

            PromoCode pObj = new PromoCode()
            {
                PromoCodeId=promo.PromoCodeId,
                Code = promo.Code,
                Description = promo.Description,
                DiscountPercentage = promo.DiscountPercentage,
                ExpiryDate = promo.ExpiryDate,
                IsActive = promo.IsActive
            };

            try
            {
                await _promoRepo.UpdatePromoCodeAsync(pObj);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
       

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeletePromoCode(int id)
        {
            try
            {
                await _promoRepo.DeletePromoCodeAsync(id);
                return Ok("Promo Code deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
       

        [HttpPatch("activate/{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> ActivatePromoCode(int id)
        {
            try
            {
                await _promoRepo.ActivatePromoCodeAsync(id);
                return Ok("Promo Code activated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
   

        [HttpPatch("deactivate/{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeactivatePromoCode(int id)
        {
            try
            {
                await _promoRepo.DeactivatePromoCodeAsync(id);
                return Ok("Promo Code deactivated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        

        [HttpPatch("deactivateExpired")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeactivateExpiredPromoCodes()
        {
            try
            {
                await _promoRepo.DeactivateExpiredPromoCodesAsync();
                return Ok("Expired Promo Codes deactivated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

