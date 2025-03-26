using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VaccineAPI.BusinessLogic.Interface;
using VaccineAPI.Shared.Request;

[Route("api/[controller]")]
[ApiController]
public class FeedbackController : ControllerBase
{
    private readonly IFeedbackService _feedbackService;

    public FeedbackController(IFeedbackService feedbackService)
    {
        _feedbackService = feedbackService;
    }

    [HttpPost("submit")]
    public async Task<IActionResult> SubmitFeedback([FromBody] FeedbackRequest request)
    {
        var response = await _feedbackService.SubmitFeedbackAsync(request);
        if (response == null) return BadRequest("Failed to submit feedback.");

        return Ok(response);  // Returns a structured response
    }
}
