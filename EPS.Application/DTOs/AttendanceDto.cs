using System.ComponentModel.DataAnnotations;
using EPS.Domain.Enums;

namespace EPS.Application.DTOs;

/// <summary>
/// DTO for attendance display
/// </summary>
public class AttendanceDto
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public DateTime? CheckInTime { get; set; }
    public DateTime? CheckOutTime { get; set; }
    public AttendanceStatus Status { get; set; }
    public string StatusDisplay { get; set; } = string.Empty;
    public string? Remarks { get; set; }
    public double? WorkingHours { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// DTO for marking attendance
/// </summary>
public class MarkAttendanceDto
{
    [Required(ErrorMessage = "Employee ID is required")]
    public int EmployeeId { get; set; }

    [Required(ErrorMessage = "Date is required")]
    [DataType(DataType.Date)]
    public DateTime Date { get; set; }

    [Required(ErrorMessage = "Check-in time is required")]
    public DateTime CheckInTime { get; set; }

    public DateTime? CheckOutTime { get; set; }

    [Required(ErrorMessage = "Status is required")]
    public AttendanceStatus Status { get; set; }

    [StringLength(500, ErrorMessage = "Remarks cannot exceed 500 characters")]
    public string? Remarks { get; set; }
}