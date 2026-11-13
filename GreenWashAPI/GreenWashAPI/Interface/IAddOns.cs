using GreenWashAPI.Models;

namespace GreenWashAPI.Interface
{
    public interface IAddOns
    {
        Task<IEnumerable<AddOn>> GetAllAddOnsAsync();
        Task<AddOn> GetAddOnByIdAsync(int addOnId);
        Task<AddOn> AddAddOnAsync(AddOn addOn);
        Task UpdateAddOnAsync(AddOn addOn);
        Task DeleteAddOnAsync(int addOnId);
        Task ActivateAddOnAsync(int addOnId);
        Task DeactivateAddOnAsync(int addOnId);
        Task<IEnumerable<AddOn>> SearchAddOnsByNameAsync(string name);
        Task<IEnumerable<AddOn>> GetActiveAddOnsAsync();
    }
}
