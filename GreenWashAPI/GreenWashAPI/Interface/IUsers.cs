using GreenWashAPI.Models;

namespace GreenWashAPI.Interface
{
    public interface IUsers
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByEmailAsync(string email);
        Task<IEnumerable<User>> GetUsersByRoleAsync(string role);
        Task<User> AddUserAsync(User user);
        Task UpdateUserAsync(int userId,User user);
        Task DeleteUserAsync(int id);
        Task<User> AuthenticateUserAsync(string email, string password);
        Task ChangeUserPasswordAsync(int id, string newPassword);
        Task ActivateUserAsync(int id);
        Task DeactivateUserAsync(int id);
        Task UpdateUserProfilePictureAsync(int id, string profilePicturePath);
        Task UpdateUserRatingAsync(int id);
        // Additional functionalities:

        // Search operations
        Task<IEnumerable<User>> SearchUsersByRoleAsync(string role);
        Task<IEnumerable<User>> SearchUsersByRatingAsync(decimal minRating, decimal maxRating);
        Task<IEnumerable<User>> GetActiveUsersAsync();
        Task<IEnumerable<User>> GetInactiveUsersAsync();
        Task<byte[]> ExportWasherReportToExcelAsync();
    }
}
