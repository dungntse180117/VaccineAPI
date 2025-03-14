using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VaccineAPI.BusinessLogic.Services.Interface;
using VaccineAPI.Shared.Request;
using VaccineAPI.Shared.Response;

[ApiController]
[Route("api/[controller]")]
public class VaccinationHistoryController : ControllerBase
{
    private readonly IVaccinationHistoryService _vaccinationHistoryService;

    public VaccinationHistoryController(IVaccinationHistoryService vaccinationHistoryService)
    {
        _vaccinationHistoryService = vaccinationHistoryService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetVaccinationHistory(int id)
    {
        var history = await _vaccinationHistoryService.GetVaccinationHistoryAsync(id);
        if (history == null) return NotFound();
        return Ok(history);
    }

    [HttpGet]
    public async Task<IActionResult> GetVaccinationHistories()
    {
        var histories = await _vaccinationHistoryService.GetVaccinationHistoriesAsync();
        return Ok(histories);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateVaccinationHistory(int id, [FromBody] UpdateVaccinationHistoryRequest request)
    {
        try
        {
            var result = await _vaccinationHistoryService.UpdateVaccinationHistoryAsync(id, request);
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
    public async Task<IActionResult> DeleteVaccinationHistory(int id)
    {
        try
        {
            await _vaccinationHistoryService.DeleteVaccinationHistoryAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("patient/{patientId}")]
    public async Task<IActionResult> GetVaccinationHistoriesByPatientId(int patientId)
    {
        var histories = await _vaccinationHistoryService.GetVaccinationHistoriesByPatientIdAsync(patientId);
        return Ok(histories);
    }

    [HttpGet("visit/{visitId}")]
    public async Task<IActionResult> GetVaccinationHistoriesByVisitId(int visitId)
    {
        var histories = await _vaccinationHistoryService.GetVaccinationHistoriesByVisitIdAsync(visitId);
        return Ok(histories);
    }
}