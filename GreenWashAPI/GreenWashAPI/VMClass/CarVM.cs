namespace GreenWashAPI.VMClass
{
    public class CarVM
    {
        public int CarId { get; set; }

        public int? UserId { get; set; }

        public string Carmake { get; set; } = null!;

        public string Carmodel { get; set; } = null!;

        public string? Carimage { get; set; }

        public bool? IsActive { get; set; }
    }
}
