namespace GreenWashAPI.Models
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        public int OrderId { get; set; }
        public string InvoiceNumber { get; set; }
        public string Carmake { get; set; }   
        public string PackageDetails { get; set; }  
        public string AddOnDetails { get; set; }    
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
