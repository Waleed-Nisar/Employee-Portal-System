using EPS.Application.DTOs;
using EPS.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EPS.API.Controllers;

/// <summary>
/// Attendance tracking controller
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AttendanceController : ControllerBase
{
    private readonly IAttendanceService _attendanceService;

    public AttendanceController(IAttendanceService attendanceService)
    {
        _attendanceService = attendanceService;
    }

    /// <summary>
    /// Mark attendance
    /// </summary>
    [HttpPost("mark")]
    [Authorize]
    public async Task<IActionResult> MarkAttendance([FromBody] MarkAttendanceDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Validation failed", errors = ModelState });
            }

            var attendance = await _attendanceService.MarkAttendanceAsync(dto);
            return Ok(new { success = true, message = "Attendance marked successfully", data = attendance });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "An error occurred", errors = new[] { ex.Message } });
        }
    }

    /// <summary>
    /// Get current user's attendance
    /// </summary>
    [HttpGet("my-attendance")]
    [Authorize]
    public async Task<IActionResult> GetMyAttendance([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
    {
        try
        {
            var employeeIdClaim = User.FindFirst("EmployeeId")?.Value;
            if (string.IsNullOrEmpty(employeeIdClaim) || !int.TryParse(employeeIdClaim, out int employeeId))
            {
                return BadRequest(new { success = false, message = "Employee ID not found in token" });
            }

            if (startDate.HasValue && endDate.HasValue)
            {
                var attendances = await _attendanceService.GetByDateRangeAsync(employeeId, startDate.Value, endDate.Value);
                return Ok(new { success = true, message = "Attendance records retrieved successfully", data = attendances });
            }
            else
            {
                var attendances = await _attendanceService.GetByEmployeeIdAsync(employeeId);
                return Ok(new { success = true, message = "Attendance records retrieved successfully", data = attendances });
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "An error occurred", errors = new[] { ex.Message } });
        }
    }

    /// <summary>
    /// Get attendance by employee ID
    /// </summary>
    [HttpGet("employee/{employeeId}")]
    [Authorize(Roles = "Admin,HR Manager,Manager")]
    public async Task<IActionResult> GetByEmployeeId(int employeeId, [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
    {
        try
        {
            if (startDate.HasValue && endDate.HasValue)
            {
                var attendances = await _attendanceService.GetByDateRangeAsync(employeeId, startDate.Value, endDate.Value);
                return Ok(new { success = true, message = "Attendance records retrieved successfully", data = attendances });
            }
            else
            {
                var attendances = await _attendanceService.GetByEmployeeIdAsync(employeeId);
                return Ok(new { success = true, message = "Attendance records retrieved successfully", data = attendances });
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "An error occurred", errors = new[] { ex.Message } });
        }
    }

    /// <summary>
    /// Get attendance summary
    /// </summary>
    [HttpGet("summary/{employeeId}")]
    [Authorize]
    public async Task<IActionResult> GetAttendanceSummary(int employeeId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        try
        {
            var summary = await _attendanceService.GetAttendanceSummaryAsync(employeeId, startDate, endDate);
            return Ok(new { success = true, message = "Attendance summary retrieved successfully", data = summary });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "An error occurred", errors = new[] { ex.Message } });
        }
    }
}