using GreenWashAPI.Interface;
using GreenWashAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GreenWashAPI.Repository
{
    public class CarRepo:ICars
    {
        private readonly GreenWashDbContext _context;
        public CarRepo(GreenWashDbContext context)
        {
            _context = context;
        }
        // Get all cars from the database.
        public async Task<IEnumerable<Car>> GetAllCarsAsync()
        {
            return await _context.Cars.ToListAsync();
        }
            
        // Get a specific car by its ID.
        public async Task<Car> GetCarByIdAsync(int carId)
        {
            return await _context.Cars.FirstOrDefaultAsync(c => c.CarId == carId);
        }
            
        // Get all cars associated with a specific user.
        public async Task<IEnumerable<Car>> GetCarsByUserIdAsync(int userId)
        {
            return await _context.Cars.Where(c => c.UserId == userId).ToListAsync();
        }
            
        // Add a new car to the database.
        public async Task<Car> AddCarAsync(Car car)
        {
            await _context.Cars.AddAsync(car);
            await _context.SaveChangesAsync();
            return car;
        }
        // Update an existing car's details.
        public async Task UpdateCarAsync(Car car)
        {
            var existingCar = await _context.Cars.FindAsync(car.CarId);
            if (existingCar == null)
                throw new Exception("Car not found.");
            // Update properties
            
            existingCar.Carmake = car.Carmake;
            existingCar.Carmodel = car.Carmodel;
            existingCar.Carimage = car.Carimage;
            existingCar.IsActive = car.IsActive;
            existingCar.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
        }
        // Delete a car from the database.
        public async Task DeleteCarAsync(int carId)
        {
            var car = await _context.Cars.FindAsync(carId);
            if (car == null)
                throw new Exception("Car not found.");
            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
        }
        // Activate a car (set IsActive to true).
        public async Task ActivateCarAsync(int carId)
        {
            var car = await _context.Cars.FindAsync(carId);
            if (car == null)
                throw new Exception("Car not found.");
            car.IsActive = true;
            car.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
        }
        // Deactivate a car (set IsActive to false).
        public async Task DeactivateCarAsync(int carId)
        {
            var car = await _context.Cars.FindAsync(carId);
            if (car == null)
                throw new Exception("Car not found.");
            car.IsActive = false;
            car.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
        }
    }
}


