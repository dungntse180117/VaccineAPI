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
    public interface IAppointmentService
    {
        Task<AppointmentResponse> GetAppointmentAsync(int id);
        Task<IEnumerable<AppointmentResponse>> GetAppointmentsAsync();
        Task<AppointmentResponse> CreateAppointmentAsync(CreateAppointmentRequest request);
        Task UpdateAppointmentAsync(int id, UpdateAppointmentRequest request);
        Task DeleteAppointmentAsync(int id);
        Task<IEnumerable<AppointmentVaccination>> GetAppointmentVaccinationsAsync(int appointmentId);
        Task<IEnumerable<RegistrationWithAppointmentsResponse>> GetRegistrationsWithAppointmentsByAccountIdAsync(int accountId);
        Task<AppointmentDetailsResponse> GetAppointmentDetailsAsync(int appointmentId);
    }
}