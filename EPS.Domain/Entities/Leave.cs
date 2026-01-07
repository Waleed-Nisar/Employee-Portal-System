using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EPS.Domain.Enums;

namespace EPS.Domain.Entities;

/// <summary>
/// Represents a leave request from an employee
/// </summary>
public class Leave
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int EmployeeId { get; set; }

    [Required]
    public LeaveType LeaveType { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    [Required]
    [StringLength(500)]
    public string Reason { get; set; } = string.Empty;

    [Required]
    public LeaveStatus Status { get; set; } = LeaveStatus.Pending;

    /// <summary>
    /// Employee ID who approved/rejected the leave
    /// </summary>
    public int? ApprovedBy { get; set; }

    public DateTime? ApprovedDate { get; set; }

    [StringLength(500)]
    public string? ApprovalRemarks { get; set; }

    /// <summary>
    /// Total number of leave days
    /// </summary>
    [NotMapped]
    public int TotalDays => (EndDate.Date - StartDate.Date).Days + 1;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    [ForeignKey(nameof(EmployeeId))]
    public Employee? Employee { get; set; }

    [ForeignKey(nameof(ApprovedBy))]
    public Employee? Approver { get; set; }
}