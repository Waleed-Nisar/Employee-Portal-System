using EPS.Application.DTOs;
using EPS.Domain.Enums;

namespace EPS.Application.Interfaces;

/// <summary>
/// Attendance service interface
/// </summary>
public interface IAttendanceService
{
    Task<AttendanceDto?> GetByIdAsync(int id);
    Task<IEnumerable<AttendanceDto>> GetByEmployeeIdAsync(int employeeId);
    Task<IEnumerable<AttendanceDto>> GetByDateRangeAsync(int employeeId, DateTime startDate, DateTime endDate);
    Task<AttendanceDto> MarkAttendanceAsync(MarkAttendanceDto dto);
    Task<bool> MarkCheckOutAsync(int employeeId, DateTime date, DateTime checkOutTime);
    Task<Dictionary<AttendanceStatus, int>> GetAttendanceSummaryAsync(int employeeId, DateTime startDate, DateTime endDate);
}