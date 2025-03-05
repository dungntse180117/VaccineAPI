using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineAPI.Shared.Request;
using VaccineAPI.Shared.Respones;

namespace VaccineAPI.BusinessLogic.Interface
{
    public interface IVaccineOrderService
    {
        Task<OrderVaccineResponse> PlaceOrderAsync(OrderVaccineRequest request);
    }
}
