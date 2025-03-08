using Microsoft.EntityFrameworkCore;
using VaccineAPI.DataAccess.Models;
using VaccineAPI.BusinessLogic.Services.Interface;
using VaccineAPI.Shared.Request;
using VaccineAPI.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VaccineAPI.DataAccess.Data; // Make sure this namespace is correct

namespace VaccineAPI.BusinessLogic.Services
{
    public class DiseaseService : IDiseaseService
    {
        private readonly VaccinationTrackingContext _context;

        public DiseaseService(VaccinationTrackingContext context)
        {
            _context = context;
        }

        public async Task<List<DiseaseResponse>> GetAllDiseases()
        {
            List<DiseaseResponse> diseaseResponses = await _context.Diseases
            .Select(d => new DiseaseResponse
            {
                DiseaseId = d.DiseaseId,
                DiseaseName = d.DiseaseName,
                Description = d.Description
            }).ToListAsync();
            return diseaseResponses;
            
        }

        public async Task<DiseaseResponse> GetDiseaseById(int id)
        {
            Disease disease = await _context.Diseases.FindAsync(id);
            if (disease == null) return null;

            return new DiseaseResponse()
            {
                DiseaseId = disease.DiseaseId,
                DiseaseName = disease.DiseaseName,
                Description = disease.Description
            };
        }

        public async Task<DiseaseResponse> CreateDisease(CreateDiseaseRequest createDiseaseRequest)
        {
            Disease disease = new()
            {
                DiseaseName = createDiseaseRequest.DiseaseName,
                Description = createDiseaseRequest.Description
            };
            _context.Diseases.Add(disease);
            await _context.SaveChangesAsync();

            return new DiseaseResponse()
            {
                DiseaseId = disease.DiseaseId,
                DiseaseName = disease.DiseaseName,
                Description = disease.Description
            };
        }

        public async Task<DiseaseResponse> UpdateDisease(int id, UpdateDiseaseRequest updateDiseaseRequest)
        {
            Disease disease = await _context.Diseases.FindAsync(id);
            if (disease == null) return null;

            disease.Description = updateDiseaseRequest.Description;
            disease.DiseaseName = updateDiseaseRequest.DiseaseName;
            await _context.SaveChangesAsync();

            return new DiseaseResponse()
            {
                DiseaseId = disease.DiseaseId,
                DiseaseName = disease.DiseaseName,
                Description = disease.Description
            };
        }

        public async Task<bool> DeleteDisease(int id)
        {
            try
            {
                Disease disease = await _context.Diseases.FindAsync(id);
                if (disease == null) return false;
       
                var vaccinationDiseases = await _context.VaccinationDiseases
                    .Where(vd => vd.DiseaseId == id)
                    .ToListAsync();

                _context.VaccinationDiseases.RemoveRange(vaccinationDiseases);

                _context.Diseases.Remove(disease);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error deleting disease: {ex}");
                return false;
            }
        }

        public async Task AssociateVaccinationWithDiseases(int vaccinationId, int diseaseId)
        {
            var vaccination = await _context.Vaccinations.FindAsync(vaccinationId);
            if (vaccination == null)
            {
                throw new ArgumentException($"Vaccination với ID {vaccinationId} không tồn tại.");
            }

            var disease = await _context.Diseases.FindAsync(diseaseId);
            if (disease == null)
            {
                throw new ArgumentException($"Disease với ID {diseaseId} không tồn tại.");
            }

            var vaccinationDisease = await _context.VaccinationDiseases
                .FirstOrDefaultAsync(vd => vd.VaccinationId == vaccinationId && vd.DiseaseId == diseaseId);

            if (vaccinationDisease != null)
            {
                throw new InvalidOperationException($"Bệnh {disease.DiseaseName} đã được thêm vào vaccine {vaccination.VaccinationName}.");
            }

            
            var newVaccinationDisease = new VaccinationDisease { VaccinationId = vaccinationId, DiseaseId = diseaseId };
            _context.VaccinationDiseases.Add(newVaccinationDisease);
            await _context.SaveChangesAsync();
        }

        public async Task<List<DiseaseResponse>> GetDiseaseByVaccinationId(int vaccinationId)
        {
            if (vaccinationId <= 0)
            {
                throw new ArgumentException("Vaccination ID phải lớn hơn 0.");
            }
            List<DiseaseResponse> diseaseResponses = await _context.VaccinationDiseases
            .Where(v => v.VaccinationId == vaccinationId)
            .Include(v => v.Disease)
            .Select(v => new DiseaseResponse
            {
                DiseaseId = v.Disease.DiseaseId,
                DiseaseName = v.Disease.DiseaseName,
                Description = v.Disease.Description,
            })
            .ToListAsync();
            return diseaseResponses;
        }
    }
}

