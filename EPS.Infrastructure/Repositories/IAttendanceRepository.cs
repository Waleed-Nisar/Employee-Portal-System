using EPS.Domain.Entities;
using EPS.Domain.Enums;

namespace EPS.Infrastructure.Repositories;

/// <summary>
/// Attendance-specific repository interface
/// </summary>
public interface IAttendanceRepository : IRepository<Attendance>
{
    /// <summary>
    /// Get attendance for an employee
    /// </summary>
    Task<IEnumerable<Attendance>> GetByEmployeeIdAsync(int employeeId);

    /// <summary>
    /// Get attendance for a specific date
    /// </summary>
    Task<Attendance?> GetByEmployeeAndDateAsync(int employeeId, DateTime date);

    /// <summary>
    /// Get attendance for date range
    /// </summary>
    Task<IEnumerable<Attendance>> GetByDateRangeAsync(int employeeId, DateTime startDate, DateTime endDate);

    /// <summary>
    /// Get attendance for all employees on a date
    /// </summary>
    Task<IEnumerable<Attendance>> GetByDateAsync(DateTime date);

    /// <summary>
    /// Mark attendance
    /// </summary>
    Task<Attendance> MarkAttendanceAsync(int employeeId, DateTime date, DateTime checkInTime, AttendanceStatus status, string? remarks = null);

    /// <summary>
    /// Mark checkout
    /// </summary>
    Task<bool> MarkCheckOutAsync(int employeeId, DateTime date, DateTime checkOutTime);

    /// <summary>
    /// Get attendance summary for employee
    /// </summary>
    Task<Dictionary<AttendanceStatus, int>> GetAttendanceSummaryAsync(int employeeId, DateTime startDate, DateTime endDate);

    /// <summary>
    /// Check if attendance exists
    /// </summary>
    Task<bool> AttendanceExistsAsync(int employeeId, DateTime date);
}