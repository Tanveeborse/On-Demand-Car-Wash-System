
using GreenWashAPI.Models;
using GreenWashAPI.Repository;

namespace GreenWashAPI.Interface
{
    public interface IPackages
    {
        Task<IEnumerable<Package>> GetAllPackagesAsync();
        Task<Package?> GetPackageByIdAsync(int packageId);
        Task<Package> AddPackageAsync(Package package);
        Task UpdatePackageAsync(Package package);
        Task DeletePackageAsync(int packageId);
        Task ActivatePackageAsync(int packageId);
        Task DeactivatePackageAsync(int packageId);
        Task<IEnumerable<Package>> SearchPackagesByNameAsync(string name);
        Task<IEnumerable<Package>> GetActivePackagesAsync();

    }
}
