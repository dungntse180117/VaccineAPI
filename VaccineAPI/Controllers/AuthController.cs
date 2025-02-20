using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using VaccineAPI.Services;
using VaccineAPI.Shared.Request;
using VaccineAPI.Shared.Response;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace VaccineAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAccountService accountService, ILogger<AuthController> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        [AllowAnonymous] 
        [HttpPost("authenticate")] 
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _accountService.AuthenticateAsync(model);

                if (result.ErrorMessage != null) // Use ErrorMessage to check for failure
                {
                    return Unauthorized(result.ErrorMessage);
                }

                // If authentication was successful, return the response:
                return Ok(result.Response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error authenticating user: {model.Email}");
                return StatusCode(500, "Internal server error");
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var newAccount = await _accountService.RegisterAsync(model);
                return Ok(newAccount); // Or return CreatedAtAction
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception here (important for debugging)
                _logger.LogError(ex, $"Error registering user: {model.Email}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}