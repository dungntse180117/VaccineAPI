using Microsoft.AspNetCore.Mvc;
using VaccineAPI.BusinessLogic.Services.Interface.VaccineAPI.BusinessLogic.Services.Interface;

namespace VaccineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(IDashboardService dashboardService, ILogger<DashboardController> logger)
        {
            _dashboardService = dashboardService;
            _logger = logger;
        }
        [HttpGet("RevenuePerMonth")]
        public async Task<IActionResult> GetRevenuePerMonth(int month)
        {
            try
            {
                if (month < 1 || month > 12)
                {
                    return BadRequest("Invalid month value. It should be between 1 and 12.");
                }

                var result = await _dashboardService.GetRevenuePerMonthAsync(month);
                _logger.LogInformation("Retrieved revenue for month {Month}.", month);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving revenue for month {Month}.", month);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("VisitsPerMonth")]
        public async Task<IActionResult> GetVisitsPerMonth(int month)
        {
            try
            {
                if (month < 1 || month > 12)
                {
                    return BadRequest("Invalid month value. It should be between 1 and 12.");
                }

                var result = await _dashboardService.GetVisitsPerMonthAsync(month);
                _logger.LogInformation("Retrieved visits for month {Month}.", month);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving visits for month {Month}.", month);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("MostPurchasedVaccine")]
        public async Task<IActionResult> GetMostPurchasedVaccine()
        {
            var result = await _dashboardService.GetMostPurchasedVaccineAsync();
            return Ok(result);
        }

        [HttpGet("MostPurchasedPackage")]
        public async Task<IActionResult> GetMostPurchasedPackage()
        {
            var result = await _dashboardService.GetMostPurchasedPackageAsync();
            return Ok(result);
        }
    }
}
