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
            var result = await _dashboardService.GetRevenuePerMonthAsync(month);
            return Ok(result);
        }

        [HttpGet("VisitsPerMonth")]
        public async Task<IActionResult> GetVisitsPerMonth(int month)
        {
            var result = await _dashboardService.GetVisitsPerMonthAsync(month);
            return Ok(result);
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
