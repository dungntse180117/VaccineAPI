using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineAPI.DataAccess.Models;

namespace VaccineAPI.Shared.Response
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string Token { get; set; }


        public AuthenticateResponse(Account account, string token)
        {
            Id = account.AccountId;
            Name = account.Name;
            Email = account.Email;
            Token = token;
        }
    }
}
