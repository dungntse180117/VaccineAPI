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

public class VisitService : IVisitService
{
    private readonly VaccinationTrackingContext _context;
    private readonly ILogger _logger;
    public VisitService(VaccinationTrackingContext context)
    {
        _context = context;
    }

    public async Task<VisitResponse> GetVisitAsync(int id)
    {
        var visit = await _context.Visits
            .Include(v => v.VisitVaccinations)
                .ThenInclude(vv => vv.AppointmentVaccination)
                    .ThenInclude(av => av.Vaccination)
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
            VisitVaccinations = visitVaccinations // Gán danh sách vaccine
        };
    }

    public async Task<IEnumerable<VisitResponse>> GetVisitsAsync()
    {
        return await _context.Visits
          .Select(v => new VisitResponse
          {
              VisitID = v.VisitId,
              AppointmentID = v.AppointmentId,
              VisitDate = v.VisitDate,
              Notes = v.Notes,
              Status = v.Status
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
        var visit = await _context.Visits.FindAsync(id);
        if (visit == null) throw new Exception("Không tìm thấy Visit");

        _context.Visits.Remove(visit);
        await _context.SaveChangesAsync();
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
}