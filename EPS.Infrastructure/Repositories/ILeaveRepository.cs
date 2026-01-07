using EPS.Domain.Entities;
using EPS.Domain.Enums;

namespace EPS.Infrastructure.Repositories;

/// <summary>
/// Leave-specific repository interface
/// </summary>
public interface ILeaveRepository : IRepository<Leave>
{
    /// <summary>
    /// Get all leaves for an employee
    /// </summary>
    Task<IEnumerable<Leave>> GetByEmployeeIdAsync(int employeeId);

    /// <summary>
    /// Get pending leaves for approval
    /// </summary>
    Task<IEnumerable<Leave>> GetPendingLeavesAsync();

    /// <summary>
    /// Get leaves by status
    /// </summary>
    Task<IEnumerable<Leave>> GetByStatusAsync(LeaveStatus status);

    /// <summary>
    /// Get leaves for a date range
    /// </summary>
    Task<IEnumerable<Leave>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);

    /// <summary>
    /// Get leave with details
    /// </summary>
    Task<Leave?> GetByIdWithDetailsAsync(int id);

    /// <summary>
    /// Check for overlapping leaves
    /// </summary>
    Task<bool> HasOverlappingLeavesAsync(int employeeId, DateTime startDate, DateTime endDate, int? excludeLeaveId = null);

    /// <summary>
    /// Get leave balance for employee
    /// </summary>
    Task<Dictionary<LeaveType, int>> GetLeaveBalanceAsync(int employeeId, int year);

    /// <summary>
    /// Approve leave
    /// </summary>
    Task<bool> ApproveLeavetAsync(int leaveId, int approvedBy, string? remarks = null);

    /// <summary>
    /// Reject leave
    /// </summary>
    Task<bool> RejectLeaveAsync(int leaveId, int approvedBy, string? remarks = null);
}