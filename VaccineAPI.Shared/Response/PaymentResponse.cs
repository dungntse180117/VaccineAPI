namespace VaccineAPI.Shared.Response
{
    public class PaymentResponse
    {
        public int returnCode { get; set; }
        public string returnMessage { get; set; } = string.Empty;
        public string orderUrl { get; set; } = string.Empty;
    }
}