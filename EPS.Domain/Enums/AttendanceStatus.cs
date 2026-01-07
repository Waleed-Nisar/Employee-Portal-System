namespace EPS.Domain.Enums;

/// <summary>
/// Represents the attendance status for a day
/// </summary>
public enum AttendanceStatus
{
    /// <summary>
    /// Employee was present for the full day
    /// </summary>
    Present = 1,

    /// <summary>
    /// Employee was absent
    /// </summary>
    Absent = 2,

    /// <summary>
    /// Employee arrived late
    /// </summary>
    Late = 3,

    /// <summary>
    /// Employee worked half day
    /// </summary>
    HalfDay = 4,

    /// <summary>
    /// Employee was on leave
    /// </summary>
    OnLeave = 5
}