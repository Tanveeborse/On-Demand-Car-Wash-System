namespace GreenWashAPI.VMClass
{
    public class AddOnVM
    {
        public int AddonId { get; set; }
        public string AddonName { get; set; } = null!;

        public string? AddonDescription { get; set; }

        public decimal Price { get; set; }

        public bool? IsActive { get; set; }
    }
}
