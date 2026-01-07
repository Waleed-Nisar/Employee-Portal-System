using EPS.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace EPS.Domain.Entities;

/// <summary>
/// Represents a document attached to an employee record
/// </summary>
public class Document
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int EmployeeId { get; set; }

    [Required]
    [StringLength(200)]
    public string FileName { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    public string FilePath { get; set; } = string.Empty;

    [Required]
    public DocumentType DocumentType { get; set; }

    [StringLength(100)]
    public string? FileType { get; set; }

    public long FileSize { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    [StringLength(100)]
    public string? UploadedBy { get; set; }

    // Navigation Properties
    [ForeignKey(nameof(EmployeeId))]
    public Employee? Employee { get; set; }
}