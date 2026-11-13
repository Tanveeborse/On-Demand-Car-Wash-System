using GreenWashAPI.Models;

namespace GreenWashAPI.Interface
{
    public interface ICars
    {
        Task<IEnumerable<Car>> GetAllCarsAsync();
        Task<Car> GetCarByIdAsync(int carId);
        Task<IEnumerable<Car>> GetCarsByUserIdAsync(int userId);
        Task<Car> AddCarAsync(Car car);
        Task UpdateCarAsync(Car car);
        Task DeleteCarAsync(int carId);
        Task ActivateCarAsync(int carId);
        Task DeactivateCarAsync(int carId);
    }
}
