using Microsoft.AspNetCore.Identity;

namespace EPS.Infrastructure.Data;

/// <summary>
/// Extends IdentityUser to link with Employee entity
/// </summary>
public class ApplicationUser : IdentityUser
{
    /// <summary>
    /// Link to Employee record (nullable - admin users may not have employee records)
    /// </summary>
    public int? EmployeeId { get; set; }

    /// <summary>
    /// Full name for display purposes
    /// </summary>
    public string? FullName { get; set; }

    /// <summary>
    /// User's refresh token for JWT authentication
    /// </summary>
    public string? RefreshToken { get; set; }

    /// <summary>
    /// Refresh token expiration date
    /// </summary>
    public DateTime? RefreshTokenExpiryTime { get; set; }

    /// <summary>
    /// Account creation date
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Last updated date
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Whether the account is active
    /// </summary>
    public bool IsActive { get; set; } = true;
}