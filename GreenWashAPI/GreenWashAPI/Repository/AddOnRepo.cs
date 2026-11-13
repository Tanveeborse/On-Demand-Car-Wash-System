using GreenWashAPI.Interface;
using GreenWashAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GreenWashAPI.Repository
{
    public class AddOnRepo:IAddOns
    {
        private readonly GreenWashDbContext _context;
        public AddOnRepo(GreenWashDbContext context)
        {
            _context = context;
        }
        // Retrieve all add-ons.
        public async Task<IEnumerable<AddOn>> GetAllAddOnsAsync() =>
            await _context.AddOns.ToListAsync();
        // Retrieve a specific add-on by its ID.
        public async Task<AddOn> GetAddOnByIdAsync(int addOnId) =>
            await _context.AddOns.FirstOrDefaultAsync(a => a.AddonId == addOnId);
        // Add a new add-on.
        public async Task<AddOn> AddAddOnAsync(AddOn addOn)
        {
            await _context.AddOns.AddAsync(addOn);
            await _context.SaveChangesAsync();
            return addOn;
        }
        // Update an existing add-on's details.
        public async Task UpdateAddOnAsync(AddOn addOn)
        {
            var existingAddOn = await _context.AddOns.FindAsync(addOn.AddonId);
            if (existingAddOn == null)
                throw new Exception("Add-On not found.");
            existingAddOn.AddonName = addOn.AddonName;
            existingAddOn.AddonDescription = addOn.AddonDescription;
            existingAddOn.Price = addOn.Price;
            existingAddOn.IsActive = addOn.IsActive;
            existingAddOn.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
        }
        // Delete an add-on.
        public async Task DeleteAddOnAsync(int addOnId)
        {
            var addOn = await _context.AddOns.FindAsync(addOnId);
            if (addOn == null)
                throw new Exception("Add-On not found.");
            _context.AddOns.Remove(addOn);
            await _context.SaveChangesAsync();
        }
        // Activate an add-on (partial update via HTTP PATCH style).
        public async Task ActivateAddOnAsync(int addOnId)
        {
            var addOn = new AddOn
            {
                AddonId = addOnId,
                IsActive = true,
                UpdatedAt = DateTime.Now
            };
            _context.AddOns.Attach(addOn);
            _context.Entry(addOn).Property(a => a.IsActive).IsModified = true;
            _context.Entry(addOn).Property(a => a.UpdatedAt).IsModified = true;
            await _context.SaveChangesAsync();
        }
        // Deactivate an add-on (partial update via HTTP PATCH style).
        public async Task DeactivateAddOnAsync(int addOnId)
        {
            var addOn = new AddOn
            {
                AddonId = addOnId,
                IsActive = false,
                UpdatedAt = DateTime.Now
            };
            _context.AddOns.Attach(addOn);
            _context.Entry(addOn).Property(a => a.IsActive).IsModified = true;
            _context.Entry(addOn).Property(a => a.UpdatedAt).IsModified = true;
            await _context.SaveChangesAsync();
        }
        // Search add-ons by name (case-insensitive partial match).
        public async Task<IEnumerable<AddOn>> SearchAddOnsByNameAsync(string name)
        {
            return await _context.AddOns
                .Where(a => a.AddonName.ToLower().Contains(name.ToLower())) // Convert both to lowercase for case-insensitive search
                .ToListAsync();
        }


        // Retrieve all active add-ons.
        public async Task<IEnumerable<AddOn>> GetActiveAddOnsAsync()
        {
            return await _context.AddOns.Where(a => (bool)a.IsActive).ToListAsync();
        }
    }
            
}
    
