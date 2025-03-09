using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VaccineAPI.BusinessLogic.Services.Interface;
using VaccineAPI.DataAccess.Data;
using VaccineAPI.DataAccess.Models;
using VaccineAPI.Shared.Request;
using VaccineAPI.Shared.Response;

namespace VaccineAPI.BusinessLogic.Services.Implement
{
    public class VaccinationServiceService : IVaccinationServiceService
    {
        private readonly VaccinationTrackingContext _context;

        public VaccinationServiceService(VaccinationTrackingContext context)
        {
            _context = context;
        }

        public async Task<List<VaccinationServiceResponse>> GetAll()
        {
            var vaccinationServices = await _context.VaccinationServices
                .Include(vs => vs.Category)
                .Include(vs => vs.VaccinationServiceVaccinations)
                    .ThenInclude(vsv => vsv.Vaccination)
                        .ThenInclude(v => v.VaccinationDiseases)
                            .ThenInclude(vd => vd.Disease)
                .Select(vs => new VaccinationServiceResponse
                {
                    serviceID = vs.ServiceId,
                    serviceName = vs.ServiceName,
                    categoryId = vs.CategoryId ?? 0,
                    categoryName = vs.Category != null ? vs.Category.Name : "N/A",
                    totalDoses = vs.TotalDoses ?? 0,
                    price = vs.Price,
                    description = vs.Description,
                    Vaccinations = vs.VaccinationServiceVaccinations
                        .Select(vsv => new VaccinationInfo
                        {
                            VaccinationId = vsv.VaccinationId ?? 0,
                            VaccinationName = vsv.Vaccination.VaccinationName,
                            Diseases = vsv.Vaccination.VaccinationDiseases
                                .Select(vd => vd.Disease.DiseaseName)
                                .ToList()
                        })
                        .ToList()
                })
                .ToListAsync();

            return vaccinationServices;
        }

        public async Task<VaccinationServiceResponse> GetById(int id)
        {
            var vaccinationService = await _context.VaccinationServices
                .Where(vs => vs.ServiceId == id)
                .Include(vs => vs.Category)
                .Include(vs => vs.VaccinationServiceVaccinations)
                    .ThenInclude(vsv => vsv.Vaccination)
                        .ThenInclude(v => v.VaccinationDiseases)
                            .ThenInclude(vd => vd.Disease)
        .Select(vs => new VaccinationServiceResponse
        {
            serviceID = vs.ServiceId,
            serviceName = vs.ServiceName,
            categoryId = vs.CategoryId ?? 0,
            categoryName = vs.Category != null ? vs.Category.Name : "N/A",
            totalDoses = vs.TotalDoses ?? 0,
            price = vs.Price,
            description = vs.Description,
            Vaccinations = vs.VaccinationServiceVaccinations
                .Select(vsv => new VaccinationInfo
                {
                    VaccinationId = vsv.VaccinationId ?? 0,
                    VaccinationName = vsv.Vaccination.VaccinationName,
                    Price = (decimal)vsv.Vaccination.Price, 
                    TotalDoses = (int)vsv.Vaccination.TotalDoses,
                    Diseases = vsv.Vaccination.VaccinationDiseases
                        .Select(vd => vd.Disease.DiseaseName)
                        .ToList()
                })
                .ToList()
        })
        .FirstOrDefaultAsync();

            return vaccinationService;
        }

        public async Task<int> Create(VaccinationServiceRequest request)
        {
            var vaccinationService = new DataAccess.Models.VaccinationService
            {
                ServiceName = request.ServiceName,
                CategoryId = request.CategoryId,
                Description = request.Description,
                Price = 0, 
                TotalDoses = 0 
            };

            _context.VaccinationServices.Add(vaccinationService);
            await _context.SaveChangesAsync();
            return vaccinationService.ServiceId;
        }

        public async Task Update(int id, VaccinationServiceRequest request)
        {
            var vaccinationService = await _context.VaccinationServices.FindAsync(id);

            if (vaccinationService == null)
            {
                throw new KeyNotFoundException("VaccinationService không tìm thấy");
            }

            vaccinationService.ServiceName = request.ServiceName;
            vaccinationService.CategoryId = request.CategoryId;
            vaccinationService.Description = request.Description;

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var vaccinationService = await _context.VaccinationServices.FindAsync(id);

            if (vaccinationService == null)
            {
                throw new KeyNotFoundException("VaccinationService không tìm thấy");
            }

            var existingVaccinations = await _context.VaccinationServiceVaccinations
                .Where(vsv => vsv.ServiceId == id)
                .ToListAsync();

            _context.VaccinationServiceVaccinations.RemoveRange(existingVaccinations);
            await _context.SaveChangesAsync();

            _context.VaccinationServices.Remove(vaccinationService);
            await _context.SaveChangesAsync();
        }

        public async Task CreateVaccinationServiceVaccination(VaccinationServiceVaccinationRequest request)
        {
            var vaccination = await _context.Vaccinations.FindAsync(request.VaccinationID);
            if (vaccination == null)
            {
                throw new KeyNotFoundException("Không tìm thấy vaccine với ID " + request.VaccinationID);
            }

            var existingVaccinationServiceVaccination = await _context.VaccinationServiceVaccinations
                .FirstOrDefaultAsync(vsv => vsv.VaccinationId == request.VaccinationID && vsv.ServiceId == request.ServiceID);

            if (existingVaccinationServiceVaccination != null)
            {
                throw new InvalidOperationException($"Vaccine {vaccination.VaccinationName} đã được thêm vào gói này rồi.");
            }
            var vaccinationServiceVaccination = new VaccinationServiceVaccination
            {
                VaccinationId = request.VaccinationID,
                ServiceId = request.ServiceID
            };

            _context.VaccinationServiceVaccinations.Add(vaccinationServiceVaccination);
            await _context.SaveChangesAsync();

           
            await UpdateVaccinationServicePriceAndDoses(request.ServiceID);
        }

        public async Task DeleteVaccinationServiceVaccination(VaccinationServiceVaccinationRequest request)
        {
            var vaccinationServiceVaccination = await _context.VaccinationServiceVaccinations
                .FirstOrDefaultAsync(vsv => vsv.ServiceId == request.ServiceID && vsv.VaccinationId == request.VaccinationID);

            if (vaccinationServiceVaccination == null)
            {
                throw new KeyNotFoundException("Không tìm thấy liên kết VaccinationServiceVaccination.");
            }

            _context.VaccinationServiceVaccinations.Remove(vaccinationServiceVaccination);
            await _context.SaveChangesAsync();

            await UpdateVaccinationServicePriceAndDoses(request.ServiceID);
        }

        private async Task UpdateVaccinationServicePriceAndDoses(int serviceID)
        {
            var service = await _context.VaccinationServices.FindAsync(serviceID);

            if (service == null)
            {
                throw new KeyNotFoundException("VaccinationService không tìm thấy");
            }
            var vaccinations = await _context.VaccinationServiceVaccinations
                .Where(vsv => vsv.ServiceId == serviceID)
                .Include(vsv => vsv.Vaccination) 
                .ToListAsync();

            if (vaccinations != null && vaccinations.Count > 0)
            {
               
                service.TotalDoses = vaccinations.Sum(vsv => vsv.Vaccination.TotalDoses ?? 0);
                service.Price = vaccinations.Sum(vsv => vsv.Vaccination.Price ?? 0);
            }
            else
            {
                service.TotalDoses = 0;
                service.Price = 0;
            }
            await _context.SaveChangesAsync();
        }
    }
}