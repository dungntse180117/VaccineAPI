using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineAPI.DataAccess.Models;

namespace VaccineAPI.BusinessLogic.Services.Interface
{
    public interface IJwtUtils
    {
        string GenerateJwtToken(Account account);
        int? ValidateJwtToken(string token);
    }
}
