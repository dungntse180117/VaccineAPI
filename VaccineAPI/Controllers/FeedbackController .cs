using Microsoft.AspNetCore.Mvc;
using VaccineAPI.BusinessLogic.Interface;
using VaccineAPI.Shared.Request;

namespace VaccineAPI.Controllers
{
    [ApiController]
    [Route("api/feedbacks")]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;


        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }


        [HttpPost]
        public async Task<IActionResult> CreateFeedback([FromBody] CreateFeedbackRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var actionResult = await _feedbackService.CreateFeedbackAsync(request); 
            var response = actionResult.Value; 

            if (response != null && !response.Success) 
            {
                return BadRequest(response); 
            }

            return Ok(response);
        }


        [HttpGet("{feedbackId}")]
        public async Task<IActionResult> GetFeedbackById(int feedbackId)
        {
            var result = await _feedbackService.GetFeedbackByIdAsync(feedbackId);
            return result.Result is OkObjectResult okResult ? Ok(okResult.Value) : result.Result!; 
        }


        [HttpGet]
        public async Task<IActionResult> ListFeedbacks()
        {
            var result = await _feedbackService.ListFeedbacksAsync();
            return result.Result is OkObjectResult okResult ? Ok(okResult.Value) : result.Result!; 
        }


        [HttpPut("{feedbackId}")]
        public async Task<IActionResult> UpdateFeedback(int feedbackId, [FromBody] UpdateFeedbackRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            request.FeedbackId = feedbackId; 
            var result = await _feedbackService.UpdateFeedbackAsync(request);
            return result.Result is OkObjectResult okResult ? Ok(okResult.Value) : result.Result!; 
        }


        [HttpDelete("{feedbackId}")]
        public async Task<IActionResult> DeleteFeedback(int feedbackId)
        {
            var result = await _feedbackService.DeleteFeedbackAsync(feedbackId);
            return result.Result!; 
        }
        
        [HttpGet("checkExistence/{visitId}")] 
        public async Task<IActionResult> CheckFeedbackExistence(int visitId)
        {
            if (visitId <= 0)
            {
                return BadRequest("VisitId không hợp lệ."); 
            }

            bool exists = await _feedbackService.CheckFeedbackExistsAsync(visitId); 

            return Ok(exists);
        }

    }
}
