
using PaymentServices.Models;
using System.Threading.Tasks;
namespace PaymentService.Interfaces
{
    public interface IPaymentService
    {
        //Task<string> ProcessPaymentAsync(PaymentSVM payment);
        Task<string> CreateCheckoutSession(decimal amount, string currency, string successUrl, string cancelUrl);
    }
}