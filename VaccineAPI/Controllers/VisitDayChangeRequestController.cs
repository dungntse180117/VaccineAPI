using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VaccineAPI.BusinessLogic.Services.Interface;
using VaccineAPI.Shared.Request;
using VaccineAPI.Shared.Response;

[ApiController]
[Route("api/[controller]")]
public class VisitDayChangeRequestController : ControllerBase
{
    private readonly IVisitDayChangeRequestService _visitDayChangeRequestService;

    public VisitDayChangeRequestController(IVisitDayChangeRequestService visitDayChangeRequestService)
    {
        _visitDayChangeRequestService = visitDayChangeRequestService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetVisitDayChangeRequest(int id)
    {
        var request = await _visitDayChangeRequestService.GetVisitDayChangeRequestAsync(id);

        if (request == null)
        {
            return NotFound();
        }

        return Ok(request);
    }

    [HttpGet]
    public async Task<IActionResult> GetVisitDayChangeRequests()
    {
        var requests = await _visitDayChangeRequestService.GetVisitDayChangeRequestsAsync();
        return Ok(requests);
    }

    [HttpPost]
    public async Task<IActionResult> CreateVisitDayChangeRequest([FromBody] CreateVisitDayChangeRequest request)
    {
        try
        {
            var changeRequest = await _visitDayChangeRequestService.CreateVisitDayChangeRequestAsync(request);
            return CreatedAtAction(nameof(GetVisitDayChangeRequest), new { id = changeRequest.ChangeRequestId }, changeRequest);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateVisitDayChangeRequest(int id, [FromBody] UpdateVisitDayChangeRequest request)
    {
        try
        {
            var result = await _visitDayChangeRequestService.UpdateVisitDayChangeRequestAsync(id, request);
            if (result is NotFoundResult)
            {
                return NotFound();
            }
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVisitDayChangeRequest(int id)
    {
        try
        {
            await _visitDayChangeRequestService.DeleteVisitDayChangeRequestAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
    [HttpGet("visit/{visitId}")]
    public async Task<IActionResult> GetVisitDayChangeRequestsByVisitId(int visitId)
    {
        var requests = await _visitDayChangeRequestService.GetVisitDayChangeRequestsByVisitIdAsync(visitId);
        return Ok(requests);
    }
}