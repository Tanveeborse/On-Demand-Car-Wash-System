using GreenWashAPI.Interface;
using GreenWashAPI.Models;
using GreenWashAPI.Repository;
using GreenWashAPI.VMClass;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace GreenWashAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController:ControllerBase
    {
        private readonly IUsers _userRepo;

        public UserController(IUsers userRepo)
        {
            _userRepo = userRepo;
        }

       
        [HttpGet("all")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userRepo.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        
        [HttpGet("{id}")]
        [Authorize(Policy = "AdminCustomerPolicy")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _userRepo.GetUserByIdAsync(id);
                if (user == null)
                    return NotFound("User not found.");
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("email/{email}")]
        [Authorize(Policy = "AdminAndCustomerPolicy")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            try
            {
                var user = await _userRepo.GetUserByEmailAsync(email);
                if (user == null)
                    return NotFound("User not found.");
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("role/{role}")]
        [Authorize(Policy = "AdminCustomerPolicy")]
        public async Task<IActionResult> GetUsersByRole(string role)
        {
            try
            {
                var users = await _userRepo.GetUsersByRoleAsync(role);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("add")]
        [Authorize(Policy = "AdminWasherCustomerPolicy")]
        public async Task<IActionResult> AddUser([FromBody] UserVM user)
        {
            User userObj = new User()
            {
                Fullname=user.Fullname,
                Address=user.Address,
                EmailId=user.EmailId,
                Password=user.Password,
                PhoneNumber=user.PhoneNumber,
                ProfilePicture=user.ProfilePicture,
                Role=user.Role
            };
            try
            {
                if (userObj == null)
                {
                    return BadRequest("User data is required.");
                }
                var createdUser = await _userRepo.AddUserAsync(userObj);
              
                 return Ok("User Created Successufully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("update/{userId}")]
        [Authorize(Policy = "AdminWasherCustomerPolicy")]
        public async Task<IActionResult> UpdateUser(int userId,[FromBody] UserDTo user)
        {
          
            User userObj = new User()
            {
               
                Fullname = user.Fullname,
                Address = user.Address,
                EmailId = user.EmailId,
               
                PhoneNumber = user.PhoneNumber,
               
            };
            try
            {
                await _userRepo.UpdateUserAsync(userId,userObj);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _userRepo.DeleteUserAsync(id);
                return Ok("User deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

      
        [HttpPost("activate/{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> ActivateUser(int id)
        {
            try
            {
                await _userRepo.ActivateUserAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        
        [HttpPost("deactivate/{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeactivateUser(int id)
        {
            try
            {
                await _userRepo.DeactivateUserAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
 
        [HttpGet("searchByRole/{role}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> SearchByRole(string role)
        {
            try
            {
                var result = await _userRepo.SearchUsersByRoleAsync(role);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

 
        [HttpGet("searchByRating")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> SearchByRating([FromQuery] decimal minRating, [FromQuery] decimal maxRating)
        {
            try
            {
                var result = await _userRepo.SearchUsersByRatingAsync(minRating, maxRating);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("active")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetActiveUsers()
        {
            try
            {
                var result = await _userRepo.GetActiveUsersAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

 
        [HttpGet("inactive")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetInactiveUsers()
        {
            try
            {
                var result = await _userRepo.GetInactiveUsersAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("exportWasherReport")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> ExportWasherReport()
        {
            try
            {
                var reportBytes = await _userRepo.ExportWasherReportToExcelAsync();
                return File(reportBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "WasherReport.xlsx");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
