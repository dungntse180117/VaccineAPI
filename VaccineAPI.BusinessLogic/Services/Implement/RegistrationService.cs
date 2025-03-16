using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VaccineAPI.BusinessLogic.Services.Interface;
using VaccineAPI.DataAccess.Data;
using VaccineAPI.DataAccess.Models;
using VaccineAPI.Shared.Request;
using VaccineAPI.Shared.Response;

namespace VaccineAPI.BusinessLogic.Services.Implement
{
    public class RegistrationService : IRegistrationService
    {
        private readonly VaccinationTrackingContext _context;
        private readonly ILogger<RegistrationService> _logger;

        public RegistrationService(VaccinationTrackingContext context, ILogger<RegistrationService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<RegistrationResponse> CreateRegistrationAsync(CreateRegistrationRequest request)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                // 1. Validation: Chỉ cho phép 1 service hoặc nhiều vaccinationIds (không cả hai)
                if (request.ServiceId == 0)
                {
                    request.ServiceId = null;
                }
                if (request.ServiceId.HasValue && request.VaccinationIds.Any())
                {
                    throw new ArgumentException("Chỉ được chọn 1 gói tiêm chủng hoặc nhiều vắc xin lẻ, không được chọn cả hai.");
                }


                // 2. Tính totalAmount
                decimal totalAmount = 0;
                List<int> vaccinationIdsToCheck = new List<int>();
                if (request.ServiceId.HasValue && request.ServiceId > 0)
                {
                    var service = await _context.VaccinationServices.FindAsync(request.ServiceId);
                    if (service == null)
                    {
                        throw new ArgumentException($"Không tìm thấy service với ID = {request.ServiceId}");
                    }
                    totalAmount = service.Price * request.PatientIds.Count; // Giá gói * số lượng bệnh nhân
                }
                else if (request.VaccinationIds.Any())
                {
                    foreach (var vaccinationId in request.VaccinationIds)
                    {
                        var vaccination = await _context.Vaccinations.FindAsync(vaccinationId);
                        if (vaccination == null)
                        {
                            throw new ArgumentException($"Không tìm thấy vắc xin với ID = {vaccinationId}");
                        }
                        totalAmount += (decimal)vaccination.Price * request.PatientIds.Count; //Giá vaccine * số lượng bệnh nhân
                        vaccinationIdsToCheck.AddRange(request.VaccinationIds);
                    }
                }
                foreach (var patientId in request.PatientIds)
                {
                    var patient = await _context.Patients.FindAsync(patientId);
                    if (patient == null)
                    {
                        throw new ArgumentException($"Không tìm thấy bệnh nhân với ID = {patientId}");
                    }

                    foreach (var vaccinationId in vaccinationIdsToCheck)
                    {
                        var vaccination = await _context.Vaccinations.FindAsync(vaccinationId);
                        if (vaccination == null)
                        {
                            throw new ArgumentException($"Không tìm thấy vắc xin với ID = {vaccinationId}");
                        }

                        // Tính tuổi của bệnh nhân bằng NGÀY
                        int patientAgeInDays = CalculateAgeInDays(patient.Dob);

                        // Tính tuổi tối thiểu của vắc xin bằng NGÀY
                        int minAgeInDays = CalculateVaccinationAgeInDays((int)vaccination.MinAge, (int)vaccination.AgeUnitId);

                        // Kiểm tra tuổi tối thiểu
                        if (patientAgeInDays < minAgeInDays)
                        {
                            throw new ArgumentException($"Tuổi của bệnh nhân {patient.PatientName} không đủ tuổi để tiêm vắc xin {vaccination.VaccinationName}.");
                        }       
                    }
                }
                // 3. Tạo Registration
                var registration = new Registration
                {
                    AccountId = request.AccountId,
                    RegistrationDate = request.RegistrationDate,
                    TotalAmount = totalAmount,
                    Status = request.Status,
                    ServiceId = request.ServiceId,
                    DesiredDate = request.DesiredDate
                };

                _context.Registrations.Add(registration);
                await _context.SaveChangesAsync();

                // 4. Tạo RegistrationVaccination 
                if (request.VaccinationIds != null && request.VaccinationIds.Any())
                {
                    foreach (var vaccinationId in request.VaccinationIds)
                    {
                        var registrationVaccination = new RegistrationVaccination
                        {
                            RegistrationId = registration.RegistrationId,
                            VaccinationId = vaccinationId
                        };
                        _context.RegistrationVaccinations.Add(registrationVaccination);
                    }
                    await _context.SaveChangesAsync();
                }
                // 5. Tạo Registration_Patient
                foreach (var patientId in request.PatientIds)
                {
                    var registrationPatient = new RegistrationPatient
                    {
                        RegistrationId = registration.RegistrationId,
                        PatientId = patientId,
                    };
                    _context.RegistrationPatients.Add(registrationPatient);
                }
                await _context.SaveChangesAsync();

                transaction.Commit();

                var response = new RegistrationResponse
                {
                    registrationID = registration.RegistrationId,
                    AccountId = registration.AccountId,
                    RegistrationDate = registration.RegistrationDate,
                    TotalAmount = registration.TotalAmount,
                    Status = registration.Status,
                    DesiredDate = registration.DesiredDate
                };

                return response;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Lỗi khi tạo Registration.");
                throw;
            }
        }
        private int CalculateAgeInDays(DateOnly dob)
        {
            DateOnly now = DateOnly.FromDateTime(DateTime.Today);
            return (int)(now.DayNumber - dob.DayNumber);
        }

        private int CalculateVaccinationAgeInDays(int age, int ageUnitId)
        {
            if (ageUnitId == 1) // Ngày
            {
                return age;
            }
            else if (ageUnitId == 2) // Tháng
            {
                return (int)(age * 30.44);
            }
            else if (ageUnitId == 3) // Năm
            {
                return (int)(age * 365.25);
            }
            else
            {
                throw new ArgumentException("Invalid AgeUnitId: " + ageUnitId);
            }
        }
    
        public async Task<RegistrationResponse?> GetRegistrationAsync(int id)
        {
            try
            {
                var registration = await _context.Registrations.FindAsync(id);

                if (registration == null)
                {
                    return null;
                }

                var response = new RegistrationResponse
                {
                    registrationID = registration.RegistrationId,
                    AccountId = registration.AccountId,
                    RegistrationDate = registration.RegistrationDate,
                    TotalAmount = registration.TotalAmount,
                    Status = registration.Status,
                    DesiredDate = registration.DesiredDate
                };

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi GetRegistration with ID: {id}.", id);
                throw;
            }
        }

        public async Task<IActionResult> UpdateRegistrationInfoAsync(int id, UpdateRegistrationRequest request)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var registration = await _context.Registrations.FindAsync(id);

                if (registration == null)
                {
                    return new NotFoundResult();
                }

                // Cập nhật các trường nếu có giá trị mới
                if (request.AccountId.HasValue)
                {
                    registration.AccountId = request.AccountId.Value;
                }
                if (request.RegistrationDate.HasValue)
                {
                    registration.RegistrationDate = request.RegistrationDate.Value;
                }
                if (request.TotalAmount.HasValue)
                {
                    registration.TotalAmount = request.TotalAmount.Value;
                }
                if (request.DesiredDate.HasValue)
                {
                    registration.DesiredDate = request.DesiredDate.Value;
                }

                await _context.SaveChangesAsync();

                var response = new RegistrationResponse
                {
                    registrationID = registration.RegistrationId,
                    AccountId = registration.AccountId,
                    RegistrationDate = registration.RegistrationDate,
                    TotalAmount = registration.TotalAmount,
                    Status = registration.Status,
                    DesiredDate = registration.DesiredDate
                };

                transaction.Commit();
                return new OkObjectResult(response);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Lỗi khi UpdateRegistrationInfo with ID: {id}.", id);
                return new StatusCodeResult(500);
            }
        }
        public async Task CreateRegistrationDetailsAsync(int registrationId)
        {
            //Loại bỏ using var transaction = _context.Database.BeginTransaction();
            try
            {
                var registration = await _context.Registrations
                    .Include(r => r.RegistrationPatients)
                    .Include(r => r.RegistrationVaccinations)
                    .FirstOrDefaultAsync(r => r.RegistrationId == registrationId);

                if (registration == null)
                {
                    throw new ArgumentException($"Không tìm thấy Registration với ID = {registrationId}");
                }

                // 3. Tạo RegistrationDetail cho từng bệnh nhân
                foreach (var registrationPatient in registration.RegistrationPatients)
                {
                    //Lấy giá
                    decimal priceForPatient = 0;
                    int quantity = 0;
                    if (registration.ServiceId != null)
                    {
                        var service = await _context.VaccinationServices.FindAsync(registration.ServiceId);
                        if (service == null)
                        {
                            throw new ArgumentException($"Không tìm thấy service với ID = {registration.ServiceId}");
                        }
                        priceForPatient = service.Price;
                        quantity = (int)service.TotalDoses;
                    }
                    else
                    {
                        foreach (var item in registration.RegistrationVaccinations)
                        {
                            if (item.VaccinationId != null)
                            {
                                var vaccine = await _context.Vaccinations.FindAsync(item.VaccinationId);
                                if (vaccine == null)
                                {
                                    throw new ArgumentException($"Không tìm thấy vắc xin với ID = {item.VaccinationId}");
                                }
                                priceForPatient += (decimal)vaccine.Price;
                                quantity += (int)vaccine.TotalDoses;
                            }
                        }
                    }

                    // Tạo RegistrationDetail
                    var registrationDetail = new RegistrationDetail
                    {
                        RegistrationId = registration.RegistrationId,
                        PatientId = registrationPatient.PatientId,
                        Quantity = quantity,
                        Price = priceForPatient,
                        DesiredDate = registration.DesiredDate,
                        Status = "Pending"
                    };

                    _context.RegistrationDetails.Add(registrationDetail);
                }

                await _context.SaveChangesAsync();

                //transaction.Commit(); //Di chuyển transaction ra bên ngoài
            }
            catch (Exception ex)
            {
                //transaction.Rollback();
                _logger.LogError(ex, "Lỗi khi tạo RegistrationDetails cho RegistrationId: {registrationId}.", registrationId);
                throw;
            }
        }
        public async Task<IActionResult> UpdateRegistrationStatusAsync(int id, UpdateRegistrationStatusRequest request)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var registration = await _context.Registrations.FindAsync(id);

                if (registration == null)
                {
                    return new NotFoundResult();
                }

                registration.Status = request.Status;

                await _context.SaveChangesAsync();

                var response = new RegistrationResponse
                {
                    registrationID = registration.RegistrationId,
                    AccountId = registration.AccountId,
                    RegistrationDate = registration.RegistrationDate,
                    TotalAmount = registration.TotalAmount,
                    Status = registration.Status,
                    DesiredDate = registration.DesiredDate
                };
                if (request.Status == "Confirmed")
                {
                    await CreateRegistrationDetailsAsync(id);
                }

                transaction.Commit();
                return new OkObjectResult(response);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Lỗi khi UpdateRegistrationStatus with ID: {id}.", id);
                return new StatusCodeResult(500);
            }
        }
        public async Task<IActionResult> DeleteRegistrationAsync(int id)
        {
            try
            {
                var registration = await _context.Registrations.FindAsync(id);

                if (registration == null)
                {
                    return new NotFoundResult();
                }

                _context.Registrations.Remove(registration);
                await _context.SaveChangesAsync();

                return new NoContentResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi DeleteRegistration with ID: {id}.", id);
                return new StatusCodeResult(500);
            }
        }
    }
}