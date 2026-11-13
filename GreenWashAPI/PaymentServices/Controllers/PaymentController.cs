using Microsoft.AspNetCore.Mvc;
using PaymentServices.Models;
using System.Threading.Tasks;

[Route("api/payment")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly PaymentRepo _paymentService;

    public PaymentController(PaymentRepo paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost("create-checkout-session")]
    public async Task<IActionResult> CreateCheckoutSession([FromBody] PaymentDTO request)
    {
        var sessionUrl = await _paymentService.CreateCheckoutSession(
            request.Amount, request.Currency, request.SuccessUrl, request.CancelUrl);

        return Ok(new { Url = sessionUrl });
    }
}


