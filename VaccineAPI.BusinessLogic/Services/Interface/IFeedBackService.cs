using Microsoft.AspNetCore.Mvc;
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
        Task<ActionResult<FeedbackResponse>> CreateFeedbackAsync(CreateFeedbackRequest request); 
        Task<ActionResult<FeedbackResponse>> GetFeedbackByIdAsync(int feedbackId);
        Task<ActionResult<List<FeedbackResponse>>> ListFeedbacksAsync(); 
        Task<ActionResult<FeedbackResponse>> UpdateFeedbackAsync(UpdateFeedbackRequest request);
        Task<ActionResult<IActionResult>> DeleteFeedbackAsync(int feedbackId);
        Task<bool> CheckFeedbackExistsAsync(int visitId);
    }
}
