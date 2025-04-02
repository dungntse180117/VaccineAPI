using Microsoft.EntityFrameworkCore;
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
            var appointment = await _context.Appointments.FindAsync(request.AppointmentID);
            if (appointment == null) throw new Exception("Không tìm thấy Appointment");
            var appointmentVaccinations = await _context.AppointmentVaccinations.Where(av => av.AppointmentId == request.AppointmentID).ToListAsync();
            bool allVaccinesCompleted = !appointmentVaccinations.Any(av => av.Status != "Đã tiêm xong");
            if (allVaccinesCompleted)
            {
                throw new Exception("Appointment đã hoàn thành, không thể tạo thêm Visit");
            }
            if (request.AppointmentVaccinationIds == null || !request.AppointmentVaccinationIds.Any())
            {
                throw new ArgumentException("Phải chọn ít nhất một Vaccine cần tiêm");
            }

            var visit = new Visit
            {
                AppointmentId = request.AppointmentID,
                VisitDate = request.VisitDate,
                Notes = request.Notes,
                Status = "Chưa tiêm"
            };

            _context.Visits.Add(visit);
            await _context.SaveChangesAsync();

            if (appointment.Status != "Đang lên lịch")
            {
                appointment.Status = "Đang lên lịch";
                await _context.SaveChangesAsync();
            }
   
            foreach (var appointmentVaccinationId in request.AppointmentVaccinationIds)
            {
                var appointmentVaccination = await _context.AppointmentVaccinations.FindAsync(appointmentVaccinationId);
                if (appointmentVaccination == null)
                {
                    throw new Exception($"Không tìm thấy AppointmentVaccination với ID = {appointmentVaccinationId}");
                }
           
                if (appointmentVaccination.DosesScheduled <= 0)
                {
                    throw new Exception($"Vaccine {appointmentVaccination.VaccinationName} đã lên lịch tiêm hết số mũi tiêm ");
                }
      
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

            if (visit.VisitVaccinations != null && visit.VisitVaccinations.Any())
            {
                _context.VisitVaccinations.RemoveRange(visit.VisitVaccinations);
            }

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
                                  .ThenInclude(p => p.Account) 

                .FirstOrDefaultAsync(v => v.VisitId == id);

            if (visit == null)
            {
                return new NotFoundResult();
            }

            visit.Status = request.Status; 


            if (request.Status == "Đã tiêm")
            {

                foreach (var visitVaccination in visit.VisitVaccinations)
                {
                    var appointmentVaccination = visitVaccination.AppointmentVaccination;
                    if (appointmentVaccination != null)
                    {
     
                        appointmentVaccination.DosesRemaining--;
      
                        if (appointmentVaccination.DosesRemaining <= 0)
                        {
                            appointmentVaccination.Status = "Đã tiêm hết";
                        }

         
                        var vaccinationHistory = new VaccinationHistory
                        {
                            VisitId = visit.VisitId,
                            VaccinationDate = visit.VisitDate.Value,
                            Reaction = "", 
                            VaccineId = appointmentVaccination.VaccinationId,
                            Notes = $"Vào ngày {visit.VisitDate.Value:dd/MM/yyyy} đã tiêm vắc xin {appointmentVaccination.Vaccination.VaccinationName}",
                            PatientId = visit.Appointment.RegistrationDetail.Patient.PatientId
                        };
                        _context.VaccinationHistories.Add(vaccinationHistory);
                    }
                }

                var appointment = await _context.Appointments.FindAsync(visit.AppointmentId); 
                if (appointment == null)
                {
                    throw new Exception("Không tìm thấy Appointment");
                }
                var appointmentVaccinations = await _context.AppointmentVaccinations
                    .Where(av => av.AppointmentId == visit.AppointmentId)
                    .ToListAsync();

                bool allDosesCompleted = !appointmentVaccinations.Any(av => av.DosesRemaining > 0);
       
                if (allDosesCompleted)
                {
                    appointment.Status = "Đã hoàn thành lịch tiêm";
                }

                try
                {
                    string feedbackPageUrl = $"{_configuration["Frontend_URL"]}/feedback?visitId={visit.VisitId}&accountId={visit.Appointment.RegistrationDetail.Patient.AccountId}";
                    string patientEmail = visit.Appointment.RegistrationDetail.Patient.Account.Email;
                    if (!string.IsNullOrEmpty(patientEmail))
                    {
                        await SendFeedbackEmailAsync(patientEmail, visit, feedbackPageUrl);
                    }
                    else
                    {
                        _logger.LogWarning($"Không tìm thấy email cho AccountId: {visit.Appointment.RegistrationDetail.Patient.AccountId} để gửi email đánh giá.");
                    }
                }
                catch (Exception emailEx)
                {
                    _logger.LogError($"Lỗi khi gửi email đánh giá cho VisitId: {visit.VisitId}, Error: {emailEx.Message}");
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
        _logger.LogInformation($"SendVisitReminderEmailsAsync() started at: {DateTime.Now}");
        DateTime today = DateTime.Today;

        var visits = await _context.Visits
            .Include(v => v.Appointment)
                .ThenInclude(a => a.RegistrationDetail)
                    .ThenInclude(rd => rd.Patient)
                        .ThenInclude(p => p.Account) 
            .Where(v => v.VisitDate.HasValue && v.VisitDate.Value.Date == today.Date && v.Status != "Đã tiêm") 
            .ToListAsync();

        _logger.LogInformation("List of Visit IDs to process:");
        foreach (var visit in visits) 
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
    private async Task SendFeedbackEmailAsync(string patientEmail, Visit visit, string feedbackPageUrl)
    {
        string subject = "Mời bạn đánh giá dịch vụ tiêm chủng";
        string body = $@"
            Chào {visit.Appointment.RegistrationDetail.Patient.PatientName},cảm ơn bạn đã sử dụng dịch vụ tiêm chủng của chúng tôi.Chúng tôi rất mong nhận được đánh giá của bạn về trải nghiệm tiêm chủng vừa qua để chúng tôi có thể cải thiện chất lượng dịch vụ.Vui lòng nhấp vào đường dẫn sau để đánh giá: <a href='{feedbackPageUrl}'>{feedbackPageUrl} </a>  Xin cảm ơn vì sự hợp tác của bạn!";

        _logger.LogInformation($"Sending feedback email for Visit ID: {visit.VisitId} to: {patientEmail}, Feedback URL: {feedbackPageUrl}");

        try
        {
            await _emailSender.SendAsync(patientEmail, subject, body);
            _logger.LogInformation($"Feedback email sent successfully to {patientEmail} for Visit ID: {visit.VisitId}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error sending feedback email to {patientEmail} for Visit ID: {visit.VisitId}: {ex.Message}");
            throw; 
        }
    }

    public async Task SendReminderEmailAsync(string email, Visit visit) 
    {
        var appointment = await _context.Appointments.FindAsync(visit.AppointmentId);


        string accountEmail = visit.Appointment.RegistrationDetail.Patient.Account.Email;

        string subject = "Nhắc nhở lịch tiêm vắc xin";
        string body = $@"
            Chào {visit.Appointment.RegistrationDetail.Patient.PatientName},

            Đây là lời nhắc nhở về lịch tiêm vắc xin của bạn. Ngày tiêm: {visit.VisitDate?.ToShortDateString() ?? "Chưa xác định"}.Vui lòng đến đúng giờ để đảm bảo quá trình tiêm chủng diễn ra suôn sẻ. Xin cảm ơn!";

        _logger.LogInformation($"Sending email for Visit ID: {visit.VisitId} to: {email}");


        try
        {
            await _emailSender.SendAsync(accountEmail, subject, body); // **Gửi email đến accountEmail**
            _logger.LogInformation($"Email nhắc nhở đã được gửi đến {accountEmail} cho lịch hẹn {visit.VisitId}"); 
        }
        catch (Exception ex)
        {
            _logger.LogError($"Lỗi khi gửi email đến {accountEmail}: {ex.Message}"); 
            throw; 
        }
    }
}
