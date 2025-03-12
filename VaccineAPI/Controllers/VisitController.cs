using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VaccineAPI.BusinessLogic.Services.Interface;
using VaccineAPI.Shared.Request;
using VaccineAPI.Shared.Response;

[ApiController]
[Route("api/[controller]")]
public class VisitController : ControllerBase
{
    private readonly IVisitService _visitService;

    public VisitController(IVisitService visitService)
    {
        _visitService = visitService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetVisit(int id)
    {
        var visit = await _visitService.GetVisitAsync(id);
        if (visit == null) return NotFound();
        return Ok(visit);
    }

    [HttpGet]
    public async Task<IActionResult> GetVisits()
    {
        var visits = await _visitService.GetVisitsAsync();
        return Ok(visits);
    }

    [HttpPost]
    public async Task<IActionResult> CreateVisit([FromBody] CreateVisitRequest request)
    {
        try
        {
            var visit = await _visitService.CreateVisitAsync(request);
            return CreatedAtAction(nameof(GetVisit), new { id = visit.VisitID }, visit);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateVisit(int id, [FromBody] VisitResponse request)
    {
        try
        {
            await _visitService.UpdateVisitAsync(id, request);
            return NoContent();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVisit(int id)
    {
        try
        {
            await _visitService.DeleteVisitAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
       
    
    [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateVisitStatus(int id, [FromBody] UpdateVisitStatusRequest request)
        {
            try
            {
                var result = await _visitService.UpdateVisitStatusAsync(id, request);
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
   
    [HttpGet("appointment/{appointmentId}")]
    public async Task<IActionResult> GetVisitsByAppointmentId(int appointmentId)
    {
        var visits = await _visitService.GetVisitsByAppointmentIdAsync(appointmentId);
        return Ok(visits);
    }
}
