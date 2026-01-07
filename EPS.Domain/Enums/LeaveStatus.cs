namespace EPS.Domain.Enums;

/// <summary>
/// Represents the approval status of a leave request
/// </summary>
public enum LeaveStatus
{
    /// <summary>
    /// Leave request is pending approval
    /// </summary>
    Pending = 1,

    /// <summary>
    /// Leave request has been approved
    /// </summary>
    Approved = 2,

    /// <summary>
    /// Leave request has been rejected
    /// </summary>
    Rejected = 3,

    /// <summary>
    /// Leave request has been cancelled by employee
    /// </summary>
    Cancelled = 4
}