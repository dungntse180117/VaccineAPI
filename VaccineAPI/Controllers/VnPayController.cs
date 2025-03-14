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

            // Nếu trạng thái đã là "Paid" thì không tạo lại thanh toán nữa
            if (registration.Status == "Paid")
            {
                return BadRequest("Payment already completed.");
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
        public async Task<IActionResult> PaymentExecute([FromServices] IRegistrationService registrationService)
        {
            try
            {
                var vnpAmount = Request.Query["vnp_Amount"];
                var vnpResponseCode = Request.Query["vnp_ResponseCode"];
                var vnpCardType = Request.Query["vnp_CardType"];
                var vnpOrderInfo = Request.Query["vnp_OrderInfo"];
                var vnpTransactionNo = Request.Query["vnp_TransactionNo"];
                var vnpBankTranNo = Request.Query["vnp_BankTranNo"];
                var vnpTxnRef = Request.Query["vnp_TxnRef"];

                if (string.IsNullOrEmpty(vnpTxnRef))
                {
                    _logger.LogError("vnp_TxnRef is missing.");
                    return BadRequest("vnp_TxnRef is required.");
                }

                var response = new VnPaymentResponseModel
                {
                    Success = vnpResponseCode == "00",
                    PaymentMethod = vnpCardType,
                    OrderDescription = vnpOrderInfo,
                    OrderId = vnpTxnRef,
                    PaymentId = vnpTransactionNo,
                    TransactionId = vnpBankTranNo,
                    VnPayResponseCode = vnpResponseCode,
                    Amount = double.Parse(vnpAmount) / 100,  // Convert từ cents sang VND
                    Message = vnpResponseCode == "00" ? "Payment Successful" : "Payment Failed"
                };

                if (response.Success)
                {
                    var orderParts = response.OrderId.Split('_');
                    if (!int.TryParse(orderParts[0], out int orderId))
                    {
                        _logger.LogError($"Invalid OrderId format: {response.OrderId}");
                        return BadRequest("Invalid OrderId format.");
                    }

                    var registration = await _context.Registrations.FindAsync(orderId);
                    if (registration == null)
                    {
                        _logger.LogError($"Registration not found with ID: {orderId}");
                        return BadRequest("Registration not found.");
                    }

                    // Cập nhật trạng thái thành "Paid" trong database
                    registration.Status = "Paid";
                    registration.TotalAmount = (decimal)response.Amount;
                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"Registration {orderId} updated to Paid successfully. PaymentId: {response.PaymentId}, Amount: {response.Amount} VND.");

                    // Gọi phương thức UpdateRegistrationStatusAsync từ service
                    var updateRequest = new UpdateRegistrationStatusRequest { Status = "Confirmed" };
                    var updateResult = await registrationService.UpdateRegistrationStatusAsync(orderId, updateRequest);

                    if (updateResult is StatusCodeResult statusResult && statusResult.StatusCode == 500)
                    {
                        _logger.LogError("Failed to update registration status to 'Confirmed' after payment.");
                    }
                    else
                    {
                        _logger.LogInformation($"Registration {orderId} successfully updated to 'Confirmed'.");
                    }
                }
                else
                {
                    _logger.LogError("Payment failed: " + response.Message);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during PaymentExecute.");
                return StatusCode(500, "An error occurred during payment execution.");
            }
        }


    }
}
