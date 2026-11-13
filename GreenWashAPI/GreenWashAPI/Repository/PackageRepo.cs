using DocumentFormat.OpenXml.Packaging;
using GreenWashAPI.Interface;
using GreenWashAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GreenWashAPI.Repository
{
    public class PackageRepo : IPackages
    {
        private readonly GreenWashDbContext _context;
        public PackageRepo(GreenWashDbContext context)
        {
            _context = context;
        }


        // Retrieve all packages (active and inactive).
        public async Task<IEnumerable<Package>> GetAllPackagesAsync()
        {
            return await _context.Packages.ToListAsync();
        }
          

        // Retrieve a package by its ID.
        public async Task<Package?> GetPackageByIdAsync(int packageId) =>
            await _context.Packages.FirstOrDefaultAsync(p => p.PackageId == packageId);


        // Add a new package.
        public async Task<Package> AddPackageAsync(Package package)
        {
            await _context.Packages.AddAsync(package);
            await _context.SaveChangesAsync();
            return package;
        }

        // Update an existing package's details.
        public async Task UpdatePackageAsync(Package package)
        {
            var existingPackage = await _context.Packages.FindAsync(package.PackageId);
            if (existingPackage == null)
                throw new Exception("Package not found.");
            // Update properties.
            existingPackage.PackageName = package.PackageName;
            existingPackage.PackageDescription = package.PackageDescription;
            existingPackage.Price = package.Price;
            existingPackage.IsActive = package.IsActive;
            existingPackage.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
        }
        // Delete a package.
        public async Task DeletePackageAsync(int packageId)
        {
            var package = await _context.Packages.FindAsync(packageId);
            if (package == null)
                throw new Exception("Package not found.");
            _context.Packages.Remove(package);
            await _context.SaveChangesAsync();
        }
        // Activate a package (set IsActive to true).
        public async Task ActivatePackageAsync(int packageId)
        {
            var package = await _context.Packages.FindAsync(packageId);
            if (package == null)
                throw new Exception("Package not found.");
            package.IsActive = true;
            package.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
        }
        // Deactivate a package (set IsActive to false).
        public async Task DeactivatePackageAsync(int packageId)
        {
            var package = await _context.Packages.FindAsync(packageId);
            if (package == null)
                throw new Exception("Package not found.");
            package.IsActive = false;
            package.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
        }
        // Search packages by name (case-insensitive partial match).
        public async Task<IEnumerable<Package>> SearchPackagesByNameAsync(string name)
        {
            return await _context.Packages
                .Where(p => p.PackageName.ToLower().Contains(name.ToLower()))
                .ToListAsync();
        }
        // Retrieve all active packages.
        public async Task<IEnumerable<Package>> GetActivePackagesAsync() =>
            await _context.Packages.Where(p => (bool)p.IsActive).ToListAsync();
    }
}

