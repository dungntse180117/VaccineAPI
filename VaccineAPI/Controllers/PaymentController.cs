using Microsoft.AspNetCore.Mvc;
using VaccineAPI.Services;
using VaccineAPI.Shared.Request;

namespace VaccineAPI.Controllers
{
    [ApiController]
    [Route("api/payments")]
    public class PaymentController : ControllerBase
    {
        private readonly IVnPayService _paymentService;

        public PaymentController(IVnPayService paymentService)
        {
            _paymentService = paymentService;
        }

         
    }
}
