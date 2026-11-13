using ClosedXML.Excel;
using GreenWashAPI.Interface;
using GreenWashAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GreenWashAPI.Repository
{
    public class UserRepo:IUsers
    {
        private readonly GreenWashDbContext _context;

        public UserRepo(GreenWashDbContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }
            
        public async Task<User> GetUserByIdAsync(int userid)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserId == userid);
        }
            
        public async Task<User> GetUserByEmailAsync(string email)
        {
            var user = await _context.Users
                             .ToListAsync(); 

            var foundUser = user.FirstOrDefault(u => string.Equals(u.EmailId, email, StringComparison.OrdinalIgnoreCase));
            return foundUser;
            
        }

        public async Task<IEnumerable<User>> GetUsersByRoleAsync(string role)
        {
            var users = await _context.Users.ToListAsync(); 

            var filteredUsers = users
                .Where(u => string.Equals(u.Role, role, StringComparison.OrdinalIgnoreCase))
                .ToList();
            return filteredUsers;
        }

        public async Task<User> AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task UpdateUserAsync(int userId,User user)
        {
            var existingUser = await _context.Users.FindAsync(userId);
            if (existingUser == null)
                throw new Exception("User not found.");
            existingUser.Fullname = user.Fullname;
            existingUser.EmailId = user.EmailId;
            //existingUser.Password = user.Password;
            existingUser.PhoneNumber = user.PhoneNumber;
            //existingUser.ProfilePicture = user.ProfilePicture;
            existingUser.Address = user.Address;
            //existingUser.Rating = user.Rating;
            //existingUser.Role = user.Role;
            //existingUser.IsActive = user.IsActive;
            //existingUser.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                throw new Exception("User not found.");
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
        public async Task<User> AuthenticateUserAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u =>
                u.EmailId.Equals(email, StringComparison.OrdinalIgnoreCase) &&
                u.Password == password);
            if (user == null)
                throw new Exception("Invalid email or password.");
            return user;
        }
        public async Task ChangeUserPasswordAsync(int id, string newPassword)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                throw new Exception("User not found.");
            user.Password = newPassword; // Remember to hash appropriately
            user.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
        }
        public async Task ActivateUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                throw new Exception("User not found.");
            user.IsActive = true;
            user.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
        }
        public async Task DeactivateUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                throw new Exception("User not found.");
            user.IsActive = false;
            user.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
        }
        public async Task UpdateUserProfilePictureAsync(int id, string profilePicturePath)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                throw new Exception("User not found.");
            user.ProfilePicture = profilePicturePath;
            user.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
        }
        public async Task UpdateUserRatingAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                throw new Exception("User not found.");
            if (!user.Role.Equals("Washer", StringComparison.OrdinalIgnoreCase))
                throw new Exception("Rating can only be updated for washers.");

            // Calculate the average rating from the Reviews table for this washer asynchronously.
            var avgRating = await _context.Reviews
                             .Where(r => r.WasherId == id)
                             .Select(r => r.Rating)
                             .DefaultIfEmpty(0)
                             .AverageAsync();
            user.Rating = avgRating;
            user.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> SearchUsersByRoleAsync(string role)
        {
            var users = await _context.Users.ToListAsync(); // Fetch all users

            var filteredUsers = users
                .Where(u => string.Equals(u.Role, role, StringComparison.OrdinalIgnoreCase))  // Case-insensitive comparison
                .ToList();
            return filteredUsers;
        }
        public async Task<IEnumerable<User>> SearchUsersByRatingAsync(decimal minRating, decimal maxRating)
        {
            return await _context.Users
                          .Where(u => u.Rating >= minRating && u.Rating <= maxRating)
                          .ToListAsync();
        }
        public async Task<IEnumerable<User>> GetActiveUsersAsync()
        {
            return await _context.Users
                          .Where(u => (bool)u.IsActive)
                          .ToListAsync();
        }
        public async Task<IEnumerable<User>> GetInactiveUsersAsync()
        {
            return await _context.Users
                          .Where(u => !(bool)u.IsActive)
                          .ToListAsync();
        }
        public async Task<byte[]> ExportWasherReportToExcelAsync()
        {
            // Retrieve all active washers asynchronously.
            var users = await _context.Users.ToListAsync();  // Fetch all users from the database

            var filteredUsers = users
                .Where(u => string.Equals(u.Role, "Washer", StringComparison.OrdinalIgnoreCase) && (bool)u.IsActive)
                .ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Washer Report");
                // Add header row
                worksheet.Cell(1, 1).Value = "Washer ID";
                worksheet.Cell(1, 2).Value = "Name";
                worksheet.Cell(1, 3).Value = "Email";
                worksheet.Cell(1, 4).Value = "Phone Number";
                worksheet.Cell(1, 5).Value = "Rating";
                worksheet.Cell(1, 6).Value = "Created At";
                // Format header row
                var headerRange = worksheet.Range("A1:F1");
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                int row = 2;
                foreach (var washer in filteredUsers)
                {
                    worksheet.Cell(row, 1).Value = washer.UserId;
                    worksheet.Cell(row, 2).Value = washer.Fullname;
                    worksheet.Cell(row, 3).Value = washer.EmailId;
                    worksheet.Cell(row, 4).Value = washer.PhoneNumber;
                    worksheet.Cell(row, 5).Value = washer.Rating;
                    worksheet.Cell(row, 6).Value = washer.CreatedAt;
                    row++;
                }
                // Auto adjust column widths for better readability.
                worksheet.Columns().AdjustToContents();
                // Save the workbook to a MemoryStream and return the byte array.
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }
    }
}
