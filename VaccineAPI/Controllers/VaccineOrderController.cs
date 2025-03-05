//using Microsoft.AspNetCore.Mvc;
//using VaccineAPI.BusinessLogic.Implement;
//using VaccineAPI.Shared.Request;

//namespace VaccineAPI.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class VaccineOrderController : ControllerBase
//    {
//        private readonly IVaccineOrderService _vaccineOrderService;

//        public VaccineOrderController(IVaccineOrderService vaccineOrderService)
//        {
//            _vaccineOrderService = vaccineOrderService;
//        }

//        [HttpPost("order")]
//        public async Task<IActionResult> PlaceOrder([FromBody] OrderVaccineRequest request)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState); // Return validation errors
//            }

//            var response = await _vaccineOrderService.PlaceOrderAsync(request);

//            if (response.IsSuccess)
//            {
//                return Ok(response); // 200 OK with the response
//            }
//            else
//            {
//                return BadRequest(response); // 400 Bad Request with the error message
//            }
//        }
//    }
//}
