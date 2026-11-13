using System.ComponentModel.DataAnnotations;

namespace GreenWashAPI.VMClass
{
    public class UserVM
    {
        
        public int UserId { get; set; }
        [Key]
        public string Fullname { get; set; } = null!;

        public string EmailId { get; set; } = null!;

        public string Password { get; set; } 

        public string PhoneNumber { get; set; } = null!;

        public string? ProfilePicture { get; set; }

        public string? Address { get; set; }

        public decimal? Rating { get; set; }

        public string Role { get; set; } = null!;

        public bool? IsActive { get; set; }
    }


    public class UserDTo
    {
        public string Fullname { get; set; } = null!;

        public string EmailId { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string? Address { get; set; }
    }
}
