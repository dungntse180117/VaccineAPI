using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineAPI.Shared.Request;
using VaccineAPI.Shared.Response;

namespace VaccineAPI.BusinessLogic.Services.Interface
{
    public interface IVisitDayChangeRequestService
    {
        Task<VisitDayChangeRequestResponse> GetVisitDayChangeRequestAsync(int id);
        Task<IEnumerable<VisitDayChangeRequestResponse>> GetVisitDayChangeRequestsAsync();
        Task<VisitDayChangeRequestResponse> CreateVisitDayChangeRequestAsync(CreateVisitDayChangeRequest request);
        Task<IActionResult> UpdateVisitDayChangeRequestAsync(int id, UpdateVisitDayChangeRequest request);
        Task DeleteVisitDayChangeRequestAsync(int id);
        Task<IEnumerable<VisitDayChangeRequestResponse>> GetVisitDayChangeRequestsByVisitIdAsync(int visitId);
    }
}
