using Microsoft.AspNetCore.Mvc;
using VaccineAPI.Services;
using VaccineAPI.DataAccess.Models;
using VaccineAPI.Shared.Response;

namespace VaccineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VnPayController : ControllerBase
    {
        private readonly IVnPayService _vnPayService;

        public VnPayController(IVnPayService vnPayService)
        {
            _vnPayService = vnPayService;
        }

        [HttpPost("create-payment")]
        public IActionResult CreatePayment([FromBody] VnPaymentRequestModel model)
        {
            var paymentUrl = _vnPayService.CreatePaymentUrl(HttpContext, model);
            if (string.IsNullOrEmpty(paymentUrl))
            {
                return BadRequest("Failed to generate payment URL.");
            }

            return Ok(new { PaymentUrl = paymentUrl });
        }

        [HttpGet("payment-return")]
        public IActionResult PaymentReturn()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);
            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}
