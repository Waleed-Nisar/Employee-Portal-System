namespace EPS.Domain.Enums;

/// <summary>
/// Represents different types of leave
/// </summary>
public enum LeaveType
{
    /// <summary>
    /// Leave due to illness
    /// </summary>
    Sick = 1,

    /// <summary>
    /// Casual leave for personal matters
    /// </summary>
    Casual = 2,

    /// <summary>
    /// Annual vacation leave
    /// </summary>
    Annual = 3,

    /// <summary>
    /// Unpaid leave
    /// </summary>
    Unpaid = 4,

    /// <summary>
    /// Maternity leave
    /// </summary>
    Maternity = 5,

    /// <summary>
    /// Paternity leave
    /// </summary>
    Paternity = 6,

    /// <summary>
    /// Emergency leave
    /// </summary>
    Emergency = 7
}