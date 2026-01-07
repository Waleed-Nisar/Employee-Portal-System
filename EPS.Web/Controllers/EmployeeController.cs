using EPS.Application.DTOs;
using EPS.Application.Interfaces;
using EPS.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EPS.Web.Controllers;

/// <summary>
/// Employee management controller for MVC views
/// </summary>
[Authorize(Roles = "Admin,HR Manager,Manager")]
public class EmployeeController : Controller
{
    private readonly IEmployeeService _employeeService;
    private readonly IDepartmentService _departmentService;

    public EmployeeController(IEmployeeService employeeService, IDepartmentService departmentService)
    {
        _employeeService = employeeService;
        _departmentService = departmentService;
    }

    /// <summary>
    /// List all employees with search and filter
    /// </summary>
    public async Task<IActionResult> Index(string? searchTerm, int? departmentId, EmployeeStatus? status, int page = 1)
    {
        try
        {
            var (employees, totalCount) = await _employeeService.GetPaginatedAsync(page, 10, searchTerm, departmentId, status);
            var departments = await _departmentService.GetAllAsync();

            ViewBag.Departments = new SelectList(departments, "Id", "Name", departmentId);
            ViewBag.Statuses = new SelectList(Enum.GetValues(typeof(EmployeeStatus)), status);
            ViewBag.SearchTerm = searchTerm;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalCount / 10.0);
            ViewBag.TotalCount = totalCount;

            return View(employees);
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error loading employees: {ex.Message}";
            return View(new List<EmployeeDto>());
        }
    }

    /// <summary>
    /// Show employee details
    /// </summary>
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var employee = await _employeeService.GetByIdAsync(id);
            if (employee == null)
            {
                TempData["Error"] = "Employee not found";
                return RedirectToAction(nameof(Index));
            }

            return View(employee);
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error loading employee: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }

    /// <summary>
    /// Show create employee form
    /// </summary>
    [Authorize(Roles = "Admin,HR Manager")]
    public async Task<IActionResult> Create()
    {
        await LoadDropdowns();
        return View();
    }

    /// <summary>
    /// Create new employee
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,HR Manager")]
    public async Task<IActionResult> Create(CreateEmployeeDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdowns();
                return View(dto);
            }

            var userEmail = User.Identity?.Name ?? "System";
            await _employeeService.CreateAsync(dto, userEmail);

            TempData["Success"] = "Employee created successfully";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error creating employee: {ex.Message}";
            await LoadDropdowns();
            return View(dto);
        }
    }

    /// <summary>
    /// Show edit employee form
    /// </summary>
    [Authorize(Roles = "Admin,HR Manager")]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var employee = await _employeeService.GetByIdAsync(id);
            if (employee == null)
            {
                TempData["Error"] = "Employee not found";
                return RedirectToAction(nameof(Index));
            }

            var dto = new UpdateEmployeeDto
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                MiddleName = employee.MiddleName,
                LastName = employee.LastName,
                Email = employee.Email,
                Phone = employee.Phone,
                DateOfBirth = employee.DateOfBirth,
                Gender = employee.Gender,
                Address = employee.Address,
                City = employee.City,
                State = employee.State,
                ZipCode = employee.ZipCode,
                Country = employee.Country,
                DepartmentId = employee.DepartmentId,
                DesignationId = employee.DesignationId,
                HireDate = employee.HireDate,
                EndDate = employee.EndDate,
                Status = employee.Status,
                Salary = employee.Salary,
                ManagerId = employee.ManagerId,
                EmergencyContactName = employee.EmergencyContactName,
                EmergencyContactPhone = employee.EmergencyContactPhone
            };

            await LoadDropdowns();
            return View(dto);
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error loading employee: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }

    /// <summary>
    /// Update employee
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,HR Manager")]
    public async Task<IActionResult> Edit(int id, UpdateEmployeeDto dto)
    {
        try
        {
            if (id != dto.Id)
            {
                TempData["Error"] = "Invalid employee ID";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                await LoadDropdowns();
                return View(dto);
            }

            var userEmail = User.Identity?.Name ?? "System";
            await _employeeService.UpdateAsync(dto, userEmail);

            TempData["Success"] = "Employee updated successfully";
            return RedirectToAction(nameof(Details), new { id });
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error updating employee: {ex.Message}";
            await LoadDropdowns();
            return View(dto);
        }
    }

    /// <summary>
    /// Delete employee
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var canDelete = await _employeeService.CanDeleteAsync(id);
            if (!canDelete)
            {
                TempData["Error"] = "Cannot delete employee with active leaves";
                return RedirectToAction(nameof(Details), new { id });
            }

            var result = await _employeeService.DeleteAsync(id);
            if (!result)
            {
                TempData["Error"] = "Employee not found";
            }
            else
            {
                TempData["Success"] = "Employee deleted successfully";
            }

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error deleting employee: {ex.Message}";
            return RedirectToAction(nameof(Details), new { id });
        }
    }

    private async Task LoadDropdowns()
    {
        var departments = await _departmentService.GetAllAsync();
        var employees = await _employeeService.GetAllAsync();

        ViewBag.Departments = new SelectList(departments, "Id", "Name");
        ViewBag.Managers = new SelectList(employees.Where(e => e.Status == EmployeeStatus.Active), "Id", "FullName");
        ViewBag.Statuses = new SelectList(Enum.GetValues(typeof(EmployeeStatus)));
    }
}