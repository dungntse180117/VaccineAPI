using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using VaccineAPI.Services;
using VaccineAPI.Shared.Request;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using VaccineAPI.DataAccess.Models; 

namespace VaccineAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountService accountService, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var accounts = await _accountService.GetAllAsync();
                return Ok(accounts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all accounts");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var account = await _accountService.GetByIdAsync(id);

                if (account == null)
                {
                    return NotFound();
                }

                return Ok(account);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting account with id: {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _accountService.UpdateAsync(id, model);
                return NoContent(); // 204 No Content - successful update
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message); // Account not found
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating account with id: {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _accountService.DeleteAsync(id);
                return NoContent(); // 204 No Content - successful delete
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message); // Account not found
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting account with id: {id}");
                return StatusCode(500, "Internal server error");
            }
        }
        
        [HttpPost("CreateAccount")]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountByAdmin model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var newAccount = await _accountService.CreateAccount(model);
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
        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            try
            {
                _logger.LogInformation($"Attempting to get account by email: {email}");

                var account = await _accountService.GetAccountResponseByEmailAsync(email);
                if (account == null)
                {
                    _logger.LogWarning($"Account not found for email: {email}");
                    return NotFound(new { message = "Account not found" });
                }

                _logger.LogInformation($"Account found for email: {email}, AccountId: {account.AccountId}");
                return Ok(account);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving account for email: {email}");
                return StatusCode(500, new { message = "An error occurred while retrieving the account" });
            }
        }
        [HttpPut("{id}/change-password")]
        public async Task<IActionResult> ChangePassword(int id, [FromBody] ChangePasswordRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var success = await _accountService.ChangePasswordAsync(id, model.CurrentPassword, model.NewPassword);
                if (!success)
                {
                    return BadRequest(new { message = "Current password is incorrect" });
                }

                return Ok(new { message = "Password changed successfully" });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Account not found" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error changing password for account {id}");
                return StatusCode(500, new { message = "An unexpected error occurred while changing the password" });
            }
        }
    }
}