using System.ComponentModel.DataAnnotations;

namespace EPS.Application.DTOs;

/// <summary>
/// DTO for department display
/// </summary>
public class DepartmentDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string? Description { get; set; }
    public int? HeadEmployeeId { get; set; }
    public string? HeadEmployeeName { get; set; }
    public string? Location { get; set; }
    public bool IsActive { get; set; }
    public int EmployeeCount { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// DTO for designation display
/// </summary>
public class DesignationDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string? Description { get; set; }
    public int Level { get; set; }
    public decimal? MinSalary { get; set; }
    public decimal? MaxSalary { get; set; }
    public bool IsActive { get; set; }
    public int EmployeeCount { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// DTO for creating/updating department
/// </summary>
public class CreateDepartmentDto
{
    [Required(ErrorMessage = "Department name is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Department name must be between 2 and 100 characters")]
    public string Name { get; set; } = string.Empty;

    [StringLength(50, ErrorMessage = "Code cannot exceed 50 characters")]
    public string? Code { get; set; }

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string? Description { get; set; }

    public int? HeadEmployeeId { get; set; }

    [StringLength(100, ErrorMessage = "Location cannot exceed 100 characters")]
    public string? Location { get; set; }

    public bool IsActive { get; set; } = true;
}

/// <summary>
/// DTO for creating/updating designation
/// </summary>
public class CreateDesignationDto
{
    [Required(ErrorMessage = "Title is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Title must be between 2 and 100 characters")]
    public string Title { get; set; } = string.Empty;

    [StringLength(50, ErrorMessage = "Code cannot exceed 50 characters")]
    public string? Code { get; set; }

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Level is required")]
    [Range(1, 10, ErrorMessage = "Level must be between 1 and 10")]
    public int Level { get; set; }

    [Range(0, 999999999, ErrorMessage = "Minimum salary must be a positive number")]
    public decimal? MinSalary { get; set; }

    [Range(0, 999999999, ErrorMessage = "Maximum salary must be a positive number")]
    public decimal? MaxSalary { get; set; }

    public bool IsActive { get; set; } = true;
}