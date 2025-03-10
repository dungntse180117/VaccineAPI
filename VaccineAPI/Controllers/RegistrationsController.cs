using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using VaccineAPI.BusinessLogic.Services.Interface;
using VaccineAPI.Shared.Request;
using VaccineAPI.Shared.Response;

namespace VaccineAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistrationsController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;
        private readonly ILogger<RegistrationsController> _logger;

        public RegistrationsController(IRegistrationService registrationService, ILogger<RegistrationsController> logger)
        {
            _registrationService = registrationService ?? throw new ArgumentNullException(nameof(registrationService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        public async Task<IActionResult> CreateRegistration([FromBody] CreateRegistrationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = await _registrationService.CreateRegistrationAsync(request);
                return CreatedAtAction(nameof(GetRegistration), new { id = response.registrationID }, response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo Registration.");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRegistration(int id)
        {
            try
            {
                var response = await _registrationService.GetRegistrationAsync(id);
                if (response == null)
                {
                    return NotFound();
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi GetRegistration with ID: {id}.", id);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRegistration(int id, [FromBody] UpdateRegistrationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                return await _registrationService.UpdateRegistrationAsync(id, request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi UpdateRegistration with ID: {id}.", id);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegistration(int id)
        {
            try
            {
                return await _registrationService.DeleteRegistrationAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi DeleteRegistration with ID: {id}.", id);
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}