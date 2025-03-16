using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineAPI.BusinessLogic.Services.Interface;
using VaccineAPI.DataAccess.Data;
using VaccineAPI.Shared.Request;
using VaccineAPI.Shared.Response;

namespace VaccineAPI.BusinessLogic.Services.Implement
{
    public class VaccinationHistoryService : IVaccinationHistoryService
    {
        private readonly VaccinationTrackingContext _context;

        public VaccinationHistoryService(VaccinationTrackingContext context)
        {
            _context = context;
        }

        public async Task<VaccinationHistoryResponse> GetVaccinationHistoryAsync(int id)
        {
            var history = await _context.VaccinationHistories.FindAsync(id);
            if (history == null) return null;

            return new VaccinationHistoryResponse
            {
                VaccinationHistoryID = history.VaccinationHistoryId,
                VisitID = history.VisitId,
                VaccinationDate = history.VaccinationDate,
                Reaction = history.Reaction,
                VaccineId = history.VaccineId,
                Notes = history.Notes,
                PatientId = history.PatientId
            };
        }

        public async Task<IEnumerable<VaccinationHistoryResponse>> GetVaccinationHistoriesAsync()
        {
            return await _context.VaccinationHistories
                .Select(h => new VaccinationHistoryResponse
                {
                    VaccinationHistoryID = h.VaccinationHistoryId,
                    VisitID = h.VisitId,
                    VaccinationDate = h.VaccinationDate,
                    Reaction = h.Reaction,
                    VaccineId = h.VaccineId,
                    Notes = h.Notes,
                    PatientId = h.PatientId
                })
                .ToListAsync();
        }

        public async Task<IActionResult> UpdateVaccinationHistoryAsync(int id, UpdateVaccinationHistoryRequest request)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var history = await _context.VaccinationHistories.FindAsync(id);
                if (history == null) return new NotFoundResult();        
                history.Reaction = request.Reaction;
                await _context.SaveChangesAsync();
                transaction.Commit();
                return new OkResult();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new StatusCodeResult(500);
            }
        }

        public async Task DeleteVaccinationHistoryAsync(int id)
        {
            var history = await _context.VaccinationHistories.FindAsync(id);
            if (history == null) throw new Exception("Không tìm thấy VaccinationHistory");

            _context.VaccinationHistories.Remove(history);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<VaccinationHistoryResponse>> GetVaccinationHistoriesByPatientIdAsync(int patientId)
        {
            return await _context.VaccinationHistories
                .Where(h => h.PatientId == patientId)
                .Select(h => new VaccinationHistoryResponse
                {
                    VaccinationHistoryID = h.VaccinationHistoryId,
                    VisitID = h.VisitId,
                    VaccinationDate = h.VaccinationDate,
                    Reaction = h.Reaction,
                    VaccineId = h.VaccineId,
                    Notes = h.Notes,
                    PatientId = h.PatientId
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<VaccinationHistoryResponse>> GetVaccinationHistoriesByVisitIdAsync(int visitId)
        {
            return await _context.VaccinationHistories
                .Where(h => h.VisitId == visitId)
                .Select(h => new VaccinationHistoryResponse
                {
                    VaccinationHistoryID = h.VaccinationHistoryId,
                    VisitID = h.VisitId,
                    VaccinationDate = h.VaccinationDate,
                    Reaction = h.Reaction,
                    VaccineId = h.VaccineId,
                    Notes = h.Notes,
                    PatientId = h.PatientId
                })
                .ToListAsync();
        }
    }
}