using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VaccineAPI.DataAccess.Models;
using VaccineAPI.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using Microsoft.Extensions.Logging;
using VaccineAPI.Shared.Response;
using VaccineAPI.Shared.Request;
using VaccineAPI.BusinessLogic.Services.Interface; 

namespace VaccineAPI.Services
{
    public class AccountService : IAccountService
    {
        private readonly VaccinationTrackingContext _dbContext;
        private readonly ILogger<AccountService> _logger;
        private readonly IJwtUtils _jwtUtils; 

        public AccountService(VaccinationTrackingContext dbContext, ILogger<AccountService> logger, IJwtUtils jwtUtils) 
        {
            _dbContext = dbContext;
            _logger = logger;
            _jwtUtils = jwtUtils;
        }

        public async Task<AuthenticationResult> AuthenticateAsync(AuthenticateRequest model)
        {
            _logger.LogInformation($"Attempting authentication for email: {model.Email}");

            var account = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.Email == model.Email);

            if (account == null)
            {
                _logger.LogWarning($"Authentication failed: Account not found for email: {model.Email}");
                return new AuthenticationResult { ErrorMessage = "Invalid email or password." };
            }

            if (!BCrypt.Net.BCrypt.Verify(model.Password, account.Password))
            {
                _logger.LogWarning($"Authentication failed: Invalid password for email: {model.Email}");
                return new AuthenticationResult { ErrorMessage = "Invalid email or password." };
            }

            _logger.LogInformation($"Authentication successful for email: {model.Email}");

            // Make JWT
            var token = _jwtUtils.GenerateJwtToken(account);

            // Make AuthenticateResponse
            var authResponse = new AuthenticateResponse(account, token);

            return new AuthenticationResult { Response = authResponse, ErrorMessage = null };
        }

        public async Task<IEnumerable<Account>> GetAllAsync()
        {
            return await _dbContext.Accounts.ToListAsync();
        }

        public async Task<Account?> GetByIdAsync(int id)
        {
            return await _dbContext.Accounts.FindAsync(id);
        }

        public async Task<Account> RegisterAsync(RegisterRequest model)
        {
            _logger.LogInformation($"Registering new account for email: {model.Email}");

            if (await _dbContext.Accounts.AnyAsync(a => a.Email == model.Email))
            {
                _logger.LogError($"Registration failed: Email already registered: {model.Email}");
                throw new ArgumentException("Email already registered.");
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

            var newAccount = new Account
            {
                Name = model.Name,
                Email = model.Email,
                Password = hashedPassword,
                RoleId = 2,// Default Role user = 2
                Phone = model.Phone,
                Address = model.Address,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _dbContext.Accounts.Add(newAccount);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Account registered successfully for email: {model.Email}");
            return newAccount;
        }

        public async Task UpdateAsync(int id, UpdateRequest model)
        {
            _logger.LogInformation($"Updating account with ID: {id}");

            var account = await _dbContext.Accounts.FindAsync(id);
            if (account == null)
            {
                _logger.LogError($"Update failed: Account not found with ID: {id}");
                throw new ArgumentException($"Account not found with ID: {id}");
            }

            // Update properties if provided
            if (!string.IsNullOrEmpty(model.Name))
            {
                account.Name = model.Name;
            }
            if (!string.IsNullOrEmpty(model.Email))
            {
                account.Email = model.Email;
            }
            if (!string.IsNullOrEmpty(model.Phone))
            {
                account.Phone = model.Phone;
            }
            if (!string.IsNullOrEmpty(model.Address))
            {
                account.Address = model.Address;
            }
            if (model.RoleId != null)
            {
                account.RoleId = (int)model.RoleId;
            }

            account.UpdatedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Account with ID: {id} updated successfully.");
        }

        public async Task DeleteAsync(int id)
        {
            _logger.LogInformation($"Deleting account with ID: {id}");

            var account = await _dbContext.Accounts.FindAsync(id);
            if (account == null)
            {
                _logger.LogError($"Delete failed: Account not found with ID: {id}");
                throw new ArgumentException($"Account not found with ID: {id}");
            }

            _dbContext.Accounts.Remove(account);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Account with ID: {id} deleted successfully.");
        }

        public async Task<Account> GetAccountByEmailAsync(string email)
        {
            return await _dbContext.Accounts.FirstOrDefaultAsync(a => a.Email == email);
        }


        // Placeholder implementations for password reset and update.  These *require* integration with a real email service.
        public async Task<bool> SendPasswordResetEmailAsync(string email, string fullName, string newPassword)
        {
            _logger.LogInformation($"Sending password reset email to: {email}");
            // TODO: Implement sending password reset email (using an email service, e.g., SendGrid)

            // For now, just log the information (important for debugging!)
            _logger.LogWarning($"Sending password reset email is not fully implemented.  Email: {email}, Name: {fullName}, New Password: {newPassword}");

            return true; // Indicate success (even though it's not fully implemented)
        }

        public async Task<bool> SendDefaultPasswordAsync(string email, string fullName, string defaultPassword)
        {
            _logger.LogInformation($"Sending default password email to: {email}");
            // TODO: Implement sending default password email (using an email service, e.g., SendGrid)

            // For now, just log the information (important for debugging!)
            _logger.LogWarning($"Sending default password email is not fully implemented.  Email: {email}, Name: {fullName}, Default Password: {defaultPassword}");

            return true; // Indicate success (even though it's not fully implemented)
        }

        public async Task UpdateUserPasswordAsync(Account account, string newPassword)
        {
            _logger.LogInformation($"Updating password for account with ID: {account.AccountId}");

            if (account == null)
            {
                _logger.LogError($"Update password failed: Account is null.");
                throw new ArgumentNullException(nameof(account), "Account cannot be null.");
            }

            if (string.IsNullOrEmpty(newPassword))
            {
                _logger.LogError($"Update password failed: New password cannot be null or empty.");
                throw new ArgumentException("New password cannot be null or empty.");
            }

            account.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            account.UpdatedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Password updated successfully for account with ID: {account.AccountId}");
        }

        public async Task<Account> CreateAsync(Account account)
        {
            _logger.LogInformation($"Creating new account for name: {account.Name}, email: {account.Email}");

            if (account == null)
            {
                _logger.LogError($"Create account failed: Account cannot be null.");
                throw new ArgumentNullException(nameof(account), "Account cannot be null.");
            }


            _dbContext.Accounts.Add(account);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Account created successfully for name: {account.Name}, email: {account.Email}. Account ID: {account.AccountId}");
            return account;

        }

        public async Task<AccountResponse> GetAccountResponseByEmailAsync(string email)
        {
            _logger.LogInformation($"Getting account response for email: {email}");

            var account = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.Email == email);

            if (account == null)
            {
                _logger.LogWarning($"Account not found for email: {email}");
                return null;
            }

            return new AccountResponse
            {
                AccountId = account.AccountId,
                Name = account.Name,
                Email = account.Email,
                RoleId = account.RoleId,
                Phone = account.Phone,
                Address = account.Address
            };
        }

        public async Task<bool> ChangePasswordAsync(int accountId, string currentPassword, string newPassword)
        {
            _logger.LogInformation($"Changing password for account with ID: {accountId}");

            var account = await _dbContext.Accounts.FindAsync(accountId);

            if (account == null)
            {
                _logger.LogError($"Change password failed: Account not found with ID: {accountId}");
                return false;
            }

            if (!BCrypt.Net.BCrypt.Verify(currentPassword, account.Password))
            {
                _logger.LogWarning($"Change password failed: Invalid current password for account with ID: {accountId}");
                return false;
            }

            account.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            account.UpdatedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Password changed successfully for account with ID: {accountId}");
            return true;
        }
    }
}