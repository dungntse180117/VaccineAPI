using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VaccineAPI.DataAccess.Data;
using VaccineAPI.DataAccess.Models;
using VaccineAPI.BusinessLogic.Services.Interface;
using VaccineAPI.BusinessLogic.Services.Interface.VaccineAPI.BusinessLogic.Services.Interface;

namespace VaccineAPI.BusinessLogic.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly VaccinationTrackingContext _context;

        public DashboardService(VaccinationTrackingContext context)
        {
            _context = context;
        }

        public async Task<Dictionary<int, decimal>> GetRevenuePerMonthAsync(int month)
        {
            return await _context.Registrations
                .Where(r => r.Status == "Confirmed" && r.RegistrationDate.HasValue && r.RegistrationDate.Value.Month == month)
                .GroupBy(r => r.RegistrationDate!.Value.Month)
                .Select(g => new { Month = g.Key, TotalAmount = g.Sum(r => r.TotalAmount) })
                .ToDictionaryAsync(x => x.Month, x => x.TotalAmount);
        }

        public async Task<Dictionary<int, int>> GetVisitsPerMonthAsync(int month)
        {
            return await _context.Visits
                .Where(v => v.VisitDate.HasValue && v.VisitDate.Value.Month == month)
                .GroupBy(v => v.VisitDate.Value.Month)
                .Select(g => new { Month = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Month, x => x.Count);
        }


        public async Task<string> GetMostPurchasedVaccineAsync()
        {
            var result = await _context.RegistrationVaccinations
                .Include(rv => rv.Vaccination)
                .Where(rv => rv.Vaccination != null)
                .GroupBy(rv => rv.VaccinationId)
                .OrderByDescending(g => g.Count())
                .Select(g => g.First().Vaccination!.VaccinationName)
                .FirstOrDefaultAsync();

            return result ?? "No data available";
        }

        public async Task<string> GetMostPurchasedPackageAsync()
        {
            var result = await _context.Registrations
                .Include(r => r.Service)
                .Where(r => r.Service != null)
                .GroupBy(r => r.ServiceId)
                .OrderByDescending(g => g.Count())
                .Select(g => g.First().Service!.ServiceName)
                .FirstOrDefaultAsync();

            return result ?? "No data available";
        }
    }
}
