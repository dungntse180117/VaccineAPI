using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VaccineAPI.BusinessLogic.Services.Interface;
using VaccineAPI.Shared.Request;
using VaccineAPI.Shared.Response;

namespace VaccineAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistrationDetailsController : ControllerBase
    {
        private readonly IRegistrationDetailService _registrationDetailService;
        private readonly ILogger<RegistrationDetailsController> _logger;

        public RegistrationDetailsController(IRegistrationDetailService registrationDetailService, ILogger<RegistrationDetailsController> logger)
        {
            _registrationDetailService = registrationDetailService ?? throw new ArgumentNullException(nameof(registrationDetailService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        public async Task<IActionResult> CreateRegistrationDetail([FromBody] CreateRegistrationDetailRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = await _registrationDetailService.CreateRegistrationDetailAsync(request);
                return CreatedAtAction(nameof(GetRegistrationDetail), new { id = response.RegistrationDetailID }, response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo RegistrationDetail.");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRegistrationDetail(int id)
        {
            try
            {
                var response = await _registrationDetailService.GetRegistrationDetailAsync(id);
                if (response == null)
                {
                    return NotFound();
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi GetRegistrationDetail with ID: {id}.", id);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRegistrationDetails()
        {
            try
            {
                var responses = await _registrationDetailService.GetAllRegistrationDetailsAsync();
                return Ok(responses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi GetAllRegistrationDetails.");
                return StatusCode(500, "Internal Server Error");
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRegistrationDetail(int id, [FromBody] UpdateRegistrationDetailRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                return await _registrationDetailService.UpdateRegistrationDetailAsync(id, request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi UpdateRegistrationDetail with ID: {id}.", id);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegistrationDetail(int id)
        {
            try
            {
                return await _registrationDetailService.DeleteRegistrationDetailAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi DeleteRegistrationDetail with ID: {id}.", id);
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}