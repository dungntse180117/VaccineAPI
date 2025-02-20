using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaccineAPI.Shared.Response
{
    public class AccountResponse
    {
        public int AccountId { get; set; }
        public string Name { get; set; } 
        public string Email { get; set; } 
        public int RoleId { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }
}
