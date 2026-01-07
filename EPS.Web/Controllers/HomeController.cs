using EPS.Application.Interfaces;
using EPS.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EPS.Web.Controllers;

/// <summary>
/// Home controller for dashboard
/// </summary>
[Authorize]
public class HomeController : Controller
{
    private readonly IEmployeeService _employeeService;
    private readonly ILeaveService _leaveService;
    private readonly IAttendanceService _attendanceService;
    private readonly IDepartmentService _departmentService;

    public HomeController(
        IEmployeeService employeeService,
        ILeaveService leaveService,
        IAttendanceService attendanceService,
        IDepartmentService departmentService)
    {
        _employeeService = employeeService;
        _leaveService = leaveService;
        _attendanceService = attendanceService;
        _departmentService = departmentService;
    }

    /// <summary>
    /// Dashboard with statistics
    /// </summary>
    public async Task<IActionResult> Index()
    {
        try
        {
            var employees = await _employeeService.GetAllAsync();
            var departments = await _departmentService.GetAllAsync();
            var pendingLeaves = await _leaveService.GetPendingLeavesAsync();

            var viewModel = new
            {
                TotalEmployees = employees.Count(),
                ActiveEmployees = employees.Count(e => e.Status == EmployeeStatus.Active),
                TotalDepartments = departments.Count(),
                PendingLeaves = pendingLeaves.Count()
            };

            ViewBag.Statistics = viewModel;

            return View();
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error loading dashboard: {ex.Message}";
            return View();
        }
    }
}