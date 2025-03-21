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
    public class AppointmentService : IAppointmentService
    {
        private readonly VaccinationTrackingContext _context;

        public AppointmentService(VaccinationTrackingContext context)
        {
            _context = context;
        }

        public async Task<AppointmentResponse> GetAppointmentAsync(int id)
        {
            var appointment = await _context.Appointments
                .Include(a => a.RegistrationDetail)
                    .ThenInclude(rd => rd.Patient)
                .FirstOrDefaultAsync(a => a.AppointmentId == id);

            if (appointment == null)
            {
                return null;
            }

            return new AppointmentResponse
            {
                AppointmentID = appointment.AppointmentId,
                RegistrationDetailID = appointment.RegistrationDetailId,
                AppointmentDate = appointment.AppointmentDate,
                ConfigId = appointment.ConfigId,
                AppointmentNumber = appointment.AppointmentNumber,
                Status = appointment.Status,
                Notes = appointment.Notes,
                PatientId = appointment.RegistrationDetail.Patient.PatientId,
                PatientName = appointment.RegistrationDetail.Patient.PatientName
            };
        }

        public async Task<IEnumerable<AppointmentResponse>> GetAppointmentsAsync()
        {
            return await _context.Appointments
                .Include(a => a.RegistrationDetail)
                    .ThenInclude(rd => rd.Patient)
                .Select(appointment => new AppointmentResponse
                {
                    AppointmentID = appointment.AppointmentId,
                    RegistrationDetailID = appointment.RegistrationDetailId,
                    AppointmentDate = appointment.AppointmentDate,
                    ConfigId = appointment.ConfigId,
                    AppointmentNumber = appointment.AppointmentNumber,
                    Status = appointment.Status,
                    Notes = appointment.Notes,
                    PatientId = appointment.RegistrationDetail.Patient.PatientId,
                    PatientName = appointment.RegistrationDetail.Patient.PatientName
                })
                .ToListAsync();
        }


        public async Task<AppointmentResponse> CreateAppointmentAsync(CreateAppointmentRequest request)
        {
            // 1. Tạo Appointment
            var appointment = new Appointment
            {
                RegistrationDetailId = request.RegistrationDetailID,
                AppointmentDate = request.AppointmentDate,
                ConfigId = 1,
                AppointmentNumber = request.AppointmentNumber,
                Status = "Chưa được lên lịch", // Trạng thái mặc định
                Notes = request.Notes
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            // 2. Lấy RegistrationDetail
            var registrationDetail = await _context.RegistrationDetails
                .Include(rd => rd.Registration)
                    .ThenInclude(r => r.RegistrationVaccinations)
                        .ThenInclude(rv => rv.Vaccination)
                .FirstOrDefaultAsync(rd => rd.RegistrationDetailId == request.RegistrationDetailID);

            if (registrationDetail == null)
            {
                throw new Exception("Không tìm thấy RegistrationDetail");
            }

            // 3. Lấy danh sách VaccinationIDs từ Registration (hoặc VaccinationService)
            List<int> vaccinationIds = new List<int>();

            if (registrationDetail.Registration.ServiceId == null) // Tiêm lẻ
            {
                vaccinationIds = registrationDetail.Registration.RegistrationVaccinations
                    .Where(rv => rv.VaccinationId.HasValue)
                    .Select(rv => rv.VaccinationId.Value)
                    .ToList();
            }
            else // Gói tiêm chủng
            {
                // Lấy VaccinationService riêng
                var vaccinationService = await _context.VaccinationServices
                    .Include(vs => vs.VaccinationServiceVaccinations)
                        .ThenInclude(vsv => vsv.Vaccination)
                    .FirstOrDefaultAsync(vs => vs.ServiceId == registrationDetail.Registration.ServiceId);

                if (vaccinationService == null)
                {
                    throw new Exception("Không tìm thấy VaccinationService");
                }

                vaccinationIds = vaccinationService.VaccinationServiceVaccinations
                    .Select(vsv => vsv.VaccinationId.Value)
                    .ToList();
            }


            // 4. Tạo Appointment_Vaccination cho từng vaccine
            foreach (var vaccinationId in vaccinationIds)
            {
                var vaccine = await _context.Vaccinations.FindAsync(vaccinationId);

                if (vaccine != null)
                {
                    var appointmentVaccination = new AppointmentVaccination
                    {
                        AppointmentId = appointment.AppointmentId,
                        VaccinationId = vaccinationId,
                        TotalDoses = (int)vaccine.TotalDoses,
                        DosesRemaining = (int)vaccine.TotalDoses,
                        Status = "Chưa lên lịch",
                        DosesScheduled = (int)vaccine.TotalDoses,
                        VaccinationName = vaccine.VaccinationName
                    };
                    registrationDetail.Status = "Đã tạo lịch tổng quát";
                    _context.AppointmentVaccinations.Add(appointmentVaccination);
                }
            }

            await _context.SaveChangesAsync();

            return new AppointmentResponse
            {
                AppointmentID = appointment.AppointmentId,
                RegistrationDetailID = appointment.RegistrationDetailId,
                AppointmentDate = appointment.AppointmentDate,
                ConfigId = appointment.ConfigId,
                AppointmentNumber = appointment.AppointmentNumber,
                Status = appointment.Status,
                Notes = appointment.Notes
            };
        }

        public async Task UpdateAppointmentAsync(int id, UpdateAppointmentRequest request)
        {
            var appointment = await _context.Appointments.FindAsync(id);

            if (appointment == null)
            {
                throw new Exception("Không tìm thấy Appointment");
            }

            appointment.AppointmentDate = request.AppointmentDate;
            appointment.ConfigId = request.ConfigId;
            appointment.AppointmentNumber = request.AppointmentNumber;
            appointment.Status = request.Status;
            appointment.Notes = request.Notes;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAppointmentAsync(int id)
        {
  
            var appointment = await _context.Appointments.FindAsync(id);

            if (appointment == null)
            {
                throw new Exception("Không tìm thấy Appointment");
            }

    
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
 
                var visits = await _context.Visits
                    .Where(v => v.AppointmentId == id)
                    .ToListAsync();

                foreach (var visit in visits)
                {
                    var vaccinationHistories = await _context.VaccinationHistories
                        .Where(vh => vh.VisitId == visit.VisitId)
                        .ToListAsync();

                    foreach (var vaccinationHistory in vaccinationHistories)
                    {
                        var feedbacks = await _context.Feedbacks
                            .Where(f => f.VaccinationHistoryId == vaccinationHistory.VaccinationHistoryId)
                            .ToListAsync();

                        _context.Feedbacks.RemoveRange(feedbacks);
                    }
                   
                    _context.VaccinationHistories.RemoveRange(vaccinationHistories);
                    
                    var visitDayChangeRequests = await _context.VisitDayChangeRequests
                        .Where(vdcr => vdcr.VisitId == visit.VisitId)
                        .ToListAsync();

                    _context.VisitDayChangeRequests.RemoveRange(visitDayChangeRequests);

                    var visitVaccinations = await _context.VisitVaccinations
                        .Where(vv => vv.VisitId == visit.VisitId)
                        .ToListAsync();

                    _context.VisitVaccinations.RemoveRange(visitVaccinations);
                }
                _context.Visits.RemoveRange(visits);

                var appointmentVaccinations = await _context.AppointmentVaccinations
                    .Where(av => av.AppointmentId == id)
                    .ToListAsync();

                _context.AppointmentVaccinations.RemoveRange(appointmentVaccinations);

                _context.Appointments.Remove(appointment);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Lỗi khi xóa Appointment: " + ex.Message, ex);
            }
        }
        public async Task<IEnumerable<AppointmentVaccination>> GetAppointmentVaccinationsAsync(int appointmentId)
        {
            return await _context.AppointmentVaccinations
                .Include(av => av.Vaccination)
                .Where(av => av.AppointmentId == appointmentId)
                .Select(av => new AppointmentVaccination
                {
                    AppointmentVaccinationId = av.AppointmentVaccinationId,
                    AppointmentId = av.AppointmentId,
                    VaccinationId = av.VaccinationId,
                    TotalDoses = av.TotalDoses,
                    DosesRemaining = av.DosesRemaining,
                    Status = av.Status,
                    DosesScheduled = av.DosesScheduled,
                    VaccinationName = av.VaccinationName
                })
                .ToListAsync();
        }
        public async Task<IEnumerable<RegistrationWithAppointmentsResponse>> GetRegistrationsWithAppointmentsByAccountIdAsync(int accountId)
        {
            var registrations = await _context.Registrations
                .Where(r => r.AccountId == accountId)
                .Include(r => r.RegistrationPatients)
                    .ThenInclude(rp => rp.Patient)
                .ToListAsync();

            var result = new List<RegistrationWithAppointmentsResponse>();

            foreach (var registration in registrations)
            {
                var appointments = await _context.Appointments
                    .Include(a => a.RegistrationDetail)
                        .ThenInclude(rd => rd.Patient)
                    .Include(a => a.AppointmentVaccinations)
                        .ThenInclude(av => av.Vaccination)
                    .Where(a => a.RegistrationDetail.RegistrationId == registration.RegistrationId)
                    .ToListAsync();

                var appointmentDetailsList = new List<AppointmentDetails>();

                foreach (var appointment in appointments)
                {
                    var appointmentVaccinationDetailsList = appointment.AppointmentVaccinations
                        .Select(av => new AppointmentVaccinationDetails
                        {
                            AppointmentVaccinationID = av.AppointmentVaccinationId,
                            VaccinationId = av.VaccinationId,
                            VaccinationName = av.Vaccination.VaccinationName,
                            TotalDoses = av.TotalDoses,
                            DosesRemaining = av.DosesRemaining,
                            Status = av.Status
                        })
                        .ToList();

                    var appointmentDetails = new AppointmentDetails
                    {
                        AppointmentID = appointment.AppointmentId,
                        AppointmentDate = appointment.AppointmentDate,
                        Status = appointment.Status,
                        Notes = appointment.Notes,
                        PatientId = appointment.RegistrationDetail.Patient.PatientId,
                        PatientName = appointment.RegistrationDetail.Patient.PatientName,
                        AppointmentVaccinations = appointmentVaccinationDetailsList
                    };
                    appointmentDetailsList.Add(appointmentDetails);
                }

                var registrationResponse = new RegistrationWithAppointmentsResponse
                {
                    RegistrationId = registration.RegistrationId,
                    AccountId = registration.AccountId,
                    RegistrationDate = (DateTime)registration.RegistrationDate,
                    TotalAmount = registration.TotalAmount,
                    Status = registration.Status,
                    DesiredDate = registration.DesiredDate,
                    AppointmentDetails = appointmentDetailsList
                };
                result.Add(registrationResponse);
            }
            return result;
        }
        public async Task<AppointmentDetailsResponse> GetAppointmentDetailsAsync(int appointmentId)
        {
            var appointment = await _context.Appointments
                .Include(a => a.RegistrationDetail)
                    .ThenInclude(rd => rd.Patient)
                .Include(a => a.AppointmentVaccinations)
                    .ThenInclude(av => av.Vaccination)
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);

            if (appointment == null)
            {
                return null;
            }

            var appointmentVaccinationDetailsList = appointment.AppointmentVaccinations
                .Select(av => new AppointmentVaccinationDetails
                {
                    AppointmentVaccinationID = av.AppointmentVaccinationId,
                    VaccinationId = av.VaccinationId,
                    VaccinationName = av.Vaccination.VaccinationName,
                    TotalDoses = av.TotalDoses,
                    DosesRemaining = av.DosesRemaining,
                    Status = av.Status,
                    DosesScheduled = av.DosesScheduled,
                    
                })
                .ToList();

            return new AppointmentDetailsResponse
            {
                AppointmentID = appointment.AppointmentId,
                AppointmentDate = appointment.AppointmentDate,
                Status = appointment.Status,
                Notes = appointment.Notes,
                PatientId = appointment.RegistrationDetail.Patient.PatientId,
                PatientName = appointment.RegistrationDetail.Patient.PatientName,
                AppointmentVaccinations = appointmentVaccinationDetailsList
            };
        }
    }
}