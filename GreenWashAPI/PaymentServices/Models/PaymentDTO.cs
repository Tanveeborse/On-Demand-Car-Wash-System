using System.ComponentModel.DataAnnotations;

namespace PaymentServices.Models
{
    public class PaymentDTO
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "usd";
        public string SuccessUrl { get; set; }
        public string CancelUrl { get; set; }
    }

}
