using EPS.Application.DTOs;
using EPS.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EPS.Application.Interfaces;

namespace EPS.Web.Controllers;

/// <summary>
/// Department management controller for MVC views
/// </summary>
[Authorize(Roles = "Admin,HR Manager")]
public class DepartmentController : Controller
{
    private readonly IDepartmentService _departmentService;
    private readonly IEmployeeService _employeeService;

    public DepartmentController(IDepartmentService departmentService, IEmployeeService employeeService)
    {
        _departmentService = departmentService;
        _employeeService = employeeService;
    }

    /// <summary>
    /// List all departments
    /// </summary>
    public async Task<IActionResult> Index()
    {
        try
        {
            var departments = await _departmentService.GetAllAsync();
            return View(departments);
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error loading departments: {ex.Message}";
            return View(new List<DepartmentDto>());
        }
    }

    /// <summary>
    /// Department details
    /// </summary>
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var department = await _departmentService.GetByIdAsync(id);
            if (department == null)
            {
                TempData["Error"] = "Department not found";
                return RedirectToAction(nameof(Index));
            }

            return View(department);
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error loading department: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }

    /// <summary>
    /// Create department form
    /// </summary>
    public async Task<IActionResult> Create()
    {
        await LoadEmployeesDropdown();
        return View();
    }

    /// <summary>
    /// Create department
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateDepartmentDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                await LoadEmployeesDropdown();
                return View(dto);
            }

            await _departmentService.CreateAsync(dto);
            TempData["Success"] = "Department created successfully";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error creating department: {ex.Message}";
            await LoadEmployeesDropdown();
            return View(dto);
        }
    }

    /// <summary>
    /// Edit department form
    /// </summary>
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var department = await _departmentService.GetByIdAsync(id);
            if (department == null)
            {
                TempData["Error"] = "Department not found";
                return RedirectToAction(nameof(Index));
            }

            var dto = new CreateDepartmentDto
            {
                Name = department.Name,
                Code = department.Code,
                Description = department.Description,
                HeadEmployeeId = department.HeadEmployeeId,
                Location = department.Location,
                IsActive = department.IsActive
            };

            await LoadEmployeesDropdown();
            return View(dto);
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error loading department: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }

    /// <summary>
    /// Edit department
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CreateDepartmentDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                await LoadEmployeesDropdown();
                return View(dto);
            }

            await _departmentService.UpdateAsync(id, dto);
            TempData["Success"] = "Department updated successfully";
            return RedirectToAction(nameof(Details), new { id });
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error updating department: {ex.Message}";
            await LoadEmployeesDropdown();
            return View(dto);
        }
    }

    /// <summary>
    /// Delete department
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _departmentService.DeleteAsync(id);
            if (!result)
            {
                TempData["Error"] = "Department not found";
            }
            else
            {
                TempData["Success"] = "Department deleted successfully";
            }

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error deleting department: {ex.Message}";
            return RedirectToAction(nameof(Details), new { id });
        }
    }

    private async Task LoadEmployeesDropdown()
    {
        var employees = await _employeeService.GetAllAsync();
        ViewBag.Employees = employees;
    }
}