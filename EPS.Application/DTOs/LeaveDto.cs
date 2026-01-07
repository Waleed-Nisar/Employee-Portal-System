using System.ComponentModel.DataAnnotations;
using EPS.Domain.Enums;

namespace EPS.Application.DTOs;

/// <summary>
/// DTO for leave display
/// </summary>
public class LeaveDto
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public string EmployeeEmail { get; set; } = string.Empty;
    public LeaveType LeaveType { get; set; }
    public string LeaveTypeDisplay { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Reason { get; set; } = string.Empty;
    public LeaveStatus Status { get; set; }
    public string StatusDisplay { get; set; } = string.Empty;
    public int? ApprovedBy { get; set; }
    public string? ApproverName { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public string? ApprovalRemarks { get; set; }
    public int TotalDays { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// DTO for creating a leave request
/// </summary>
public class LeaveRequestDto
{
    [Required(ErrorMessage = "Employee ID is required")]
    public int EmployeeId { get; set; }

    [Required(ErrorMessage = "Leave type is required")]
    public LeaveType LeaveType { get; set; }

    [Required(ErrorMessage = "Start date is required")]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "End date is required")]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }

    [Required(ErrorMessage = "Reason is required")]
    [StringLength(500, MinimumLength = 10, ErrorMessage = "Reason must be between 10 and 500 characters")]
    public string Reason { get; set; } = string.Empty;
}

/// <summary>
/// DTO for approving/rejecting leave
/// </summary>
public class LeaveApprovalDto
{
    [Required]
    public int LeaveId { get; set; }

    [Required]
    public int ApprovedBy { get; set; }

    [Required(ErrorMessage = "Action is required (Approve/Reject)")]
    public string Action { get; set; } = string.Empty; // "Approve" or "Reject"

    [StringLength(500, ErrorMessage = "Remarks cannot exceed 500 characters")]
    public string? Remarks { get; set; }
}