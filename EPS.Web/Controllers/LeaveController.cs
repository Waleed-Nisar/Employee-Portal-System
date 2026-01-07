using EPS.Application.DTOs;
using EPS.Application.Interfaces;
using EPS.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EPS.Web.Controllers;

/// <summary>
/// Leave management controller for MVC views
/// </summary>
[Authorize]
public class LeaveController : Controller
{
    private readonly ILeaveService _leaveService;
    private readonly IEmployeeService _employeeService;

    public LeaveController(ILeaveService leaveService, IEmployeeService employeeService)
    {
        _leaveService = leaveService;
        _employeeService = employeeService;
    }

    /// <summary>
    /// List all leaves
    /// </summary>
    [Authorize(Roles = "Admin,HR Manager,Manager")]
    public async Task<IActionResult> Index(LeaveStatus? status)
    {
        try
        {
            var leaves = status.HasValue
                ? await _leaveService.GetByStatusAsync(status.Value)
                : await _leaveService.GetAllAsync();

            ViewBag.Statuses = Enum.GetValues(typeof(LeaveStatus));
            ViewBag.SelectedStatus = status;

            return View(leaves);
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error loading leaves: {ex.Message}";
            return View(new List<LeaveDto>());
        }
    }

    /// <summary>
    /// Pending leaves for approval
    /// </summary>
    [Authorize(Roles = "Admin,HR Manager,Manager")]
    public async Task<IActionResult> Pending()
    {
        try
        {
            var leaves = await _leaveService.GetPendingLeavesAsync();
            return View(leaves);
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error loading pending leaves: {ex.Message}";
            return View(new List<LeaveDto>());
        }
    }

    /// <summary>
    /// Approve leave
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,HR Manager,Manager")]
    public async Task<IActionResult> Approve(int id, string? remarks)
    {
        try
        {
            // Get employee ID of current user (approver)
            var userEmail = User.Identity?.Name;
            var approver = await _employeeService.GetByEmailAsync(userEmail!);

            if (approver == null)
            {
                TempData["Error"] = "Approver employee record not found";
                return RedirectToAction(nameof(Pending));
            }

            await _leaveService.ApproveLeaveAsync(id, approver.Id, remarks);
            TempData["Success"] = "Leave approved successfully";
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error approving leave: {ex.Message}";
        }

        return RedirectToAction(nameof(Pending));
    }

    /// <summary>
    /// Reject leave
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,HR Manager,Manager")]
    public async Task<IActionResult> Reject(int id, string? remarks)
    {
        try
        {
            var userEmail = User.Identity?.Name;
            var approver = await _employeeService.GetByEmailAsync(userEmail!);

            if (approver == null)
            {
                TempData["Error"] = "Approver employee record not found";
                return RedirectToAction(nameof(Pending));
            }

            await _leaveService.RejectLeaveAsync(id, approver.Id, remarks);
            TempData["Success"] = "Leave rejected successfully";
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error rejecting leave: {ex.Message}";
        }

        return RedirectToAction(nameof(Pending));
    }
}

