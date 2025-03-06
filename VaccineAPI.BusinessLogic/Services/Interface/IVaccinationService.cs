using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineAPI.Shared.Request;
using VaccineAPI.Shared.Response;

namespace VaccineAPI.BusinessLogic.Services.Interface
{
    public interface IVaccinationService
    {
        Task<List<VaccinationResponse>> GetAllVaccinations();
        Task<VaccinationResponse?> GetVaccinationById(int id); 
        Task<VaccinationResponse?> CreateVaccination(VaccinationRequest vaccinationRequest); 
        Task<VaccinationResponse?> UpdateVaccination(int id, VaccinationRequest vaccinationRequest); 
        Task<bool> DeleteVaccination(int id);
    }
}
