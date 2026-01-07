namespace EPS.Domain.Enums;

/// <summary>
/// Represents the current status of an employee
/// </summary>
public enum EmployeeStatus
{
    /// <summary>
    /// Employee is currently active and working
    /// </summary>
    Active = 1,

    /// <summary>
    /// Employee is temporarily inactive
    /// </summary>
    Inactive = 2,

    /// <summary>
    /// Employee is currently on leave
    /// </summary>
    OnLeave = 3,

    /// <summary>
    /// Employee has been terminated
    /// </summary>
    Terminated = 4
}