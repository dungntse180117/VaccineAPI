using Microsoft.Identity.Client;
using Newtonsoft.Json;
using VaccineAPI.Shared.Helpers;
using VaccineAPI.Shared.Response;

namespace VaccineAPI.Shared.Request
{
    public class PaymentRequest
    {
        public PaymentRequest(int appId, string appUser, long appTime,
            long amount, string appTransID, string bankCode, string description)
        {
            AppId = appId;
            AppUser = appUser;
            AppTime = appTime;
            Amount = amount;
            BankCode = bankCode;
            description = description;

        }

        public int AppId { get; set; }
        public string AppUser { get; set; }
        public long AppTime { get; set; }
        public long Amount { get; set; }
        public string AppTransID { get; set; }
        = string.Empty;

        public string returnURL { get; } = string.Empty;

        public string Mac { get; set; } = string.Empty;
        public string EmbedData { get; set; }
        public string BankCode { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;

        public void MakeSignature(string key)
        {
            var data = AppId + "|" + AppTransID + "|" + AppUser + "|" + Amount + "|" + AppTime + "|" + "|";
            this.Mac = HashHelper.HmacSHA256(data, key);
        }

        public Dictionary<string, string> GetContent()
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

            keyValuePairs.Add("appid", AppId.ToString());
            keyValuePairs.Add("appuser", AppUser);
            keyValuePairs.Add("apptime", AppTime.ToString());
            keyValuePairs.Add("amount", Amount.ToString());
            keyValuePairs.Add("apptransid", AppTransID);
            keyValuePairs.Add("description", description);
            keyValuePairs.Add("bankcode", "Zalopayapp");
            keyValuePairs.Add("mac", Mac);

            return keyValuePairs;

        }
        public (bool, string) GetLink(string paymentUrl)
        {
            using var client = new HttpClient();
            var content = new FormUrlEncodedContent(GetContent());
            var response = client.PostAsync(paymentUrl, content).Result;

            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content.ReadAsStringAsync().Result;
                var responseData = JsonConvert
                    .DeserializeObject<PaymentResponse>(responseContent);
                if (responseData.returnCode == 1)
                {
                    return (true, responseData.orderUrl);
                }
                else
                {
                    return (false, responseData.returnMessage);
                }

            }
            else
            {
                return (false, response.ReasonPhrase ?? string.Empty);
            }
        }

    }
}