using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VaccineAPI.DataAccess;
using VaccineAPI.DataAccess.Models;
using VaccineAPI.Services;
using VaccineAPI.Shared.Request;
using Microsoft.EntityFrameworkCore;
using VaccineAPI.DataAccess.Data;

namespace VaccineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VnPayController : ControllerBase
    {
        private readonly IVnPayService _vnPayService;
        private readonly VaccinationTrackingContext _context;
        private readonly ILogger<VnPayController> _logger;

        public VnPayController(IVnPayService vnPayService, VaccinationTrackingContext context, ILogger<VnPayController> logger)
        {
            _vnPayService = vnPayService;
            _context = context;
            _logger = logger;
        }

        [HttpPost("CreatePayment")]
        public async Task<IActionResult> CreatePayment(int registrationId)
        {
            var registration = await _context.Registrations.FindAsync(registrationId);
            if (registration == null)
            {
                return BadRequest("Registration not found.");
            }

            var model = new VnPaymentRequestModel
            {
                OrderID = registration.RegistrationId,
                FullName = "Anonymous", // Update if you want to use account details
                Description = "Thanh toán đăng ký tiêm chủng",
                Amount = (double)registration.TotalAmount,
                CreatedDate = DateTime.Now
            };

            var paymentUrl = _vnPayService.CreatePaymentUrl(HttpContext, model);

            return Ok(new { paymentUrl });
        }

        [HttpGet("PaymentExecute")]
        public IActionResult PaymentExecute()
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
