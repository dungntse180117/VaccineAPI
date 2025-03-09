using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using VaccineAPI.Shared.Helpers;
using VaccineAPI.Shared.Request;
using VaccineAPI.Shared.Response;

namespace VaccineAPI.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly HttpClient _httpClient;
        private readonly ZalopayConfig _zalopayConfig;

        public PaymentService(HttpClient httpClient, IOptions<ZalopayConfig> zalopayConfig)
        {
            _httpClient = httpClient;
            _zalopayConfig = zalopayConfig.Value;
        }

        public async Task<(bool IsSuccess, string Message)> CreatePaymentAsync(PaymentRequest request)
        {
            try
            {
                // Generate the signature (MAC)
                request.MakeSignature(_zalopayConfig.Key1);

                var content = new FormUrlEncodedContent(request.GetContent());
                var response = await _httpClient.PostAsync(_zalopayConfig.PaymentURL, content);

                if (!response.IsSuccessStatusCode)
                {
                    return (false, $"Failed: {response.ReasonPhrase}");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var responseData = JsonSerializer.Deserialize<PaymentResponse>(responseContent);

                if (responseData != null && responseData.returnCode == 1)
                {
                    return (true, responseData.orderUrl);
                }

                return (false, responseData?.returnMessage ?? "Unknown error");
            }
            catch (Exception ex)
            {
                return (false, $"Exception: {ex.Message}");
            }
        }
    }
}
