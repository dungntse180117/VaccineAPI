using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineAPI.BusinessLogic.Services.Interface;
using VaccineAPI.DataAccess.Data;
using VaccineAPI.DataAccess.Models;
using VaccineAPI.Shared.Request;
using VaccineAPI.Shared.Response;

namespace VaccineAPI.BusinessLogic.Services.Implement
{
    public class VaccinationService : IVaccinationService
    {
        private readonly VaccinationTrackingContext _context;

        public VaccinationService(VaccinationTrackingContext context)
        {
            _context = context;
        }

        public async Task<List<VaccinationResponse>> GetAllVaccinations()
        {
            return await _context.Vaccinations
                .Select(v => new VaccinationResponse
                {
                    VaccinationId = v.VaccinationId,
                    VaccinationName = v.VaccinationName,
                    Manufacturer = v.Manufacturer,
                    TotalDoses = v.TotalDoses,
                    Interval = v.Interval,
                    Price = v.Price,
                    Description = v.Description,
                    MinAge = v.MinAge,
                    MaxAge = v.MaxAge,
                    AgeUnitId = v.AgeUnitId,
                    UnitId = v.UnitId
                })
                .ToListAsync();
        }

        public async Task<VaccinationResponse?> GetVaccinationById(int id)
        {
            var vaccination = await _context.Vaccinations.FindAsync(id);

            if (vaccination == null)
            {
                return null;
            }

            return new VaccinationResponse
            {
                VaccinationId = vaccination.VaccinationId,
                VaccinationName = vaccination.VaccinationName,
                Manufacturer = vaccination.Manufacturer,
                TotalDoses = vaccination.TotalDoses,
                Interval = vaccination.Interval,
                Price = vaccination.Price,
                Description = vaccination.Description,
                MinAge = vaccination.MinAge,
                MaxAge = vaccination.MaxAge,
                AgeUnitId = vaccination.AgeUnitId,
                UnitId = vaccination.UnitId
            };
        }

        public async Task<VaccinationResponse?> CreateVaccination(VaccinationRequest vaccinationRequest)
        {
            try
            {
                var vaccination = new Vaccination
                {
                    VaccinationName = vaccinationRequest.VaccinationName,
                    Manufacturer = vaccinationRequest.Manufacturer,
                    TotalDoses = vaccinationRequest.TotalDoses,
                    Interval = vaccinationRequest.Interval,
                    Price = vaccinationRequest.Price,
                    Description = vaccinationRequest.Description,
                    MinAge = vaccinationRequest.MinAge,
                    MaxAge = vaccinationRequest.MaxAge,
                    AgeUnitId = vaccinationRequest.AgeUnitId,
                    UnitId = vaccinationRequest.UnitId
                };

                _context.Vaccinations.Add(vaccination);
                await _context.SaveChangesAsync();

                return new VaccinationResponse
                {
                    VaccinationId = vaccination.VaccinationId,
                    VaccinationName = vaccination.VaccinationName,
                    Manufacturer = vaccination.Manufacturer,
                    TotalDoses = vaccination.TotalDoses,
                    Interval = vaccination.Interval,
                    Price = vaccination.Price,
                    Description = vaccination.Description,
                    MinAge = vaccination.MinAge,
                    MaxAge = vaccination.MaxAge,
                    AgeUnitId = vaccination.AgeUnitId,
                    UnitId = vaccination.UnitId
                };
            }
            catch
            {
                return null;
            }
        }

        public async Task<VaccinationResponse?> UpdateVaccination(int id, VaccinationRequest vaccinationRequest)
        {
            try
            {
                var vaccination = await _context.Vaccinations.FindAsync(id);

                if (vaccination == null)
                {
                    return null;
                }

                vaccination.VaccinationName = vaccinationRequest.VaccinationName;
                vaccination.Manufacturer = vaccinationRequest.Manufacturer;
                vaccination.TotalDoses = vaccinationRequest.TotalDoses;
                vaccination.Interval = vaccinationRequest.Interval;
                vaccination.Price = vaccinationRequest.Price;
                vaccination.Description = vaccinationRequest.Description;
                vaccination.MinAge = vaccinationRequest.MinAge;
                vaccination.MaxAge = vaccinationRequest.MaxAge;
                vaccination.AgeUnitId = vaccinationRequest.AgeUnitId;
                vaccination.UnitId = vaccinationRequest.UnitId;

                await _context.SaveChangesAsync();

                return new VaccinationResponse
                {
                    VaccinationId = vaccination.VaccinationId,
                    VaccinationName = vaccination.VaccinationName,
                    Manufacturer = vaccination.Manufacturer,
                    TotalDoses = vaccination.TotalDoses,
                    Interval = vaccination.Interval,
                    Price = vaccination.Price,
                    Description = vaccination.Description,
                    MinAge = vaccination.MinAge,
                    MaxAge = vaccination.MaxAge,
                    AgeUnitId = vaccination.AgeUnitId,
                    UnitId = vaccination.UnitId
                };
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> DeleteVaccination(int id)
        {
            try
            {
                var vaccination = await _context.Vaccinations.FindAsync(id);

                if (vaccination == null)
                {
                    return false;
                }

                // *XÓA CÁC LIÊN KẾT VACCINATION_DISEASE*
                var vaccinationDiseases = await _context.VaccinationDiseases
                    .Where(vd => vd.VaccinationId == id)
                    .ToListAsync();

                _context.VaccinationDiseases.RemoveRange(vaccinationDiseases);


                //  Xóa VaccinationImage và Image
                var vaccinationImages = await _context.VaccinationImages
                    .Where(vi => vi.VaccinationId == id)
                    .ToListAsync();

                foreach (var vi in vaccinationImages)
                {
                    int imgId = vi.ImgId;

                    _context.VaccinationImages.Remove(vi);

                    var anyOtherVaccinationUsingThisImage = await _context.VaccinationImages
                        .AnyAsync(x => x.ImgId == imgId && x.VaccinationId != id);

                    if (!anyOtherVaccinationUsingThisImage)
                    {
                        var image = await _context.Images.FindAsync(imgId);
                        if (image != null)
                        {
                            _context.Images.Remove(image);
                        }
                    }
                }

                // Cuối cùng, xóa Vaccination
                _context.Vaccinations.Remove(vaccination);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error deleting vaccination: {ex}");
                return false;
            }
        }
    }
}
