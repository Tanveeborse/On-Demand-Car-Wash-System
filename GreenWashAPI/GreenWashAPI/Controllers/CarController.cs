using GreenWashAPI.Interface;
using GreenWashAPI.Models;
using GreenWashAPI.VMClass;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenWashAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarController : ControllerBase
    {
        private readonly ICars _carRepository;
        public CarController(ICars carRepository)
        {
            _carRepository = carRepository;
        }
       

        [HttpGet("all")]
        [Authorize(Policy = "AdminWasherCustomerPolicy")]
        public async Task<IActionResult> GetAllCars()
        {
            try
            {
                var cars = await _carRepository.GetAllCarsAsync();
                return Ok(cars);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
      

        [HttpGet("{id}")]
        [Authorize(Policy = "AdminWasherCustomerPolicy")]
        public async Task<IActionResult> GetCarById(int id)
        {
            try
            {
                var car = await _carRepository.GetCarByIdAsync(id);
                if (car == null)
                    return NotFound("Car not found.");
                return Ok(car);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
       

        [HttpGet("user/{userId}")]
        [Authorize(Policy = "AdminWasherCustomerPolicy")]
        public async Task<IActionResult> GetCarsByUserId(int userId)
        {
            try
            {
                var cars = await _carRepository.GetCarsByUserIdAsync(userId);
                return Ok(cars);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
     

        [HttpPost("add")]
        [Authorize(Policy = "CustomerPolicy")]
        public async Task<IActionResult> AddCar([FromBody] CarVM car)
        {
            if (car == null)
                return BadRequest("Car details are required.");

            Car carObj = new Car()
            {
                UserId=car.UserId,
                Carmake=car.Carmake,
                Carmodel=car.Carmodel,
                Carimage=car.Carimage,
                IsActive=car.IsActive
            };

            try
            {
                var createdCar = await _carRepository.AddCarAsync(carObj);

                return Ok(new
                {
                    message = "Cars Details Added Successfully"
                });


            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
      

        [HttpPut]
        [Authorize(Policy = "CustomerPolicy")]
        public async Task<IActionResult> UpdateCar([FromBody] CarVM car)
        {
            if (car == null)
                return BadRequest("Car details are required.");
            Car carObj = new Car()
            {
                UserId=car.UserId,
                CarId=car.CarId,
                Carmake = car.Carmake,
                Carmodel = car.Carmodel,
                Carimage = car.Carimage,
                IsActive = car.IsActive
            };

            try
            {
                await _carRepository.UpdateCarAsync(carObj);
                return Ok("Car updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        

        [HttpDelete("{id}")]
        [Authorize(Policy = "CustomerPolicy")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            try
            {
                await _carRepository.DeleteCarAsync(id);
                return Ok("Car deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
     

        [HttpPost("activate/{id}")]
       // [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> ActivateCar(int id)
        {
            try
            {
                await _carRepository.ActivateCarAsync(id);
                return Ok("Car activated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        
        [HttpPost("deactivate/{id}")]
        //[Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> DeactivateCar(int id)
        {
            try
            {
                await _carRepository.DeactivateCarAsync(id);
                return Ok("Car deactivated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}




















