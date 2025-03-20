using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineAPI.Shared.Request;
using VaccineAPI.Shared.Response;

namespace VaccineAPI.BusinessLogic.Services.Interface
{
    public interface IPatientService
    {
        Task<PatientResponse> CreatePatientAsync(CreatePatientRequest request);
        Task<PatientResponse?> GetPatientByIdAsync(int id);
        Task<List<PatientResponse>> GetAllPatientsAsync();
        Task<PatientResponse?> UpdatePatientAsync(int id, UpdatePatientRequest request);
        Task<bool> DeletePatientAsync(int id);
        Task<List<PatientResponse>> GetAllPatientsByAccountIdAsync(int accountId);
        Task<List<PatientResponse>> GetPatientsByPhoneAsync(string phone, int accountId);
    }
}
