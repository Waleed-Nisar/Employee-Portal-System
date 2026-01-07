using EPS.Domain.Enums;

namespace EPS.Application.DTOs;

/// <summary>
/// DTO for employee display/retrieval
/// </summary>
public class EmployeeDto
{
    public int Id { get; set; }
    public string EmployeeId { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public string? Country { get; set; }

    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;

    public int DesignationId { get; set; }
    public string DesignationTitle { get; set; } = string.Empty;

    public DateTime HireDate { get; set; }
    public DateTime? EndDate { get; set; }
    public EmployeeStatus Status { get; set; }
    public string StatusDisplay { get; set; } = string.Empty;
    public decimal Salary { get; set; }

    public int? ManagerId { get; set; }
    public string? ManagerName { get; set; }

    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}