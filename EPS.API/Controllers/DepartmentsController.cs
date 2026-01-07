using EPS.Application.DTOs;
using EPS.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EPS.API.Controllers;

/// <summary>
/// Attendance tracking controller
/// </summary>
/// <summary>
/// Department management controller
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DepartmentsController : ControllerBase
{
    private readonly IDepartmentService _departmentService;

    public DepartmentsController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    /// <summary>
    /// Get all departments
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var departments = await _departmentService.GetAllAsync();
            return Ok(new { success = true, message = "Departments retrieved successfully", data = departments });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "An error occurred", errors = new[] { ex.Message } });
        }
    }

    /// <summary>
    /// Get active departments
    /// </summary>
    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        try
        {
            var departments = await _departmentService.GetActiveDepartmentsAsync();
            return Ok(new { success = true, message = "Active departments retrieved successfully", data = departments });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "An error occurred", errors = new[] { ex.Message } });
        }
    }

    /// <summary>
    /// Get department by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var department = await _departmentService.GetByIdAsync(id);
            if (department == null)
            {
                return NotFound(new { success = false, message = $"Department with ID {id} not found" });
            }

            return Ok(new { success = true, message = "Department retrieved successfully", data = department });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "An error occurred", errors = new[] { ex.Message } });
        }
    }

    /// <summary>
    /// Create department
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin,HR Manager")]
    public async Task<IActionResult> Create([FromBody] CreateDepartmentDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Validation failed", errors = ModelState });
            }

            var department = await _departmentService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = department.Id },
                new { success = true, message = "Department created successfully", data = department });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "An error occurred", errors = new[] { ex.Message } });
        }
    }

    /// <summary>
    /// Update department
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,HR Manager")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateDepartmentDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Validation failed", errors = ModelState });
            }

            var department = await _departmentService.UpdateAsync(id, dto);
            return Ok(new { success = true, message = "Department updated successfully", data = department });
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
    /// Delete department
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _departmentService.DeleteAsync(id);
            if (!result)
            {
                return NotFound(new { success = false, message = $"Department with ID {id} not found" });
            }

            return Ok(new { success = true, message = "Department deleted successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "An error occurred", errors = new[] { ex.Message } });
        }
    }
}