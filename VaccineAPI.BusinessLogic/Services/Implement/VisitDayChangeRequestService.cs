using Microsoft.AspNetCore.Mvc;
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
    public class VisitDayChangeRequestService : IVisitDayChangeRequestService
    {
        private readonly VaccinationTrackingContext _context;

        public VisitDayChangeRequestService(VaccinationTrackingContext context)
        {
            _context = context;
        }

        public async Task<VisitDayChangeRequestResponse> GetVisitDayChangeRequestAsync(int id)
        {
            var request = await _context.VisitDayChangeRequests.FindAsync(id);

            if (request == null)
            {
                return null;
            }

            return new VisitDayChangeRequestResponse
            {
                ChangeRequestId = request.ChangeRequestId,
                VisitID = request.VisitId,
                PatientId = request.PatientId,
                RequestedDate = request.RequestedDate,
                Reason = request.Reason,
                Status = request.Status,
                StaffNotes = request.StaffNotes,
                RequestedDateAt = request.RequestedDateAt
            };
        }

        public async Task<IEnumerable<VisitDayChangeRequestResponse>> GetVisitDayChangeRequestsAsync()
        {
            return await _context.VisitDayChangeRequests
                .Select(request => new VisitDayChangeRequestResponse
                {
                    ChangeRequestId = request.ChangeRequestId,
                    VisitID = request.VisitId,
                    PatientId = request.PatientId,
                    RequestedDate = request.RequestedDate,
                    Reason = request.Reason,
                    Status = request.Status,
                    StaffNotes = request.StaffNotes,
                    RequestedDateAt = request.RequestedDateAt
                })
                .ToListAsync();
        }

        public async Task<VisitDayChangeRequestResponse> CreateVisitDayChangeRequestAsync(CreateVisitDayChangeRequest request)
        {
            
            var visit = await _context.Visits
                 .Include(v => v.Appointment)
                     .ThenInclude(a => a.RegistrationDetail)
            .FirstOrDefaultAsync(r => r.VisitId == request.VisitID);
            if (visit == null)
            {
                throw new Exception("Không tìm thấy Visit");
            }

            var changeRequest = new VisitDayChangeRequest
            {
                VisitId = request.VisitID,
                PatientId = visit.Appointment.RegistrationDetail.PatientId.Value, 
                RequestedDate = request.RequestedDate,
                Reason = request.Reason,
                Status = "Pending",
                RequestedDateAt = DateTime.Now
            };

            _context.VisitDayChangeRequests.Add(changeRequest);
            await _context.SaveChangesAsync();

            return new VisitDayChangeRequestResponse
            {
                ChangeRequestId = changeRequest.ChangeRequestId,
                VisitID = changeRequest.VisitId,
                PatientId = changeRequest.PatientId,
                RequestedDate = changeRequest.RequestedDate,
                Reason = changeRequest.Reason,
                Status = changeRequest.Status,
                StaffNotes = changeRequest.StaffNotes,
                RequestedDateAt = changeRequest.RequestedDateAt
            };
        }
        public async Task<IActionResult> UpdateVisitDayChangeRequestAsync(int id, UpdateVisitDayChangeRequest request)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var changeRequest = await _context.VisitDayChangeRequests.FindAsync(id);

                if (changeRequest == null)
                {
                    return new NotFoundResult();
                }

                changeRequest.Status = request.Status;
                changeRequest.StaffNotes = request.StaffNotes;

                // Nếu được chấp nhận, cập nhật VisitDate
                if (request.Status == "Approved")
                {
                    var visit = await _context.Visits.FindAsync(changeRequest.VisitId);
                    if (visit == null)
                    {
                        throw new Exception("Không tìm thấy Visit");
                    }
                    var oldVisitDate = visit.VisitDate;
                    visit.VisitDate = changeRequest.RequestedDate;
                    changeRequest.StaffNotes = "Thay đổi "+ oldVisitDate + " thành " + changeRequest.RequestedDate + " thành công";
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

        public async Task DeleteVisitDayChangeRequestAsync(int id)
        {
            var changeRequest = await _context.VisitDayChangeRequests.FindAsync(id);

            if (changeRequest == null)
            {
                throw new Exception("Không tìm thấy VisitDayChangeRequest");
            }

            _context.VisitDayChangeRequests.Remove(changeRequest);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<VisitDayChangeRequestResponse>> GetVisitDayChangeRequestsByVisitIdAsync(int visitId)
        {
            return await _context.VisitDayChangeRequests
                .Where(request => request.VisitId == visitId)
                .Select(request => new VisitDayChangeRequestResponse
                {
                    ChangeRequestId = request.ChangeRequestId,
                    VisitID = request.VisitId,
                    PatientId = request.PatientId,
                    RequestedDate = request.RequestedDate,
                    Reason = request.Reason,
                    Status = request.Status,
                    StaffNotes = request.StaffNotes,
                    RequestedDateAt = request.RequestedDateAt
                })
                .ToListAsync();
        }
    }
    }