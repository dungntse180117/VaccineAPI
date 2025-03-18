﻿using Microsoft.EntityFrameworkCore;
using VaccineAPI.Shared.Request;
using VaccineAPI.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VaccineAPI.BusinessLogic.Services.Interface;
using VaccineAPI.DataAccess.Data;
using VaccineAPI.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VaccineAPI.BusinessLogic.Services.Implement;
using Microsoft.Extensions.Configuration;

public class VisitService : IVisitService
{
    private readonly VaccinationTrackingContext _context;
    private readonly ILogger<VisitService> _logger;
    private readonly IEmailSender _emailSender; 
    private readonly IConfiguration _configuration;
    public VisitService(VaccinationTrackingContext context, ILogger<VisitService> logger, IEmailSender emailSender, IConfiguration configuration) 
    {
        _context = context;
        _logger = logger;
        _emailSender = emailSender; 
        _configuration = configuration; 
    }

    public async Task<VisitResponse> GetVisitAsync(int id)
    {
        var visit = await _context.Visits
            .Include(v => v.VisitVaccinations)
                .ThenInclude(vv => vv.AppointmentVaccination)
                    .ThenInclude(av => av.Vaccination)
            .Include(v => v.Appointment)
                .ThenInclude(a => a.RegistrationDetail)
                    .ThenInclude(rd => rd.Patient)
            .FirstOrDefaultAsync(v => v.VisitId == id);

        if (visit == null)
        {
            return null;
        }

        var visitVaccinations = visit.VisitVaccinations
            .Select(vv => new VisitVaccinationInfo
            {
                AppointmentVaccinationID = vv.AppointmentVaccinationId,
                VaccinationId = vv.AppointmentVaccination.VaccinationId,
                VaccinationName = vv.AppointmentVaccination.Vaccination.VaccinationName
            })
            .ToList();

        return new VisitResponse
        {
            VisitID = visit.VisitId,
            AppointmentID = visit.AppointmentId,
            VisitDate = visit.VisitDate,
            Notes = visit.Notes,
            Status = visit.Status,
            VisitVaccinations = visitVaccinations,
            PatientName = visit.Appointment.RegistrationDetail.Patient.PatientName,
            PatientPhone = visit.Appointment.RegistrationDetail.Patient.Phone
        };
    }
    public async Task<IEnumerable<VisitResponse>> GetVisitsAsync()
    {
        return await _context.Visits
            .Include(v => v.Appointment)
                .ThenInclude(a => a.RegistrationDetail)
                    .ThenInclude(rd => rd.Patient)
          .Select(v => new VisitResponse
          {
              VisitID = v.VisitId,
              AppointmentID = v.AppointmentId,
              VisitDate = v.VisitDate,
              Notes = v.Notes,
              Status = v.Status,
              PatientName = v.Appointment.RegistrationDetail.Patient.PatientName,
              PatientPhone = v.Appointment.RegistrationDetail.Patient.Phone
          }).ToListAsync();
    }
    public async Task<VisitResponse> CreateVisitAsync(CreateVisitRequest request)
    {
        using var transaction = _context.Database.BeginTransaction();
        try
        {
            // 1. Validation
            var appointment = await _context.Appointments.FindAsync(request.AppointmentID);
            if (appointment == null) throw new Exception("Không tìm thấy Appointment");
            //Kiểm tra xem Appointment đã hoàn thành chưa
            var appointmentVaccinations = await _context.AppointmentVaccinations.Where(av => av.AppointmentId == request.AppointmentID).ToListAsync();
            bool allVaccinesCompleted = !appointmentVaccinations.Any(av => av.Status != "Đã tiêm xong");
            if (allVaccinesCompleted)
            {
                throw new Exception("Appointment đã hoàn thành, không thể tạo thêm Visit");
            }
            //Kiểm tra Appointment Vaccination
            if (request.AppointmentVaccinationIds == null || !request.AppointmentVaccinationIds.Any())
            {
                throw new ArgumentException("Phải chọn ít nhất một Vaccine cần tiêm");
            }

            // 2. Tạo Visit
            var visit = new Visit
            {
                AppointmentId = request.AppointmentID,
                VisitDate = request.VisitDate,
                Notes = request.Notes,
                Status = "Chưa tiêm"
            };

            _context.Visits.Add(visit);
            await _context.SaveChangesAsync();
            //Cập nhật trạng thái Appointment thành Scheduling
            if (appointment.Status != "Đang lên lịch")
            {
                appointment.Status = "Đang lên lịch";
                await _context.SaveChangesAsync();
            }
            // 3. Tạo Visit_Vaccination
            foreach (var appointmentVaccinationId in request.AppointmentVaccinationIds)
            {
                var appointmentVaccination = await _context.AppointmentVaccinations.FindAsync(appointmentVaccinationId);
                if (appointmentVaccination == null)
                {
                    throw new Exception($"Không tìm thấy AppointmentVaccination với ID = {appointmentVaccinationId}");
                }
                //Kiểm tra xem còn mũi nào có thể lên lịch không
                if (appointmentVaccination.DosesScheduled <= 0)
                {
                    throw new Exception($"Vaccine {appointmentVaccination.VaccinationName} đã lên lịch tiêm hết số mũi tiêm ");
                }
                //Giảm DosesScheduled khi lên lịch thành công
                appointmentVaccination.DosesScheduled--;
                appointmentVaccination.Status = "Đang lên lịch";
                var VisitVaccination = new VisitVaccination
                {
                    VisitId = visit.VisitId,
                    AppointmentVaccinationId = appointmentVaccinationId,


                };

                _context.VisitVaccinations.Add(VisitVaccination);

            }
            await _context.SaveChangesAsync();
            //Kiểm tra xem tất cả các AppointmentVaccination đã được lên lịch hết
            appointmentVaccinations = await _context.AppointmentVaccinations.Where(av => av.AppointmentId == request.AppointmentID).ToListAsync();
            bool allDosesScheduled = !appointmentVaccinations.Any(av => av.DosesScheduled > 0);
            if (allDosesScheduled)
            {
                appointment.Status = "Lên lịch hoàn tất";
                await _context.SaveChangesAsync();
            }
            transaction.Commit();
            return new VisitResponse
            {
                VisitID = visit.VisitId,
                AppointmentID = visit.AppointmentId,
                VisitDate = visit.VisitDate,
                Notes = visit.Notes,
                Status = visit.Status
            };
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw;
        }
    }
    public async Task UpdateVisitAsync(int id, VisitResponse request)
    {
        var visit = await _context.Visits.FindAsync(id);
        if (visit == null) throw new Exception("Không tìm thấy Visit");

        visit.VisitDate = request.VisitDate;
        visit.Notes = request.Notes;
        visit.Status = request.Status;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteVisitAsync(int id)
    {
        using var transaction = _context.Database.BeginTransaction();
        try
        {
            var visit = await _context.Visits
                .Include(v => v.VisitVaccinations)
                .ThenInclude(vv => vv.AppointmentVaccination)
                .Include(v => v.Appointment)
                .FirstOrDefaultAsync(v => v.VisitId == id);

            if (visit == null) throw new Exception("Không tìm thấy Visit");

            // Duyệt qua từng VisitVaccination và cập nhật dosesScheduled
            foreach (var visitVaccination in visit.VisitVaccinations)
            {
                var appointmentVaccination = visitVaccination.AppointmentVaccination;
                if (appointmentVaccination != null)
                {
                    appointmentVaccination.DosesScheduled++;
                    _context.AppointmentVaccinations.Update(appointmentVaccination);
                }
            }

            await _context.SaveChangesAsync();

            // Xóa các VisitVaccination liên quan
            if (visit.VisitVaccinations != null && visit.VisitVaccinations.Any())
            {
                _context.VisitVaccinations.RemoveRange(visit.VisitVaccinations);
            }

            // Cập nhật trạng thái của Appointment nếu cần
            if (visit.Appointment != null && visit.Appointment.Status == "Lên lịch hoàn tất")
            {
                visit.Appointment.Status = "Đang lên lịch";
                _context.Appointments.Update(visit.Appointment);
            }

            _context.Visits.Remove(visit);
            await _context.SaveChangesAsync();
            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task<IActionResult> UpdateVisitStatusAsync(int id, UpdateVisitStatusRequest request)
    {
        using var transaction = _context.Database.BeginTransaction();
        try
        {
            var visit = await _context.Visits
                .Include(v => v.VisitVaccinations)
                    .ThenInclude(vv => vv.AppointmentVaccination)
                        .ThenInclude(av => av.Vaccination)
                      .Include(v => v.Appointment)
                          .ThenInclude(ap => ap.RegistrationDetail)
                              .ThenInclude(rd => rd.Patient)

                .FirstOrDefaultAsync(v => v.VisitId == id);

            if (visit == null)
            {
                return new NotFoundResult();
            }

            visit.Status = request.Status; // Cập nhật trạng thái của Visit

            //Nếu status = Da tiêm
            if (request.Status == "Đã tiêm")
            {
                //Duyệt qua tất cả VisitVaccination liên kết với Visit này
                foreach (var visitVaccination in visit.VisitVaccinations)
                {
                    var appointmentVaccination = visitVaccination.AppointmentVaccination;
                    if (appointmentVaccination != null)
                    {
                        //Giảm dosesRemaining
                        appointmentVaccination.DosesRemaining--;
                        //Kiểm tra xem đã tiêm hết số mũi chưa
                        if (appointmentVaccination.DosesRemaining <= 0)
                        {
                            appointmentVaccination.Status = "Đã tiêm hết";
                        }

                        //Tạo bản ghi Vaccination_History
                        var vaccinationHistory = new VaccinationHistory
                        {
                            VisitId = visit.VisitId,
                            VaccinationDate = visit.VisitDate.Value,
                            Reaction = "", //Để trống , có thể thêm sau
                            VaccineId = appointmentVaccination.VaccinationId,
                            Notes = $"Vào ngày {visit.VisitDate.Value:dd/MM/yyyy} đã tiêm vắc xin {appointmentVaccination.Vaccination.VaccinationName}",
                            PatientId = visit.Appointment.RegistrationDetail.Patient.PatientId
                        };
                        _context.VaccinationHistories.Add(vaccinationHistory);
                    }
                }

                //Kiểm tra xem tất cả AppointmentVaccination liên kết với Appointment đã hoàn thành chưa
                var appointment = await _context.Appointments.FindAsync(visit.AppointmentId); //Đã khai báo bên ngoài if
                if (appointment == null)
                {
                    throw new Exception("Không tìm thấy Appointment");
                }
                var appointmentVaccinations = await _context.AppointmentVaccinations
                    .Where(av => av.AppointmentId == visit.AppointmentId)
                    .ToListAsync();

                bool allDosesCompleted = !appointmentVaccinations.Any(av => av.DosesRemaining > 0);
                //Neu da hoan thanh thi doi status appointment thanh Đã hoàn thành lịch tiêm
                if (allDosesCompleted)
                {
                    appointment.Status = "Đã hoàn thành lịch tiêm";
                }
            }
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

    public async Task<IEnumerable<VisitResponse>> GetVisitsByAppointmentIdAsync(int appointmentId)
    {
        return await _context.Visits
            .Include(v => v.VisitVaccinations)
                .ThenInclude(vv => vv.AppointmentVaccination)
                    .ThenInclude(av => av.Vaccination)
            .Where(v => v.AppointmentId == appointmentId)
            .Select(v => new VisitResponse
            {
                VisitID = v.VisitId,
                AppointmentID = v.AppointmentId,
                VisitDate = v.VisitDate,
                Notes = v.Notes,
                Status = v.Status,
                VisitVaccinations = v.VisitVaccinations
                    .Select(vv => new VisitVaccinationInfo
                    {
                        AppointmentVaccinationID = vv.AppointmentVaccinationId,
                        VaccinationId = vv.AppointmentVaccination.VaccinationId,
                        VaccinationName = vv.AppointmentVaccination.Vaccination.VaccinationName
                    })
                    .ToList()
            })
            .ToListAsync();
    }
    public async Task<IEnumerable<VisitResponse>> GetVisitsByPatientIdAsync(int patientId)
    {
        return await _context.Visits
            .Include(v => v.Appointment)
                .ThenInclude(a => a.RegistrationDetail)
                    .ThenInclude(rd => rd.Patient)
            .Where(v => v.Appointment.RegistrationDetail.PatientId == patientId)
            .Select(v => new VisitResponse
            {
                VisitID = v.VisitId,
                AppointmentID = v.AppointmentId,
                VisitDate = v.VisitDate,
                Notes = v.Notes,
                Status = v.Status,
                PatientName = v.Appointment.RegistrationDetail.Patient.PatientName,
                PatientPhone = v.Appointment.RegistrationDetail.Patient.Phone
            }).ToListAsync();
    }
    public async Task SendVisitReminderEmailsAsync()
    {
        // LOGGING: Start of SendVisitReminderEmailsAsync
        _logger.LogInformation($"SendVisitReminderEmailsAsync() started at: {DateTime.Now}");

        // Get visits scheduled for today only
        DateTime today = DateTime.Today;

        var visits = await _context.Visits
            .Include(v => v.Appointment)
                .ThenInclude(a => a.RegistrationDetail)
                    .ThenInclude(rd => rd.Patient)
                        .ThenInclude(p => p.Account) // Include Account here - VERY IMPORTANT for email retrieval!
            .Where(v => v.VisitDate.HasValue && v.VisitDate.Value.Date == today.Date && v.Status != "Đã tiêm") // Chỉ gửi nhắc nhở cho lịch ngày hôm nay
            .ToListAsync();

        // LOGGING: List of Visit IDs to process
        _logger.LogInformation("List of Visit IDs to process:");
        foreach (var visit in visits) // Log Visit IDs before the loop
        {
            _logger.LogInformation($"  Visit ID: {visit.VisitId}");
        }

        foreach (var visit in visits)
        {
            try
            {
                _logger.LogInformation($"Bắt đầu xử lý lịch hẹn ID: {visit.VisitId}");

                var appointment = visit.Appointment;
                if (appointment == null)
                {
                    _logger.LogError($"Lịch hẹn ID: {visit.VisitId} không có Appointment liên kết!");
                    continue;
                }

                var registrationDetail = appointment.RegistrationDetail;
                if (registrationDetail == null)
                {
                    _logger.LogError($"Lịch hẹn ID: {visit.VisitId}, Appointment ID: {appointment.AppointmentId} không có RegistrationDetail liên kết!");
                    continue;
                }

                var patient = registrationDetail.Patient;
                if (patient == null)
                {
                    _logger.LogError($"Lịch hẹn ID: {visit.VisitId}, Appointment ID: {appointment.AppointmentId}, RegistrationDetail ID: {registrationDetail.RegistrationDetailId} không có Patient liên kết!");
                    continue;
                }

                var account = patient.Account;
                if (account == null)
                {
                    _logger.LogError($"Lịch hẹn ID: {visit.VisitId}, Patient ID: {patient.PatientId} không có Account liên kết!");
                    continue;
                }

                string patientEmail = account.Email;

                if (!string.IsNullOrEmpty(patientEmail))
                {
                    await SendReminderEmailAsync(patientEmail, visit);
                }
                else
                {
                    _logger.LogWarning($"Không tìm thấy địa chỉ email cho bệnh nhân {patient.PatientName} (ID: {patient.PatientId}, Account ID: {account.AccountId}).  Không gửi nhắc nhở.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi khi gửi email nhắc nhở cho lịch hẹn {visit.VisitId}: {ex.Message}");
            }
        }
    }

    public async Task SendReminderEmailAsync(string email, Visit visit) // Make sure it's public
    {
        var appointment = await _context.Appointments.FindAsync(visit.AppointmentId);

        // **Lấy email từ bảng Account liên kết với Patient**
        string accountEmail = visit.Appointment.RegistrationDetail.Patient.Account.Email; // Truy cập email qua mối quan hệ

        string subject = "Nhắc nhở lịch tiêm vắc xin";
        string body = $@"
            Chào {visit.Appointment.RegistrationDetail.Patient.PatientName},

            Đây là lời nhắc nhở về lịch tiêm vắc xin của bạn:

            Ngày tiêm: {visit.VisitDate?.ToShortDateString() ?? "Chưa xác định"}

            Vui lòng đến đúng giờ để đảm bảo quá trình tiêm chủng diễn ra suôn sẻ.

            Xin cảm ơn!
        ";

        // LOGGING: Before SendAsync call
        _logger.LogInformation($"Sending email for Visit ID: {visit.VisitId} to: {email}");


        try
        {
            await _emailSender.SendAsync(accountEmail, subject, body); // **Gửi email đến accountEmail**
            _logger.LogInformation($"Email nhắc nhở đã được gửi đến {accountEmail} cho lịch hẹn {visit.VisitId}"); // Log accountEmail
        }
        catch (Exception ex)
        {
            _logger.LogError($"Lỗi khi gửi email đến {accountEmail}: {ex.Message}"); // Log accountEmail
            throw; // Re-throw để xử lý ở tầng cao hơn nếu cần
        }
    }
}
