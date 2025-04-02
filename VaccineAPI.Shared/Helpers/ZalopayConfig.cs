using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaccineAPI.Shared.Helpers
{
    public class ZalopayConfig
    {
        public static string ConfigName = "ZaloPay";

        public string AppUser { get; set; } = string.Empty;

        public string PaymentURL { get; set; } = string.Empty;

        public string edirectURL {  get; set; } = string.Empty;

        public string IpnURL {  get; set; } = string.Empty;

        public string AppId {  get; set; } = string.Empty;

        public string Key1 {  get; set; } = string.Empty;
    }
}
