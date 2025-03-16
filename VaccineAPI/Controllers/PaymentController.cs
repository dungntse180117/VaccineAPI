//using Microsoft.AspNetCore.Mvc;
//using VaccineAPI.Services;
//using VaccineAPI.Shared.Request;
//using VaccineAPI.BusinessLogic.Implement;
//using VaccineAPI.BusinessLogic.Interface;
//namespace VaccineAPI.Controllers
//{
//    [ApiController]
//    [Route("api/payments")]
//    public class PaymentController : ControllerBase
//    {
//        private readonly IPaymentService _paymentService;

//        public PaymentController(IPaymentService paymentService)
//        {
//            _paymentService = paymentService;
//        }

//        [HttpPost("create")]
//        public async Task<IActionResult> CreatePayment([FromBody] PaymentRequest request)
//        {
//            var (isSuccess, message) = await _paymentService.CreatePaymentAsync(request);

//            if (isSuccess)
//            {
//                return Ok(new { success = true, paymentUrl = message });
//            }

//            return BadRequest(new { success = false, error = message });
//        } 
//    }
//}
