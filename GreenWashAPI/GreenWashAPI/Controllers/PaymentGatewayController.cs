using System.Text;
using System.Text.Json;
using GreenWashAPI.VMClass;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace GreenWashAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentGatewayController:ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        public PaymentGatewayController(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
        }
        [HttpPost("create-checkout")]
        public async Task<IActionResult> CreateCheckout([FromBody] PaymentVM request)
        {
            var client = _clientFactory.CreateClient();
            var paymentServiceUrl = _configuration["ServiceUrls:PaymentServ"];
            var payload = JsonSerializer.Serialize(request);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{paymentServiceUrl}create-checkout-session", content);
            if (response.IsSuccessStatusCode)
            {
                //1. send notification
                //2. update payment status in backend
                var contents = await response.Content.ReadAsStringAsync();
                return Ok(contents);
            }
            return StatusCode((int)response.StatusCode, response.ReasonPhrase);
        }
    }
}

