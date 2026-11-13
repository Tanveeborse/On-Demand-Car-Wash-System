using GreenWashAPI.Interface;
using GreenWashAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GreenWashAPI.Repository
{
    public class PromoCodeRepo : IPromoCodes
    {
        private readonly GreenWashDbContext _context;
        public PromoCodeRepo(GreenWashDbContext context)
        {
            _context = context;
        }
        // Retrieve all promo codes.
        public async Task<IEnumerable<PromoCode>> GetAllPromoCodesAsync() =>
            await _context.PromoCodes.ToListAsync();
        // Retrieve a specific promo code by its ID.
        public async Task<PromoCode> GetPromoCodeByIdAsync(int promoCodeId) =>
            await _context.PromoCodes.FirstOrDefaultAsync(p => p.PromoCodeId == promoCodeId);
        // Add a new promo code.
        public async Task<PromoCode> AddPromoCodeAsync(PromoCode promoCode)
        {
            await _context.PromoCodes.AddAsync(promoCode);
            await _context.SaveChangesAsync();
            return promoCode;
        }
        // Update an existing promo code's details.
        public async Task UpdatePromoCodeAsync(PromoCode promoCode)
        {
            var existingPromo = await _context.PromoCodes.FindAsync(promoCode.PromoCodeId);
            if (existingPromo == null)
                throw new Exception("Promo Code not found.");
            existingPromo.Code = promoCode.Code;
            existingPromo.Description = promoCode.Description;
            existingPromo.DiscountPercentage = promoCode.DiscountPercentage;
            existingPromo.ExpiryDate = promoCode.ExpiryDate;
            existingPromo.IsActive = promoCode.IsActive;
            existingPromo.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
        }
        // Delete a promo code.
        public async Task DeletePromoCodeAsync(int promoCodeId)
        {
            var promo = await _context.PromoCodes.FindAsync(promoCodeId);
            if (promo == null)
                throw new Exception("Promo Code not found.");
            _context.PromoCodes.Remove(promo);
            await _context.SaveChangesAsync();
        }
        // Activate a promo code (partial update via HTTP PATCH style).
        public async Task ActivatePromoCodeAsync(int promoCodeId)
        {
            var promo = new PromoCode
            {
                PromoCodeId = promoCodeId,
                IsActive = true,
                UpdatedAt = DateTime.Now
            };
            _context.PromoCodes.Attach(promo);
            _context.Entry(promo).Property(p => p.IsActive).IsModified = true;
            _context.Entry(promo).Property(p => p.UpdatedAt).IsModified = true;
            await _context.SaveChangesAsync();
        }
        // Deactivate a promo code (partial update via HTTP PATCH style).
        public async Task DeactivatePromoCodeAsync(int promoCodeId)
        {
            var promo = new PromoCode
            {
                PromoCodeId = promoCodeId,
                IsActive = false,
                UpdatedAt = DateTime.Now
            };
            _context.PromoCodes.Attach(promo);
            _context.Entry(promo).Property(p => p.IsActive).IsModified = true;
            _context.Entry(promo).Property(p => p.UpdatedAt).IsModified = true;
            await _context.SaveChangesAsync();
        }
        // Search promo codes by code (case-insensitive partial match).
        public async Task<IEnumerable<PromoCode>> SearchPromoCodesByCodeAsync(string code)
        {
            return await _context.PromoCodes
                .Where(p => p.Code.Contains(code))
                .ToListAsync();
        }
        // Retrieve all active promo codes.
        public async Task<IEnumerable<PromoCode>> GetActivePromoCodesAsync()
        {
            return await _context.PromoCodes
                .Where(p => p.IsActive == true)  // Simply check for active promo codes
                .ToListAsync();
        }

        public async Task DeactivateExpiredPromoCodesAsync()
        {
            // Retrieve all active promo codes that have expired.
            var expiredPromos = await _context.PromoCodes
                .Where(p => p.ExpiryDate < DateTime.Now && p.IsActive == true)
                .ToListAsync();

            // Deactivate expired promo codes
            foreach (var promo in expiredPromos)
            {
                promo.IsActive = false;  // Set IsActive to false
            }

            await _context.SaveChangesAsync();  // Save the changes to the database
        }
    }
}

