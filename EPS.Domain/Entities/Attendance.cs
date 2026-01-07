using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EPS.Domain.Enums;

namespace EPS.Domain.Entities;

/// <summary>
/// Represents daily attendance record for an employee
/// </summary>
public class Attendance
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int EmployeeId { get; set; }

    [Required]
    public DateTime Date { get; set; }

    public DateTime? CheckInTime { get; set; }

    public DateTime? CheckOutTime { get; set; }

    [Required]
    public AttendanceStatus Status { get; set; } = AttendanceStatus.Present;

    [StringLength(500)]
    public string? Remarks { get; set; }

    /// <summary>
    /// Total working hours for the day
    /// </summary>
    [NotMapped]
    public double? WorkingHours
    {
        get
        {
            if (CheckInTime.HasValue && CheckOutTime.HasValue)
            {
                return (CheckOutTime.Value - CheckInTime.Value).TotalHours;
            }
            return null;
        }
    }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    [ForeignKey(nameof(EmployeeId))]
    public Employee? Employee { get; set; }
}