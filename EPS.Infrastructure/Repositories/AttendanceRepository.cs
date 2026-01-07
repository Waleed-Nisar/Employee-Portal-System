using EPS.Domain.Entities;
using EPS.Domain.Enums;
using EPS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EPS.Infrastructure.Repositories;

/// <summary>
/// Attendance repository implementation
/// </summary>
public class AttendanceRepository : Repository<Attendance>, IAttendanceRepository
{
    public AttendanceRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Attendance>> GetByEmployeeIdAsync(int employeeId)
    {
        return await _dbSet
            .Include(a => a.Employee)
            .Where(a => a.EmployeeId == employeeId)
            .OrderByDescending(a => a.Date)
            .ToListAsync();
    }

    public async Task<Attendance?> GetByEmployeeAndDateAsync(int employeeId, DateTime date)
    {
        return await _dbSet
            .Include(a => a.Employee)
            .FirstOrDefaultAsync(a =>
                a.EmployeeId == employeeId &&
                a.Date.Date == date.Date);
    }

    public async Task<IEnumerable<Attendance>> GetByDateRangeAsync(int employeeId, DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Include(a => a.Employee)
            .Where(a =>
                a.EmployeeId == employeeId &&
                a.Date.Date >= startDate.Date &&
                a.Date.Date <= endDate.Date)
            .OrderBy(a => a.Date)
            .ToListAsync();
    }

    public async Task<IEnumerable<Attendance>> GetByDateAsync(DateTime date)
    {
        return await _dbSet
            .Include(a => a.Employee)
                .ThenInclude(e => e.Department)
            .Where(a => a.Date.Date == date.Date)
            .OrderBy(a => a.Employee.FirstName)
            .ToListAsync();
    }

    public async Task<Attendance> MarkAttendanceAsync(int employeeId, DateTime date, DateTime checkInTime, AttendanceStatus status, string? remarks = null)
    {
        var existingAttendance = await GetByEmployeeAndDateAsync(employeeId, date);

        if (existingAttendance != null)
        {
            existingAttendance.CheckInTime = checkInTime;
            existingAttendance.Status = status;
            existingAttendance.Remarks = remarks;
            existingAttendance.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existingAttendance;
        }

        var attendance = new Attendance
        {
            EmployeeId = employeeId,
            Date = date.Date,
            CheckInTime = checkInTime,
            Status = status,
            Remarks = remarks,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _dbSet.AddAsync(attendance);
        await _context.SaveChangesAsync();

        return attendance;
    }

    public async Task<bool> MarkCheckOutAsync(int employeeId, DateTime date, DateTime checkOutTime)
    {
        var attendance = await GetByEmployeeAndDateAsync(employeeId, date);

        if (attendance == null)
        {
            return false;
        }

        attendance.CheckOutTime = checkOutTime;
        attendance.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Dictionary<AttendanceStatus, int>> GetAttendanceSummaryAsync(int employeeId, DateTime startDate, DateTime endDate)
    {
        var attendances = await GetByDateRangeAsync(employeeId, startDate, endDate);

        var summary = new Dictionary<AttendanceStatus, int>();

        foreach (AttendanceStatus status in Enum.GetValues(typeof(AttendanceStatus)))
        {
            summary[status] = attendances.Count(a => a.Status == status);
        }

        return summary;
    }

    public async Task<bool> AttendanceExistsAsync(int employeeId, DateTime date)
    {
        return await _dbSet.AnyAsync(a =>
            a.EmployeeId == employeeId &&
            a.Date.Date == date.Date);
    }
}