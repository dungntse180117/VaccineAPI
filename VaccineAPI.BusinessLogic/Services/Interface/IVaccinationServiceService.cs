using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineAPI.Shared.Request;
using VaccineAPI.Shared.Response;

namespace VaccineAPI.BusinessLogic.Services.Interface
{
    public interface IVaccinationServiceService
    {
        Task<int> Create(VaccinationServiceRequest request);
        Task Update(int id, VaccinationServiceRequest request);
        Task Delete(int id);
        Task<VaccinationServiceResponse> GetById(int id);
        Task<List<VaccinationServiceResponse>> GetAll();

  
        Task CreateVaccinationServiceVaccination(VaccinationServiceVaccinationRequest request);
        Task DeleteVaccinationServiceVaccination(VaccinationServiceVaccinationRequest request);
    }
}
