
using EPS.Application.DTOs;
using EPS.Application.Interfaces;
using EPS.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EPS.Web.Controllers;

/// <summary>
/// Attendance management controller for MVC views
/// </summary>
[Authorize]
public class AttendanceController : Controller
{
    private readonly IAttendanceService _attendanceService;
    private readonly IEmployeeService _employeeService;

    public AttendanceController(IAttendanceService attendanceService, IEmployeeService employeeService)
    {
        _attendanceService = attendanceService;
        _employeeService = employeeService;
    }

    /// <summary>
    /// List attendance records
    /// </summary>
    [Authorize(Roles = "Admin,HR Manager,Manager")]
    public async Task<IActionResult> Index(int? employeeId, DateTime? startDate, DateTime? endDate)
    {
        try
        {
            IEnumerable<AttendanceDto> attendances;

            if (employeeId.HasValue)
            {
                if (startDate.HasValue && endDate.HasValue)
                {
                    attendances = await _attendanceService.GetByDateRangeAsync(employeeId.Value, startDate.Value, endDate.Value);
                }
                else
                {
                    attendances = await _attendanceService.GetByEmployeeIdAsync(employeeId.Value);
                }
            }
            else
            {
                attendances = new List<AttendanceDto>();
            }

            var employees = await _employeeService.GetAllAsync();
            ViewBag.Employees = employees;
            ViewBag.SelectedEmployeeId = employeeId;
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;

            return View(attendances);
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error loading attendance: {ex.Message}";
            return View(new List<AttendanceDto>());
        }
    }

    /// <summary>
    /// Mark attendance form
    /// </summary>
    [Authorize(Roles = "Admin,HR Manager,Manager")]
    public async Task<IActionResult> Mark()
    {
        var employees = await _employeeService.GetAllAsync();
        ViewBag.Employees = employees;
        ViewBag.Statuses = Enum.GetValues(typeof(AttendanceStatus));
        return View();
    }

    /// <summary>
    /// Mark attendance
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,HR Manager,Manager")]
    public async Task<IActionResult> Mark(MarkAttendanceDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var employees = await _employeeService.GetAllAsync();
                ViewBag.Employees = employees;
                ViewBag.Statuses = Enum.GetValues(typeof(AttendanceStatus));
                return View(dto);
            }

            await _attendanceService.MarkAttendanceAsync(dto);
            TempData["Success"] = "Attendance marked successfully";
            return RedirectToAction(nameof(Index), new { employeeId = dto.EmployeeId });
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error marking attendance: {ex.Message}";
            var employees = await _employeeService.GetAllAsync();
            ViewBag.Employees = employees;
            ViewBag.Statuses = Enum.GetValues(typeof(AttendanceStatus));
            return View(dto);
        }
    }
}