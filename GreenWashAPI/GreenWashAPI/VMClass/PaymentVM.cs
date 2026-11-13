using GreenWashAPI.Models;

namespace GreenWashAPI.VMClass
{
    public class PaymentVM
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "usd";
        public string SuccessUrl { get; set; }
        public string CancelUrl { get; set; }
    }

   
}
