using EPS.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace EPS.Domain.Entities;

/// <summary>
/// Represents an employee in the organization
/// </summary>
public class Employee
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Unique employee identifier (Format: EMP-XXXX)
    /// </summary>
    [Required]
    [StringLength(20)]
    public string EmployeeId { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [StringLength(50)]
    public string? MiddleName { get; set; }

    [Required]
    [StringLength(50)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [Phone]
    [StringLength(20)]
    public string? Phone { get; set; }

    [Required]
    public DateTime DateOfBirth { get; set; }

    [StringLength(10)]
    public string? Gender { get; set; }

    [StringLength(200)]
    public string? Address { get; set; }

    [StringLength(50)]
    public string? City { get; set; }

    [StringLength(50)]
    public string? State { get; set; }

    [StringLength(10)]
    public string? ZipCode { get; set; }

    [StringLength(50)]
    public string? Country { get; set; }

    // Employment Details
    [Required]
    public int DepartmentId { get; set; }

    [Required]
    public int DesignationId { get; set; }

    [Required]
    public DateTime HireDate { get; set; }

    public DateTime? EndDate { get; set; }

    [Required]
    public EmployeeStatus Status { get; set; } = EmployeeStatus.Active;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Salary { get; set; }

    // Link to Identity User (nullable - not all employees need login access)
    public string? UserId { get; set; }

    // Manager relationship
    public int? ManagerId { get; set; }

    // Emergency Contact
    [StringLength(100)]
    public string? EmergencyContactName { get; set; }

    [Phone]
    [StringLength(20)]
    public string? EmergencyContactPhone { get; set; }

    // Audit Fields
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [StringLength(100)]
    public string? CreatedBy { get; set; }

    [StringLength(100)]
    public string? UpdatedBy { get; set; }

    // Navigation Properties
    [ForeignKey(nameof(DepartmentId))]
    public Department? Department { get; set; }

    [ForeignKey(nameof(DesignationId))]
    public Designation? Designation { get; set; }

    [ForeignKey(nameof(ManagerId))]
    public Employee? Manager { get; set; }

    public ICollection<Employee> Subordinates { get; set; } = new List<Employee>();
    public ICollection<Leave> Leaves { get; set; } = new List<Leave>();
    public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    public ICollection<Document> Documents { get; set; } = new List<Document>();

    /// <summary>
    /// Full name of the employee
    /// </summary>
    [NotMapped]
    public string FullName => $"{FirstName} {MiddleName} {LastName}".Replace("  ", " ").Trim();
}