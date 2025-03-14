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
    public interface IVaccinationHistoryService
    {
        Task<VaccinationHistoryResponse> GetVaccinationHistoryAsync(int id);
        Task<IEnumerable<VaccinationHistoryResponse>> GetVaccinationHistoriesAsync();
        Task<IActionResult> UpdateVaccinationHistoryAsync(int id, UpdateVaccinationHistoryRequest request);
        Task DeleteVaccinationHistoryAsync(int id);
        Task<IEnumerable<VaccinationHistoryResponse>> GetVaccinationHistoriesByPatientIdAsync(int patientId);
        Task<IEnumerable<VaccinationHistoryResponse>> GetVaccinationHistoriesByVisitIdAsync(int visitId);
    }
}
