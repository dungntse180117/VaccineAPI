using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineAPI.Shared.Request;
using VaccineAPI.Shared.Response;

namespace VaccineAPI.BusinessLogic.Services.Interface
{
    public interface IRegistrationService
    {
        Task<RegistrationResponse> CreateRegistrationAsync(CreateRegistrationRequest request);
        Task<RegistrationResponse?> GetRegistrationAsync(int id);
        Task<IActionResult> UpdateRegistrationInfoAsync(int id, UpdateRegistrationRequest request);
        Task<IActionResult> DeleteRegistrationAsync(int id);
        Task<IActionResult> UpdateRegistrationStatusAsync(int id, UpdateRegistrationStatusRequest request);
        Task CreateRegistrationDetailsAsync(int registrationId);
    }
}

