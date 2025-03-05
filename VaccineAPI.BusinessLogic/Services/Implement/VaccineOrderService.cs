//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.Data.SqlClient;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using VaccineAPI.Shared.Request;
//using VaccineAPI.Shared.Respones;


//namespace VaccineAPI.BusinessLogic.Implement
//{
//    public interface IVaccineOrderService
//    {
//        Task<OrderVaccineResponse> PlaceOrderAsync(OrderVaccineRequest request);
//    }


//    // --- Service Implementation ---

//    public class VaccineOrderService : IVaccineOrderService
//    {
//        private readonly string _connectionString; // Inject connection string
//        private readonly ILogger<VaccineOrderService> _logger;

//        public VaccineOrderService(IConfiguration configuration, ILogger<VaccineOrderService> logger)
//        {
//            _connectionString = configuration.GetConnectionString("DefaultConnection");
//            _logger = logger;
//        }

//        public async Task<OrderVaccineResponse> PlaceOrderAsync(OrderVaccineRequest request)
//        {
//            OrderVaccineResponse response = new();

//            try
//            {
//                using (SqlConnection connection = new SqlConnection(_connectionString))
//                {
//                    await connection.OpenAsync();

//                    // 1. Retrieve Vaccination Price
//                    string getVaccinationPriceQuery = "SELECT price FROM Vaccination WHERE vaccinationId = @VaccinationId";
//                    decimal vaccinePrice;
//                    using (SqlCommand priceCommand = new SqlCommand(getVaccinationPriceQuery, connection))
//                    {
//                        priceCommand.Parameters.AddWithValue("@VaccinationId", request.VaccinationId);
//                        object result = await priceCommand.ExecuteScalarAsync();
//                        if (result == null || result == DBNull.Value)
//                        {
//                            response.IsSuccess = false;
//                            response.Message = "Vaccination not found.";
//                            return response;
//                        }
//                        vaccinePrice = Convert.ToDecimal(result);
//                    }



//                    // 2. Insert into Order_Vaccines table
//                    string insertOrderQuery = @"
//                    INSERT INTO Order_Vaccines (vaccinationId, price, quantity)
//                    VALUES (@VaccinationId, @Price, @Quantity);
//                    SELECT SCOPE_IDENTITY();"; // Get the newly inserted OrderId

//                    using (SqlCommand insertCommand = new SqlCommand(insertOrderQuery, connection))
//                    {
//                        insertCommand.Parameters.AddWithValue("@VaccinationId", request.VaccinationId);
//                        insertCommand.Parameters.AddWithValue("@Price", vaccinePrice);  // Use retrieved price
//                        insertCommand.Parameters.AddWithValue("@Quantity", request.Quantity);

//                        object orderIdResult = await insertCommand.ExecuteScalarAsync(); // ExecuteScalar returns the first column of the first row.  SCOPE_IDENTITY() returns the last identity value inserted into a table.
//                        if (orderIdResult != null && orderIdResult != DBNull.Value)
//                        {
//                            response.OrderId = Convert.ToInt32(orderIdResult);
//                        }
//                        else
//                        {
//                            response.IsSuccess = false;
//                            response.Message = "Failed to create order.";
//                            return response;
//                        }
//                    }

//                    response.Price = vaccinePrice;
//                    response.Quantity = request.Quantity;
//                    response.TotalPrice = vaccinePrice * request.Quantity; // Calculate total price
//                    response.IsSuccess = true;
//                    response.Message = "Order placed successfully.";
//                    _logger.LogInformation($"Order placed successfully. OrderId: {response.OrderId}, VaccinationId: {request.VaccinationId}, Quantity: {request.Quantity}");


//                }
//            }
//            catch (Exception ex)
//            {
//                response.IsSuccess = false;
//                response.Message = $"An error occurred: {ex.Message}";
//                _logger.LogError(ex, "Error placing order.");
//            }

//            return response;
//        }
//    }

//}
