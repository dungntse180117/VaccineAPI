using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VaccineAPI.DataAccess.Models;
using VaccineAPI.Shared.Request;
using VaccineAPI.Shared.Response;
using AuthenticationResult = VaccineAPI.Shared.Response.AuthenticationResult;

namespace VaccineAPI.Services
{
 
    public interface IAccountService
    {
        Task<AuthenticationResult> AuthenticateAsync(AuthenticateRequest model);
        Task<IEnumerable<Account>> GetAllAsync();
        Task<Account?> GetByIdAsync(int id);
        Task<Account> RegisterAsync(RegisterRequest model);
        Task UpdateAsync(int id, UpdateRequest model);
        Task DeleteAsync(int id);
        Task<Account> GetAccountByEmailAsync(string email);
        Task<bool> SendPasswordResetEmailAsync(string email, string fullName, string newPassword);
        Task UpdateUserPasswordAsync(Account account, string newPassword);
        Task<Account> CreateAsync(Account account);
        Task<AccountResponse> GetAccountResponseByEmailAsync(string email);
        Task<bool> SendDefaultPasswordAsync(string email, string fullName, string defaultPassword);
        Task<bool> ChangePasswordAsync(int accountId, string currentPassword, string newPassword);
    }
}