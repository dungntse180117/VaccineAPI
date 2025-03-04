using System.Collections.Generic;
using System.Threading.Tasks;
using VaccineAPI.Shared.Request;
using VaccineAPI.Shared.Response;

namespace VaccineAPI.BusinessLogic.Interface
{
    public interface IVaccineService
    {
        Task<VaccineResponse> CreateVaccineAsync(VaccinationRequest request);
        Task<VaccineResponse> ManageVaccineAsync(int vaccinationId, string action, VaccinationRequest? updateRequest = null);
        Task<IEnumerable<VaccineResponse>> GetAllVaccinesAsync();
        Task<VaccineResponse> GetVaccineAsync(int vaccinationId);
        Task<VaccineResponse> UpdateVaccineAsync(int vaccinationId, VaccinationRequest request); // This to is good
        Task<VaccineResponse> DeleteVaccineAsync(int vaccinationId);
    }
}