using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using VaccineAPI.BusinessLogic.Services.Interface;
using VaccineAPI.DataAccess.Data;
using VaccineAPI.DataAccess.Models;
using VaccineAPI.Shared.Request;
using VaccineAPI.Shared.Response;

namespace VaccineAPI.BusinessLogic.Services.Implement
{
    public class RegistrationDetailService : IRegistrationDetailService
    {
        private readonly VaccinationTrackingContext _context;
        private readonly ILogger<RegistrationDetailService> _logger;

        public RegistrationDetailService(VaccinationTrackingContext context, ILogger<RegistrationDetailService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<RegistrationDetailResponse> CreateRegistrationDetailAsync(CreateRegistrationDetailRequest request)
        {
            try
            {
                var registration = await _context.Registrations.FindAsync(request.RegistrationID);
                if (registration == null)
                {
                    throw new ArgumentException($"Không tìm thấy Registration với ID = {request.RegistrationID}");
                }
                var patient = await _context.Patients.FindAsync(request.PatientId);
                if (registration == null)
                {
                    throw new ArgumentException($"Không tìm thấy Registration với ID = {request.PatientId}");
                }

                var registrationDetail = new RegistrationDetail
                {
                    RegistrationId = request.RegistrationID,
                    Quantity = request.Quantity,
                    Price = request.Price,
                    DesiredDate = request.DesiredDate,
                    PatientId = request.PatientId,
                    Status = request.Status
                };

                _context.RegistrationDetails.Add(registrationDetail);
                await _context.SaveChangesAsync();

                return new RegistrationDetailResponse
                {
                    RegistrationDetailID = registrationDetail.RegistrationDetailId,
                    RegistrationID = registrationDetail.RegistrationId,
                    Quantity = registrationDetail.Quantity,
                    Price = registrationDetail.Price,
                    DesiredDate = registrationDetail.DesiredDate,
                    PatientId = (int)registrationDetail.PatientId,
                    Status = registrationDetail.Status
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo RegistrationDetail.");
                throw;
            }
        }
        public async Task<List<RegistrationDetailResponse>> GetAllRegistrationDetailsAsync()
        {
            try
            {
                return await _context.RegistrationDetails
                    .Include(rd => rd.Registration)
                    .ThenInclude(r => r.RegistrationVaccinations)
                    .Include(rd => rd.Registration)
                    .ThenInclude(r => r.Account)
                    .Select( rd => new RegistrationDetailResponse
                    {
                        RegistrationDetailID = rd.RegistrationDetailId,
                        RegistrationID = rd.RegistrationId,
                        Quantity = rd.Quantity,
                        Price = rd.Price,
                        DesiredDate = rd.DesiredDate,
                        PatientId = (int)rd.PatientId,
                        Status = rd.Status,
                        AccountId = rd.Registration.AccountId, // Lấy accountId từ Registration
                        VaccinationNames = rd.Registration.RegistrationVaccinations
                            .Select(rv => rv.Vaccination.VaccinationName)
                            .ToList(), // Lấy danh sách tên vắc xin
                        ServiceName = rd.Registration.Service.ServiceName
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi GetAllRegistrationDetailsAsync");
                throw;
            }
        }
        public async Task<RegistrationDetailResponse?> GetRegistrationDetailAsync(int id)
        {
            try
            {
                var registrationDetail = await _context.RegistrationDetails.FindAsync(id);

                if (registrationDetail == null)
                {
                    return null;
                }

                var registration = await _context.Registrations
                    .Include(r => r.RegistrationVaccinations)
                    .FirstOrDefaultAsync(r => r.RegistrationId == registrationDetail.RegistrationId);

                if (registration == null)
                {
                    throw new ArgumentException($"Không tìm thấy Registration với ID = {registrationDetail.RegistrationId}");
                }
                var patient = await _context.Patients.FindAsync(registrationDetail.PatientId);

                List<string> vaccinationNames = new List<string>();
                string? serviceName = null;

                if (registration.ServiceId != null)
                {
                    var service = await _context.VaccinationServices.FindAsync(registration.ServiceId);
                    if (service != null)
                    {
                        serviceName = service.ServiceName;
                    }
                }
                else
                {
                    foreach (var item in registration.RegistrationVaccinations)
                    {
                        if (item.VaccinationId != null)
                        {
                            var vaccine = await _context.Vaccinations.FindAsync(item.VaccinationId);
                            if (vaccine != null)
                            {
                                vaccinationNames.Add(vaccine.VaccinationName);
                            }
                        }
                    }
                }

                var response = new RegistrationDetailResponse
                {
                    RegistrationDetailID = registrationDetail.RegistrationDetailId,
                    RegistrationID = registrationDetail.RegistrationId,
                    Quantity = registrationDetail.Quantity,
                    Price = registrationDetail.Price,
                    DesiredDate = registrationDetail.DesiredDate,
                    PatientId = (int)registrationDetail.PatientId,
                    VaccinationNames = vaccinationNames,
                    ServiceName = serviceName,
                    Status = registrationDetail.Status,
                    AccountId = registration.AccountId
                };

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi GetRegistrationDetail with ID: {id}.", id);
                throw;
            }
        }

        public async Task<IActionResult> UpdateRegistrationDetailAsync(int id, UpdateRegistrationDetailRequest request)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var registrationDetail = await _context.RegistrationDetails.FindAsync(id);

                if (registrationDetail == null)
                {
                    return new NotFoundResult();
                }

                if (request.Quantity.HasValue)
                {
                    registrationDetail.Quantity = request.Quantity.Value;
                }
                if (request.Price.HasValue)
                {
                    registrationDetail.Price = request.Price.Value;
                }
                if (request.DesiredDate.HasValue)
                {
                    registrationDetail.DesiredDate = request.DesiredDate.Value;
                }

                await _context.SaveChangesAsync();

                var response = new RegistrationDetailResponse
                {
                    RegistrationDetailID = registrationDetail.RegistrationDetailId,
                    RegistrationID = registrationDetail.RegistrationId,
                    Quantity = registrationDetail.Quantity,
                    Price = registrationDetail.Price,
                    DesiredDate = registrationDetail.DesiredDate
                };

                transaction.Commit();
                return new OkObjectResult(response);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Lỗi khi UpdateRegistrationDetail with ID: {id}.", id);
                return new StatusCodeResult(500);
            }
        }

        public async Task<IActionResult> DeleteRegistrationDetailAsync(int id)
        {
            try
            {
                var registrationDetail = await _context.RegistrationDetails.FindAsync(id);

                if (registrationDetail == null)
                {
                    return new NotFoundResult();
                }

                _context.RegistrationDetails.Remove(registrationDetail);
                await _context.SaveChangesAsync();

                return new NoContentResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi DeleteRegistrationDetail with ID: {id}.", id);
                return new StatusCodeResult(500);
            }
        }
    }
}