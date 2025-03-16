using Microsoft.EntityFrameworkCore;
using VaccineAPI.DataAccess.Models;
using VaccineAPI.BusinessLogic.Services.Interface;
using VaccineAPI.Shared.Request;
using VaccineAPI.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VaccineAPI.DataAccess.Data;

namespace VaccineAPI.BusinessLogic.Services.Implement
{
    public class PatientService : IPatientService
    {
        private readonly VaccinationTrackingContext _context;
        private readonly ILogger<PatientService> _logger;

        public PatientService(VaccinationTrackingContext context, ILogger<PatientService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<PatientResponse> CreatePatientAsync(CreatePatientRequest request)
        {
            try
            {
                // Kiểm tra xem AccountId có tồn tại hay không
                var account = await _context.Accounts.FindAsync(request.AccountId);
                if (account == null)
                {
                    throw new ArgumentException($"Không tìm thấy Account với ID = {request.AccountId}");
                }

                // Tạo Patient từ request (ánh xạ thủ công)
                var patient = new Patient
                {
                    Dob = request.Dob,
                    PatientName = request.PatientName,
                    Gender = request.Gender,
                    GuardianPhone = request.GuardianPhone,
                    Address = request.Address,
                    RelationshipToAccount = request.RelationshipToAccount,
                    Phone = request.Phone
                };

                // Thêm Patient vào database
                _context.Patients.Add(patient);
                await _context.SaveChangesAsync();

                // Tạo Parent_Child
                var parentChild = new ParentChild
                {
                    AccountId = request.AccountId,
                    PatientId = patient.PatientId
                };
                _context.ParentChildren.Add(parentChild);
                await _context.SaveChangesAsync();

                // Tạo PatientResponse (ánh xạ thủ công)
                return new PatientResponse
                {
                    PatientId = patient.PatientId,
                    Dob = patient.Dob,
                    PatientName = patient.PatientName,
                    Gender = patient.Gender,
                    GuardianPhone = request.GuardianPhone,
                    Address = request.Address,
                    RelationshipToAccount = request.RelationshipToAccount,
                    Phone = patient.Phone
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo Patient."); // Ghi log
                throw; // Ném exception để controller xử lý
            }
        }

        public async Task<PatientResponse?> GetPatientByIdAsync(int id)
        {
            try
            {
                var patient = await _context.Patients.FindAsync(id);

                if (patient == null)
                {
                    return null;
                }

                // Tạo PatientResponse (ánh xạ thủ công)
                return new PatientResponse
                {
                    PatientId = patient.PatientId,
                    Dob = patient.Dob,
                    PatientName = patient.PatientName,
                    Gender = patient.Gender,
                    GuardianPhone = patient.GuardianPhone,
                    Address = patient.Address,
                    RelationshipToAccount = patient.RelationshipToAccount,
                    Phone = patient.Phone
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi GetPatientByIdAsync with ID: {id}.", id); // Ghi log
                throw;
            }
        }

        public async Task<List<PatientResponse>> GetAllPatientsAsync()
        {
            try
            {
                return await _context.Patients
                    .Select(patient => new PatientResponse
                    {
                        PatientId = patient.PatientId,
                        Dob = patient.Dob,
                        PatientName = patient.PatientName,
                        Gender = patient.Gender,
                        GuardianPhone = patient.GuardianPhone,
                        Address = patient.Address,
                        RelationshipToAccount = patient.RelationshipToAccount,
                        Phone = patient.Phone
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi GetAllPatientsAsync"); 
                throw;
            }
        }
        public async Task<List<PatientResponse>> GetAllPatientsByAccountIdAsync(int accountId)
        {
            try
            {
                var patients = await _context.ParentChildren
                       .Where(pc => pc.AccountId == accountId)
                       .Select(pc => pc.Patient)
                       .ToListAsync();

                return patients.Select(patient => new PatientResponse
                {
                    PatientId = patient.PatientId,
                    Dob = patient.Dob,
                    PatientName = patient.PatientName,
                    Gender = patient.Gender,
                    GuardianPhone = patient.GuardianPhone,
                    Address = patient.Address,
                    RelationshipToAccount = patient.RelationshipToAccount,
                    Phone = patient.Phone
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi GetAllPatientsByAccountIdAsync with AccountId: {accountId}.", accountId);
                throw;
            }
        }
        public async Task<PatientResponse?> UpdatePatientAsync(int id, UpdatePatientRequest request)
        {
            try
            {
                var patient = await _context.Patients.FindAsync(id);

                if (patient == null)
                {
                    return null;
                }

               
                if (request.Dob.HasValue)
                {
                    patient.Dob = request.Dob.Value;
                }
                if (!string.IsNullOrEmpty(request.PatientName))
                {
                    patient.PatientName = request.PatientName;
                }
                if (!string.IsNullOrEmpty(request.Gender))
                {
                    patient.Gender = request.Gender;
                }
                if (!string.IsNullOrEmpty(request.GuardianPhone))
                {
                    patient.GuardianPhone = request.GuardianPhone;
                }
                if (!string.IsNullOrEmpty(request.Address))
                {
                    patient.Address = request.Address;
                }
                if (!string.IsNullOrEmpty(request.RelationshipToAccount))
                {
                    patient.RelationshipToAccount = request.RelationshipToAccount;
                }
                if (!string.IsNullOrEmpty(request.Phone))
                {
                    patient.Phone = request.Phone;
                }

                await _context.SaveChangesAsync();

               
                return new PatientResponse
                {
                    PatientId = patient.PatientId,
                    Dob = patient.Dob,
                    PatientName = patient.PatientName,
                    Gender = patient.Gender,
                    GuardianPhone = request.GuardianPhone,
                    Address = patient.Address,
                    RelationshipToAccount = request.RelationshipToAccount,
                    Phone = patient.Phone
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi UpdatePatientAsync with ID: {id}.", id); 
                throw;
            }
        }

        public async Task<bool> DeletePatientAsync(int id)
        {
            try
            {
                var patient = await _context.Patients.FindAsync(id);

                if (patient == null)
                {
                    return false;
                }

               
                var parentChildren = await _context.ParentChildren
                    .Where(pc => pc.PatientId == id)
                    .ToListAsync();

                _context.ParentChildren.RemoveRange(parentChildren);
                await _context.SaveChangesAsync();

               
                _context.Patients.Remove(patient);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi DeletePatientAsync with ID: {id}.", id); 
                throw;
            }
        }
        public async Task<List<PatientResponse>> GetPatientsByPhoneAsync(string phone)
        {
            try
            {
                var patients = await _context.Patients
                .Where(p => p.Phone == phone)
                .Select(patient => new PatientResponse
                {
                    PatientId = patient.PatientId,
                    Dob = patient.Dob,
                    PatientName = patient.PatientName,
                    Gender = patient.Gender,
                    GuardianPhone = patient.GuardianPhone,
                    Address = patient.Address,
                    RelationshipToAccount = patient.RelationshipToAccount,
                    Phone = patient.Phone

                }).ToListAsync();

                return patients;
            }
            catch (Exception e)
            {

                _logger.LogError(e, "GetPatientsByPhoneAsync error with phone: {phone}", phone);
                throw;
            }
        }
    }
}
