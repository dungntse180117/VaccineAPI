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
            Disease disease = await _context.Diseases.FindAsync(id);
            if (disease == null) return false;

            _context.Diseases.Remove(disease);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}