using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineAPI.Shared.Request;
using VaccineAPI.Shared.Response;

namespace VaccineAPI.BusinessLogic.Interface
{
    public interface IFeedbackService
    {
        Task<ManageFeedbackResponse> ManageFeedbackAsync(string action, ManageFeedbackRequest request);
        Task<GetFeedbackResponse> GetFeedbackAsync(int feedbackId);
        Task<IEnumerable<GetFeedbackResponse>> GetAllFeedbacksAsync();
    }
}
