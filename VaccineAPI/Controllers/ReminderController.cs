using Microsoft.AspNetCore.Mvc;
using VaccineAPI.BusinessLogic.Services.Interface; // Thêm namespace này
using Microsoft.Extensions.Logging; // Thêm namespace này

namespace VaccineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReminderController : ControllerBase
    {
        private readonly IVisitService _visitService;
        private readonly ILogger<ReminderController> _logger;

        public ReminderController(IVisitService visitService, ILogger<ReminderController> logger)
        {
            _visitService = visitService;
            _logger = logger;
        }

        [HttpPost("send-reminders")]
        public async Task<IActionResult> SendReminders()
        {
            try
            {
                await _visitService.SendVisitReminderEmailsAsync();
                _logger.LogInformation("Yêu cầu gửi email nhắc nhở thủ công đã được thực hiện.");
                return Ok(new { message = "Đã yêu cầu gửi email nhắc nhở." });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi khi gửi email nhắc nhở thủ công: {ex.Message}");
                return StatusCode(500, new { message = "Lỗi khi gửi email nhắc nhở.", error = ex.Message });
            }
        }
    }
}