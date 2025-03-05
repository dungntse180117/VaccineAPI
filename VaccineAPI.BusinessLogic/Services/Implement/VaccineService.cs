//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using VaccineAPI.BusinessLogic.Interface;
//using VaccineAPI.DataAccess.Data;
//using VaccineAPI.DataAccess.Models;
//using VaccineAPI.Shared.Request;
//using VaccineAPI.Shared.Response;

//namespace VaccineAPI.BusinessLogic.Implement
//{
//    public class VaccineService : IVaccineService
//    {
//        private readonly DbContext _context;
//        private readonly ILogger<VaccineService> _logger;

//        public VaccineService(VaccinationTrackingContext context, ILogger<VaccineService> logger)
//        {
//            _context = context;  // Use VaccinationTrackingContext directly
//            _logger = logger;
//        }

//        public async Task<VaccineResponse> CreateVaccineAsync(VaccineRequest request)
//        {
//            VaccineResponse response = new VaccineResponse { IsSuccess = false };

//            try
//            {
//                var vaccine = new Vaccination
//                {
//                    VaccinationName = request.VaccinationName,
//                    Price = request.Price,
//                    Description = request.Description,
//                    Quantity = request.Quantity,
//                    Img = request.Img
//                };

//                _context.Set<Vaccination>().Add(vaccine);
//                await _context.SaveChangesAsync();

//                response.VaccinationId = vaccine.VaccinationId;
//                response.VaccinationName = vaccine.VaccinationName;
//                response.Price = vaccine.Price;
//                response.Description = vaccine.Description;
//                response.Quantity = vaccine.Quantity;
//                response.Img = vaccine.Img;

//                response.IsSuccess = true;
//                response.Message = "Vaccine created successfully.";
//                _logger.LogInformation($"Vaccine created successfully. VaccinationId: {response.VaccinationId}, VaccinationName: {response.VaccinationName}");
//            }
//            catch (Exception ex)
//            {
//                response.Message = $"An error occurred: {ex.Message}";
//                _logger.LogError(ex, "Error creating vaccine.");
//            }

//            return response;
//        }

//        public async Task<VaccineResponse> ManageVaccineAsync(int vaccinationId, string action, VaccineRequest? updateRequest = null)
//        {
//            VaccineResponse response = new VaccineResponse { VaccinationId = vaccinationId, IsSuccess = false };

//            try
//            {
//                var vaccine = await _context.Set<Vaccination>().FindAsync(vaccinationId);

//                if (vaccine == null)
//                {
//                    response.Message = "Vaccine not found.";
//                    _logger.LogError($"Vaccine not found. VaccinationId: {vaccinationId}");
//                    return response;
//                }

//                switch (action.ToLower())
//                {
//                    case "update":
//                        if (updateRequest == null)
//                        {
//                            response.Message = "Update request required for update action.";
//                            _logger.LogError($"Update request required for update action. VaccinationId: {vaccinationId}");
//                            return response;
//                        }

//                        vaccine.VaccinationName = updateRequest.VaccinationName;
//                        vaccine.Price = updateRequest.Price;
//                        vaccine.Description = updateRequest.Description;
//                        vaccine.Quantity = updateRequest.Quantity;
//                        vaccine.Img = updateRequest.Img;

//                        await _context.SaveChangesAsync();

//                        response.VaccinationId = vaccine.VaccinationId;
//                        response.VaccinationName = vaccine.VaccinationName;
//                        response.Price = vaccine.Price;
//                        response.Description = vaccine.Description;
//                        response.Quantity = vaccine.Quantity;
//                        response.Img = vaccine.Img;

//                        response.IsSuccess = true;
//                        response.Message = "Vaccine updated successfully.";
//                        _logger.LogInformation($"Vaccine updated successfully. VaccinationId: {vaccine.VaccinationId}");
//                        break;

//                    case "get":
//                        response.VaccinationId = vaccine.VaccinationId;
//                        response.VaccinationName = vaccine.VaccinationName;
//                        response.Price = vaccine.Price;
//                        response.Description = vaccine.Description;
//                        response.Quantity = vaccine.Quantity;
//                        response.Img = vaccine.Img;

//                        response.IsSuccess = true;
//                        response.Message = "Vaccine retrieved successfully.";
//                        _logger.LogInformation($"Vaccine retrieved successfully. VaccinationId: {vaccinationId}");
//                        break;

//                    case "delete":
//                        _context.Set<Vaccination>().Remove(vaccine);
//                        await _context.SaveChangesAsync();

//                        response.IsSuccess = true;
//                        response.Message = "Vaccine deleted successfully.";
//                        _logger.LogInformation($"Vaccine deleted successfully. VaccinationId: {vaccinationId}");
//                        break;

//                    default:
//                        response.Message = "Invalid action specified.";
//                        _logger.LogError($"Invalid action specified. Action: {action}");
//                        break;
//                }
//            }
//            catch (Exception ex)
//            {
//                response.Message = $"An error occurred: {ex.Message}";
//                _logger.LogError(ex, "Error managing vaccine.");
//            }

//            return response;
//        }

//        public async Task<IEnumerable<VaccineResponse>> GetAllVaccinesAsync()
//        {
//            try
//            {
//                var vaccines = await _context.Set<Vaccination>().ToListAsync();

//                return vaccines.Select(vaccine => new VaccineResponse
//                {
//                    VaccinationId = vaccine.VaccinationId,
//                    VaccinationName = vaccine.VaccinationName,
//                    Price = vaccine.Price,
//                    Description = vaccine.Description,
//                    Quantity = vaccine.Quantity,
//                    Img = vaccine.Img,
//                    IsSuccess = true,    // Assuming these are always successful for GetAll
//                    Message = "Vaccine retrieved."
//                }).ToList();
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error getting all vaccines.");
//                return new List<VaccineResponse>();
//            }
//        }

//        public async Task<VaccineResponse> GetVaccineAsync(int vaccinationId)
//        {
//            VaccineResponse response = new VaccineResponse() { VaccinationId = vaccinationId, IsSuccess = false };
//            try
//            {
//                var vaccine = await _context.Set<Vaccination>().FindAsync(vaccinationId);

//                if (vaccine == null)
//                {
//                    response.Message = "Vaccine not found.";
//                    _logger.LogError($"Vaccine not found. VaccinationId: {vaccinationId}");
//                    return response;
//                }

//                response.VaccinationId = vaccine.VaccinationId;
//                response.VaccinationName = vaccine.VaccinationName;
//                response.Price = vaccine.Price;
//                response.Description = vaccine.Description;
//                response.Quantity = vaccine.Quantity;
//                response.Img = vaccine.Img;

//                response.IsSuccess = true;
//                response.Message = "Vaccine retrieved successfully.";
//                _logger.LogInformation($"Vaccine retrieved successfully. VaccinationId: {vaccinationId}");
//            }
//            catch (Exception ex)
//            {
//                response.Message = $"An error occurred: {ex.Message}";
//                _logger.LogError(ex, "Error getting vaccine.");

//            }
//            return response;
//        }
//        public async Task<VaccineResponse> UpdateVaccineAsync(int vaccinationId, VaccineRequest request)
//        {
//            VaccineResponse response = new VaccineResponse { IsSuccess = false };

//            try
//            {
//                var vaccine = await _context.Set<Vaccination>().FindAsync(vaccinationId);

//                if (vaccine == null)
//                {
//                    response.Message = "Vaccine not found.";
//                    _logger.LogError($"Vaccine not found. VaccinationId: {vaccinationId}");
//                    return response;
//                }

//                vaccine.VaccinationName = request.VaccinationName;
//                vaccine.Price = request.Price;
//                vaccine.Description = request.Description;
//                vaccine.Quantity = request.Quantity;
//                vaccine.Img = request.Img;

//                await _context.SaveChangesAsync();

//                response = new VaccineResponse
//                {
//                    VaccinationId = vaccinationId,
//                    VaccinationName = vaccine.VaccinationName,
//                    Price = vaccine.Price,
//                    Description = vaccine.Description,
//                    Quantity = vaccine.Quantity,
//                    Img = vaccine.Img,
//                    IsSuccess = true,
//                    Message = "Vaccine updated successfully."
//                };

//                _logger.LogInformation($"Vaccine updated successfully. VaccinationId: {vaccinationId}");
//            }
//            catch (Exception ex)
//            {
//                response.Message = $"An error occurred: {ex.Message}";
//                _logger.LogError(ex, "Error updating vaccine.");
//            }

//            return response;
//        }

//        public async Task<VaccineResponse> DeleteVaccineAsync(int vaccinationId)
//        {
//            VaccineResponse response = new VaccineResponse { VaccinationId = vaccinationId, IsSuccess = false };

//            try
//            {
//                var vaccine = await _context.Set<Vaccination>().FindAsync(vaccinationId);

//                if (vaccine == null)
//                {
//                    response.Message = "Vaccine not found.";
//                    _logger.LogError($"Vaccine not found. VaccinationId: {vaccinationId}");
//                    return response;
//                }

//                _context.Set<Vaccination>().Remove(vaccine);
//                await _context.SaveChangesAsync();

//                response.IsSuccess = true;
//                response.Message = "Vaccine deleted successfully.";
//                _logger.LogInformation($"Vaccine deleted successfully. VaccinationId: {vaccinationId}");
//            }
//            catch (Exception ex)
//            {
//                response.Message = $"An error occurred: {ex.Message}";
//                _logger.LogError(ex, "Error deleting vaccine.");
//            }

//            return response;
//        }
//    }
//}
