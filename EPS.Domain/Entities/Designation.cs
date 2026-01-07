using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPS.Domain.Entities;

/// <summary>
/// Represents a job designation/title in the organization
/// </summary>
public class Designation
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [StringLength(50)]
    public string? Code { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// Hierarchical level (1 = Top level, higher numbers = lower levels)
    /// </summary>
    public int Level { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? MinSalary { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? MaxSalary { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}