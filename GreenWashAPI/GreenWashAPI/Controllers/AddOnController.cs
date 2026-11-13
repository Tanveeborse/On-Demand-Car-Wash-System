using GreenWashAPI.Interface;
using GreenWashAPI.Models;
using GreenWashAPI.VMClass;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenWashAPI.Controllers
{
   
        [ApiController]
        [Route("api/[controller]")]
        public class AddOnController : ControllerBase
        {
            private readonly IAddOns _addOnRepository;
            public AddOnController(IAddOns addOnRepository)
            {
                _addOnRepository = addOnRepository;
            }
           

            [HttpGet("all")]
            [Authorize(Policy = "AdminWasherCustomerPolicy")]
            public async Task<IActionResult> GetAllAddOns()
            {
                try
                {
                    var addOns = await _addOnRepository.GetAllAddOnsAsync();
                    return Ok(addOns);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }
        

            [HttpGet("{id}")]
            [Authorize(Policy = "AdminWasherCustomerPolicy")]
            public async Task<IActionResult> GetAddOnById(int id)
            {
                try
                {
                    var addOn = await _addOnRepository.GetAddOnByIdAsync(id);
                    if (addOn == null)
                        return NotFound("Add-On not found.");
                    return Ok(addOn);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }
           

            [HttpPost("add")]
            [Authorize(Policy = "AdminPolicy")]
            public async Task<IActionResult> AddAddOn([FromBody] AddOnVM addOn)
            {
                if (addOn == null)
                    return BadRequest("Add-On details are required.");

            AddOn addonObj = new AddOn()
            {
                AddonName=addOn.AddonName,
                AddonDescription=addOn.AddonDescription,
                Price=addOn.Price,
                IsActive=addOn.IsActive
            };

                try
                {
                    var createdAddOn = await _addOnRepository.AddAddOnAsync(addonObj);
                    return CreatedAtAction(nameof(GetAddOnById), new { id = createdAddOn.AddonId }, createdAddOn);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }
          

            [HttpPut("update/{addonId}")]
            [Authorize(Policy = "AdminPolicy")]
            public async Task<IActionResult> UpdateAddOn(int addonId,[FromBody] AddOnVM addOn)
            {
                if (addOn == null)
                    return BadRequest("Add-On details are required.");

               AddOn addonObj = new AddOn()
               {
                  AddonId=addonId,
                  AddonName = addOn.AddonName,
                  AddonDescription = addOn.AddonDescription,
                  Price = addOn.Price,
                  IsActive = addOn.IsActive
               };

                try
                {
                    await _addOnRepository.UpdateAddOnAsync(addonObj);
                    return Ok("Add-On updated successfully.");
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }
     

            [HttpDelete("{id}")]
            [Authorize(Policy = "AdminPolicy")]
            public async Task<IActionResult> DeleteAddOn(int id)
            {
                try
                {
                    await _addOnRepository.DeleteAddOnAsync(id);
                    return Ok("Add-On deleted successfully.");
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }
            

            [HttpPatch("activate/{id}")]
            [Authorize(Policy = "AdminPolicy")]
            public async Task<IActionResult> ActivateAddOn(int id)
            {
                try
                {
                    await _addOnRepository.ActivateAddOnAsync(id);
                    return Ok("Add-On activated successfully.");
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }
            
            [HttpPatch("deactivate/{id}")]
            [Authorize(Policy = "AdminPolicy")]
            public async Task<IActionResult> DeactivateAddOn(int id)
            {
                try
                {
                    await _addOnRepository.DeactivateAddOnAsync(id);
                    return Ok("Add-On deactivated successfully.");
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }
           
            [HttpGet("searchByName")]
            [Authorize(Policy = "AdminWasherCustomerPolicy")]
            public async Task<IActionResult> SearchAddOnsByName([FromQuery] string name)
            {
                try
                {
                    var result = await _addOnRepository.SearchAddOnsByNameAsync(name);
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }
         

            [HttpGet("active")]
            [Authorize(Policy = "AdminWasherCustomerPolicy")]
            public async Task<IActionResult> GetActiveAddOns()
            {
                try
                {
                    var result = await _addOnRepository.GetActiveAddOnsAsync();
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }
        }
    }

