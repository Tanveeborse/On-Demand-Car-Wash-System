using Stripe;
using Stripe.Checkout;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

public class PaymentRepo
{
    private readonly string _secretKey;

    public PaymentRepo(IConfiguration configuration)
    {
        _secretKey = configuration["Stripe:SecretKey"];
        StripeConfiguration.ApiKey = _secretKey;
    }

    public async Task<string> CreateCheckoutSession(decimal amount, string currency, string successUrl, string cancelUrl)
    {
        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string> { "card" },
            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = currency,
                        UnitAmount = (long)(amount * 100), // Convert to cents
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Car Wash Service"
                        }
                    },
                    Quantity = 1
                }
            },
            Mode = "payment",
            SuccessUrl = successUrl,
            CancelUrl = cancelUrl
        };

        var service = new SessionService();
        var session = await service.CreateAsync(options);

        return session.Url; // Redirect the user to this URL for payment
    }
}