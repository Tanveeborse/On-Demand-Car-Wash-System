namespace GreenWashAPI.Models
{
    public class PaymentReceipt
    {
        public int ReceiptId { get; set; }
        public int OrderId { get; set; }
        public string ReceiptNumber { get; set; }
        public string CarMake { get; set; }
        public string PackageDetails { get; set; }
        public string AddOnDetails { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
