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
    public interface IRegistrationDetailService
    {
        Task<RegistrationDetailResponse> CreateRegistrationDetailAsync(CreateRegistrationDetailRequest request);
        Task<List<RegistrationDetailResponse>> GetAllRegistrationDetailsAsync();
        Task<RegistrationDetailResponse?> GetRegistrationDetailAsync(int id);
        Task<IActionResult> UpdateRegistrationDetailAsync(int id, UpdateRegistrationDetailRequest request);
        Task<IActionResult> DeleteRegistrationDetailAsync(int id);
    }
}
