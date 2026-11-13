namespace GreenWashAPI.VMClass
{
    public class PackageVM
    {
        public int PackageId { get; set; }
        public string PackageName { get; set; } 

        public string? PackageDescription { get; set; }

        public decimal Price { get; set; }

         public bool? IsActive { get; set; }
    }
}
