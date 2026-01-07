namespace EPS.Domain.Enums;

/// <summary>
/// Represents user roles in the system for authorization
/// </summary>
public static class UserRole
{
    public const string Admin = "Admin";
    public const string HRManager = "HR Manager";
    public const string Manager = "Manager";
    public const string Employee = "Employee";

    /// <summary>
    /// Returns all available roles
    /// </summary>
    public static string[] GetAllRoles()
    {
        return new[] { Admin, HRManager, Manager, Employee };
    }
}