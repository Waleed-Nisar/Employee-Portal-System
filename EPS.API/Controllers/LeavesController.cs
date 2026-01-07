using EPS.Application.DTOs;
using EPS.Application.Interfaces;
using EPS.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EPS.API.Controllers;

/// <summary>
/// Leave management controller with approval workflow
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class LeavesController : ControllerBase
{
    private readonly ILeaveService _leaveService;

    public LeavesController(ILeaveService leaveService)
    {
        _leaveService = leaveService;
    }

    /// <summary>
    /// Get all leaves (Admin and HR Manager only)
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin,HR Manager")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var leaves = await _leaveService.GetAllAsync();
            return Ok(new { success = true, message = "Leaves retrieved successfully", data = leaves });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "An error occurred", errors = new[] { ex.Message } });
        }
    }

    /// <summary>
    /// Get leave by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var leave = await _leaveService.GetByIdAsync(id);
            if (leave == null)
            {
                return NotFound(new { success = false, message = $"Leave with ID {id} not found" });
            }

            return Ok(new { success = true, message = "Leave retrieved successfully", data = leave });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "An error occurred", errors = new[] { ex.Message } });
        }
    }

    /// <summary>
    /// Get current user's leaves
    /// </summary>
    [HttpGet("my-leaves")]
    [Authorize]
    public async Task<IActionResult> GetMyLeaves()
    {
        try
        {
            var employeeIdClaim = User.FindFirst("EmployeeId")?.Value;
            if (string.IsNullOrEmpty(employeeIdClaim) || !int.TryParse(employeeIdClaim, out int employeeId))
            {
                return BadRequest(new { success = false, message = "Employee ID not found in token" });
            }

            var leaves = await _leaveService.GetByEmployeeIdAsync(employeeId);
            return Ok(new { success = true, message = "Your leaves retrieved successfully", data = leaves });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "An error occurred", errors = new[] { ex.Message } });
        }
    }

    /// <summary>
    /// Get leaves by employee ID (Managers and HR only)
    /// </summary>
    [HttpGet("employee/{employeeId}")]
    [Authorize(Roles = "Admin,HR Manager,Manager")]
    public async Task<IActionResult> GetByEmployeeId(int employeeId)
    {
        try
        {
            var leaves = await _leaveService.GetByEmployeeIdAsync(employeeId);
            return Ok(new { success = true, message = "Leaves retrieved successfully", data = leaves });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "An error occurred", errors = new[] { ex.Message } });
        }
    }

    /// <summary>
    /// Get pending leaves for approval
    /// </summary>
    [HttpGet("pending")]
    [Authorize(Roles = "Admin,HR Manager,Manager")]
    public async Task<IActionResult> GetPendingLeaves()
    {
        try
        {
            var leaves = await _leaveService.GetPendingLeavesAsync();
            return Ok(new { success = true, message = "Pending leaves retrieved successfully", data = leaves });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "An error occurred", errors = new[] { ex.Message } });
        }
    }

    /// <summary>
    /// Request leave (All authenticated users)
    /// </summary>
    [HttpPost("request")]
    [Authorize]
    public async Task<IActionResult> RequestLeave([FromBody] LeaveRequestDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Validation failed", errors = ModelState });
            }

            var leave = await _leaveService.RequestLeaveAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = leave.Id },
                new { success = true, message = "Leave request submitted successfully", data = leave });
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
    /// Approve or reject leave (HR Manager and Manager only)
    /// </summary>
    [HttpPut("approve/{id}")]
    [Authorize(Roles = "Admin,HR Manager,Manager")]
    public async Task<IActionResult> ProcessLeave(int id, [FromBody] LeaveApprovalDto dto)
    {
        try
        {
            if (id != dto.LeaveId)
            {
                return BadRequest(new { success = false, message = "ID mismatch" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Validation failed", errors = ModelState });
            }

            LeaveDto leave;
            if (dto.Action.ToLower() == "approve")
            {
                leave = await _leaveService.ApproveLeaveAsync(dto.LeaveId, dto.ApprovedBy, dto.Remarks);
            }
            else if (dto.Action.ToLower() == "reject")
            {
                leave = await _leaveService.RejectLeaveAsync(dto.LeaveId, dto.ApprovedBy, dto.Remarks);
            }
            else
            {
                return BadRequest(new { success = false, message = "Invalid action. Use 'Approve' or 'Reject'" });
            }

            return Ok(new { success = true, message = $"Leave {dto.Action.ToLower()}d successfully", data = leave });
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
    /// Cancel leave request
    /// </summary>
    [HttpPut("cancel/{id}")]
    [Authorize]
    public async Task<IActionResult> CancelLeave(int id)
    {
        try
        {
            var employeeIdClaim = User.FindFirst("EmployeeId")?.Value;
            if (string.IsNullOrEmpty(employeeIdClaim) || !int.TryParse(employeeIdClaim, out int employeeId))
            {
                return BadRequest(new { success = false, message = "Employee ID not found in token" });
            }

            var result = await _leaveService.CancelLeaveAsync(id, employeeId);
            if (!result)
            {
                return NotFound(new { success = false, message = $"Leave with ID {id} not found" });
            }

            return Ok(new { success = true, message = "Leave cancelled successfully" });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { success = false, message = ex.Message });
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
    /// Get leave balance for employee
    /// </summary>
    [HttpGet("balance/{employeeId}/{year}")]
    [Authorize]
    public async Task<IActionResult> GetLeaveBalance(int employeeId, int year)
    {
        try
        {
            var balance = await _leaveService.GetLeaveBalanceAsync(employeeId, year);
            return Ok(new { success = true, message = "Leave balance retrieved successfully", data = balance });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "An error occurred", errors = new[] { ex.Message } });
        }
    }
}