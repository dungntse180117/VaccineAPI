using VaccineAPI.Shared.Request;
using VaccineAPI.Shared.Response;

namespace VaccineAPI.Services
{
    public interface IPaymentService
    {
        Task<(bool IsSuccess, string Message)> CreatePaymentAsync(PaymentRequest request);
    }
}
