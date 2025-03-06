using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineAPI.Shared.Request;
using VaccineAPI.Shared.Response;

namespace VaccineAPI.BusinessLogic.Services.Interface
{
    public interface IDiseaseService
    {
        Task<List<DiseaseResponse>> GetAllDiseases();
        Task<DiseaseResponse> GetDiseaseById(int id);
        Task<DiseaseResponse> CreateDisease(CreateDiseaseRequest createDiseaseRequest);
        Task<DiseaseResponse> UpdateDisease(int id, UpdateDiseaseRequest updateDiseaseRequest);
        Task<bool> DeleteDisease(int id);
    }
}
