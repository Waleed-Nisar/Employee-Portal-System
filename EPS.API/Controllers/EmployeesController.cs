using EPS.Application.DTOs;
using EPS.Application.Interfaces;
using EPS.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EPS.API.Controllers;

/// <summary>
/// Employee management controller with role-based access control
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeesController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    /// <summary>
    /// Get all employees with pagination and filters
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin,HR Manager,Manager")]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] int? departmentId = null,
        [FromQuery] EmployeeStatus? status = null)
    {
        try
        {
            var (employees, totalCount) = await _employeeService.GetPaginatedAsync(page, pageSize, searchTerm, departmentId, status);

            return Ok(new
            {
                success = true,
                message = "Employees retrieved successfully",
                data = employees,
                pagination = new
                {
                    currentPage = page,
                    pageSize,
                    totalCount,
                    totalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
                }
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "An error occurred", errors = new[] { ex.Message } });
        }
    }

    /// <summary>
    /// Get employee by ID
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,HR Manager,Manager")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var employee = await _employeeService.GetByIdAsync(id);
            if (employee == null)
            {
                return NotFound(new { success = false, message = $"Employee with ID {id} not found" });
            }

            return Ok(new { success = true, message = "Employee retrieved successfully", data = employee });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "An error occurred", errors = new[] { ex.Message } });
        }
    }

    /// <summary>
    /// Get employee by employee ID (EMP-XXXX)
    /// </summary>
    [HttpGet("by-employee-id/{employeeId}")]
    [Authorize(Roles = "Admin,HR Manager,Manager")]
    public async Task<IActionResult> GetByEmployeeId(string employeeId)
    {
        try
        {
            var employee = await _employeeService.GetByEmployeeIdAsync(employeeId);
            if (employee == null)
            {
                return NotFound(new { success = false, message = $"Employee with ID {employeeId} not found" });
            }

            return Ok(new { success = true, message = "Employee retrieved successfully", data = employee });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "An error occurred", errors = new[] { ex.Message } });
        }
    }

    /// <summary>
    /// Search employees by name or employee ID
    /// </summary>
    [HttpGet("search")]
    [Authorize(Roles = "Admin,HR Manager,Manager")]
    public async Task<IActionResult> Search([FromQuery] string searchTerm)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return BadRequest(new { success = false, message = "Search term is required" });
            }

            var employees = await _employeeService.SearchAsync(searchTerm);
            return Ok(new { success = true, message = "Search completed successfully", data = employees });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "An error occurred", errors = new[] { ex.Message } });
        }
    }

    /// <summary>
    /// Get employees by department
    /// </summary>
    [HttpGet("by-department/{departmentId}")]
    [Authorize(Roles = "Admin,HR Manager,Manager")]
    public async Task<IActionResult> GetByDepartment(int departmentId)
    {
        try
        {
            var employees = await _employeeService.GetByDepartmentAsync(departmentId);
            return Ok(new { success = true, message = "Employees retrieved successfully", data = employees });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "An error occurred", errors = new[] { ex.Message } });
        }
    }

    /// <summary>
    /// Create new employee (Admin and HR Manager only)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin,HR Manager")]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Validation failed", errors = ModelState });
            }

            var userEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value ?? "System";
            var employee = await _employeeService.CreateAsync(dto, userEmail);

            return CreatedAtAction(nameof(GetById), new { id = employee.Id },
                new { success = true, message = "Employee created successfully", data = employee });
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
    /// Update employee (Admin and HR Manager only)
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,HR Manager")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateEmployeeDto dto)
    {
        try
        {
            if (id != dto.Id)
            {
                return BadRequest(new { success = false, message = "ID mismatch" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Validation failed", errors = ModelState });
            }

            var userEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value ?? "System";
            var employee = await _employeeService.UpdateAsync(dto, userEmail);

            return Ok(new { success = true, message = "Employee updated successfully", data = employee });
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
    /// Delete employee (Admin only)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var canDelete = await _employeeService.CanDeleteAsync(id);
            if (!canDelete)
            {
                return BadRequest(new { success = false, message = "Cannot delete employee with active leaves" });
            }

            var result = await _employeeService.DeleteAsync(id);
            if (!result)
            {
                return NotFound(new { success = false, message = $"Employee with ID {id} not found" });
            }

            return Ok(new { success = true, message = "Employee deleted successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "An error occurred", errors = new[] { ex.Message } });
        }
    }
}