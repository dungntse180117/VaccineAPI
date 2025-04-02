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
using VaccineAPI.BusinessLogic.Services.Interface;

namespace VaccineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VnPayController : ControllerBase
    {
        private readonly IVnPayService _vnPayService;
        private readonly VaccinationTrackingContext _context;
        private readonly ILogger<VnPayController> _logger;
        private readonly IRegistrationService _registrationService;
        public VnPayController(IVnPayService vnPayService, VaccinationTrackingContext context, ILogger<VnPayController> logger, IRegistrationService registrationService)
        {
            _vnPayService = vnPayService;
            _context = context;
            _logger = logger;
            _registrationService = registrationService;
        }

        [HttpPost("CreatePayment")]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentRequest request)
        {
            try
            {
                var registration = await _context.Registrations.FindAsync(request.RegistrationId);
                if (registration == null)
                {
                    return BadRequest("Registration not found.");
                }

                if (registration.Status == "Paid")
                {
                    return BadRequest("Payment already completed.");
                }

                var model = new VnPaymentRequestModel
                {
                    OrderID = registration.RegistrationId,
                    FullName = "Anonymous",
                    Description = "Thanh toán đăng ký tiêm chủng",
                    Amount = (double)registration.TotalAmount,
                    CreatedDate = DateTime.Now
                };

                var paymentUrl = _vnPayService.CreatePaymentUrl(HttpContext, model);
                return Ok(new { paymentUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while creating the payment.");
            }
        }


        [HttpGet("PaymentExecute")]
        public async Task<IActionResult> PaymentExecute()
        {
            try
            {
                var vnpResponseCode = Request.Query["vnp_ResponseCode"];
                var vnpTxnRef = Request.Query["vnp_TxnRef"];
                var vnpAmount = Request.Query["vnp_Amount"];

                if (string.IsNullOrEmpty(vnpTxnRef))
                {
                    return BadRequest("vnp_TxnRef is required.");
                }

                
                if (vnpResponseCode == "00") 
                {
                    // Phân tách vnpTxnRef bằng dấu gạch dưới
                    var orderParts = vnpTxnRef.ToString().Split('_'); 
                    if (!int.TryParse(orderParts[0], out int orderId)) 
                    {
                        return BadRequest("Invalid OrderId format.");
                    }

                    var registration = await _context.Registrations.FindAsync(orderId);
                    if (registration == null)
                    {
                        return BadRequest("Registration not found.");
                    }
                    var updateRequest = new UpdateRegistrationStatusRequest
                    {
                        Status = "Paid" 
                    };

                    // Gọi phương thức UpdateRegistrationStatusAsync
                    var result = await _registrationService.UpdateRegistrationStatusAsync(orderId, updateRequest);
                    registration.TotalAmount = (decimal)(double.Parse(vnpAmount) / 100); 
                    await _context.SaveChangesAsync();

                  
                    return Redirect("http://localhost:5173/payment-success");
                }
                else 
                {
                  
                    return Redirect("http://localhost:5173/payment-failed");
                }
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                _logger.LogError(ex, "An error occurred during payment execution.");
                return StatusCode(500, "An error occurred during payment execution.");
            }
        }
    }
}