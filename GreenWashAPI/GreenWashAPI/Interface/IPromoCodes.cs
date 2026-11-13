using GreenWashAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace GreenWashAPI.Interface
{
    public interface IPromoCodes
    {
        Task<IEnumerable<PromoCode>> GetAllPromoCodesAsync();
        Task<PromoCode> GetPromoCodeByIdAsync(int promoCodeId);
        Task<PromoCode> AddPromoCodeAsync(PromoCode promoCode);
        Task UpdatePromoCodeAsync(PromoCode promoCode);
        Task DeletePromoCodeAsync(int promoCodeId);
        Task ActivatePromoCodeAsync(int promoCodeId);
        Task DeactivatePromoCodeAsync(int promoCodeId);
        Task<IEnumerable<PromoCode>> SearchPromoCodesByCodeAsync(string code);
        Task<IEnumerable<PromoCode>> GetActivePromoCodesAsync();
        Task DeactivateExpiredPromoCodesAsync();
    }
}
