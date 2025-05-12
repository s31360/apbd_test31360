using apbd_test31360.Services;
using apbd_test31360.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace apbd_test31360.Controllers;

[ApiController]
[Route("api/appointments")]
public class AppointmentController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAppointment(int id)
    {
        var result = await _appointmentService.FindAppointmentAsync(id);

        if (result == null)
        {
            return NotFound($"Appointment witn ID {id} not found.");
        }
        
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddAppointment([FromBody] NewAppointmentRequestDTO dto)
    {
        try
        {
            await _appointmentService.AddAppointmentAsync(dto);
            return CreatedAtAction(nameof(GetAppointment), new { id = dto.AppointmentId }, null);
        }
        catch (Exception e)
        {
            return BadRequest(new { error = e.Message });
        }
    }
    
}