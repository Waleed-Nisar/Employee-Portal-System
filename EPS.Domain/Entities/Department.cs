using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPS.Domain.Entities;

/// <summary>
/// Represents a department in the organization
/// </summary>
public class Department
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(50)]
    public string? Code { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// Department head employee ID
    /// </summary>
    public int? HeadEmployeeId { get; set; }

    [StringLength(100)]
    public string? Location { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    [ForeignKey(nameof(HeadEmployeeId))]
    public Employee? HeadEmployee { get; set; }

    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}