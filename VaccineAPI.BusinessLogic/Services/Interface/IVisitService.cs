using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineAPI.DataAccess.Models;
using VaccineAPI.Shared.Request;
using VaccineAPI.Shared.Response;

namespace VaccineAPI.BusinessLogic.Services.Interface
{
    public interface IVisitService
    {
        Task<VisitResponse> GetVisitAsync(int id);
        Task<IEnumerable<VisitResponse>> GetVisitsAsync();
        Task<VisitResponse> CreateVisitAsync(CreateVisitRequest request);
        Task UpdateVisitAsync(int id, VisitResponse request);
        Task DeleteVisitAsync(int id);
        Task<IActionResult> UpdateVisitStatusAsync(int id, UpdateVisitStatusRequest request);
        Task<IEnumerable<VisitResponse>> GetVisitsByAppointmentIdAsync(int appointmentId);
        Task<IEnumerable<VisitResponse>> GetVisitsByPatientIdAsync(int patientId);
        Task SendReminderEmailAsync(string email, Visit visit);
        Task SendVisitReminderEmailsAsync();
    }
}
