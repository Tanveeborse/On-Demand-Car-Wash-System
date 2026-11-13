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
    public class PackageController : ControllerBase
    {
        private readonly IPackages _packageRepository;
        public PackageController(IPackages packageRepository)
        {
            _packageRepository = packageRepository;
        }
       

        [HttpGet("all")]
        [Authorize(Policy = "AdminWasherCustomerPolicy")]
        public async Task<IActionResult> GetAllPackages()
        {
            try
            {
                var packages = await _packageRepository.GetAllPackagesAsync();
                return Ok(packages);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        
        [HttpGet("{id}")]
        [Authorize(Policy = "AdminWasherCustomerPolicy")]
        public async Task<IActionResult> GetPackageById(int id)
        {
            try
            {
                var package = await _packageRepository.GetPackageByIdAsync(id);
                if (package == null)
                    return NotFound("Package not found.");
                return Ok(package);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
     

        [HttpGet("searchByName")]
        [Authorize(Policy = "AdminWasherCustomerPolicy")]
        public async Task<IActionResult> SearchPackagesByName([FromQuery] string name)
        {
            try
            {
                var packages = await _packageRepository.SearchPackagesByNameAsync(name);
                return Ok(packages);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        

        [HttpGet("active")]
        [Authorize(Policy = "AdminWasherCustomerPolicy")]
        public async Task<IActionResult> GetActivePackages()
        {
            try
            {
                var packages = await _packageRepository.GetActivePackagesAsync();
                return Ok(packages);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
      

        [HttpPost("add")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> AddPackage([FromBody] PackageVM package)
        {
            Package packageObj = new Package
            {
                PackageName = package.PackageName,
                PackageDescription = package.PackageDescription,
                Price = package.Price,
                IsActive = package.IsActive,
                
            };
            packageObj.CreatedAt = DateTime.UtcNow;
            packageObj.UpdatedAt = DateTime.UtcNow;

            // Validate package fields
            if (string.IsNullOrEmpty(packageObj.PackageName))
            {
                return BadRequest("Package name is required.");
            }

            if (string.IsNullOrEmpty(packageObj.PackageDescription))
            {
                return BadRequest("Package description is required.");
            }

            if (packageObj.Price <= 0)
            {
                return BadRequest("Package price must be greater than zero.");
            }
            try
            {
                var createdPackage = await _packageRepository.AddPackageAsync(packageObj);
                return Ok();
                //return CreatedAtAction(nameof(GetPackageById), new { id = createdPackage.PackageId }, createdPackage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
      

        [HttpPut("update/{packageId}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> UpdatePackage(int packageId,[FromBody] PackageVM package)
        {
            Package packageObj = new Package
            {
                PackageId=packageId,
                PackageName = package.PackageName,
                PackageDescription = package.PackageDescription,
                Price = package.Price,
                IsActive = package.IsActive,

            };
            packageObj.CreatedAt = DateTime.UtcNow;
            packageObj.UpdatedAt = DateTime.UtcNow;
            if (packageObj == null)
                return BadRequest("Package details are required.");
            try
            {
                await _packageRepository.UpdatePackageAsync(packageObj);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
       

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeletePackage(int id)
        {
            try
            {
                await _packageRepository.DeletePackageAsync(id);
                return Ok("Package deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        

        [HttpPost("activate/{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> ActivatePackage(int id)
        {
            try
            {
                await _packageRepository.ActivatePackageAsync(id);
                return Ok("Package activated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        // POST: api/Package/deactivate/{id}
        [HttpPost("deactivate/{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeactivatePackage(int id)
        {
            try
            {
                await _packageRepository.DeactivatePackageAsync(id);
                return Ok("Package deactivated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}




















