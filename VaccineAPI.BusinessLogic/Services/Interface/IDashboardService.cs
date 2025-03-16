using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaccineAPI.BusinessLogic.Services.Interface
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    namespace VaccineAPI.BusinessLogic.Services.Interface
    {
        public interface IDashboardService
        {
            Task<Dictionary<int, decimal>> GetRevenuePerMonthAsync(int year);
            Task<Dictionary<int, int>> GetVisitsPerMonthAsync(int year);
            Task<string> GetMostPurchasedVaccineAsync();
            Task<string> GetMostPurchasedPackageAsync();
        }
    }

}
