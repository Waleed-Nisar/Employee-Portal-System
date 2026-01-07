using System.ComponentModel.DataAnnotations;
using EPS.Domain.Enums;

namespace EPS.Application.DTOs;

/// <summary>
/// DTO for updating an existing employee
/// </summary>
public class UpdateEmployeeDto
{
    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage = "First name is required")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters")]
    public string FirstName { get; set; } = string.Empty;

    [StringLength(50, ErrorMessage = "Middle name cannot exceed 50 characters")]
    public string? MiddleName { get; set; }

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 50 characters")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Invalid phone format")]
    [StringLength(20, ErrorMessage = "Phone cannot exceed 20 characters")]
    public string? Phone { get; set; }

    [Required(ErrorMessage = "Date of birth is required")]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }

    [StringLength(10, ErrorMessage = "Gender cannot exceed 10 characters")]
    public string? Gender { get; set; }

    [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters")]
    public string? Address { get; set; }

    [StringLength(50, ErrorMessage = "City cannot exceed 50 characters")]
    public string? City { get; set; }

    [StringLength(50, ErrorMessage = "State cannot exceed 50 characters")]
    public string? State { get; set; }

    [StringLength(10, ErrorMessage = "Zip code cannot exceed 10 characters")]
    public string? ZipCode { get; set; }

    [StringLength(50, ErrorMessage = "Country cannot exceed 50 characters")]
    public string? Country { get; set; }

    [Required(ErrorMessage = "Department is required")]
    public int DepartmentId { get; set; }

    [Required(ErrorMessage = "Designation is required")]
    public int DesignationId { get; set; }

    [Required(ErrorMessage = "Hire date is required")]
    [DataType(DataType.Date)]
    public DateTime HireDate { get; set; }

    [DataType(DataType.Date)]
    public DateTime? EndDate { get; set; }

    [Required(ErrorMessage = "Status is required")]
    public EmployeeStatus Status { get; set; }

    [Required(ErrorMessage = "Salary is required")]
    [Range(0, 999999999, ErrorMessage = "Salary must be a positive number")]
    public decimal Salary { get; set; }

    public int? ManagerId { get; set; }

    [StringLength(100, ErrorMessage = "Emergency contact name cannot exceed 100 characters")]
    public string? EmergencyContactName { get; set; }

    [Phone(ErrorMessage = "Invalid emergency contact phone format")]
    [StringLength(20, ErrorMessage = "Emergency contact phone cannot exceed 20 characters")]
    public string? EmergencyContactPhone { get; set; }
}