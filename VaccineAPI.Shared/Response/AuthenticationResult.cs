using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineAPI.DataAccess.Models;

namespace VaccineAPI.Shared.Response
{
    public class AuthenticationResult
    {
        public AuthenticateResponse? Response { get; set; }
        public string? ErrorMessage { get; set; }
        public bool Success => ErrorMessage == null;
    }
}
