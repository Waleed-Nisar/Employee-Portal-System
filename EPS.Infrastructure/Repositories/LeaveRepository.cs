using EPS.Domain.Entities;
using EPS.Domain.Enums;
using EPS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EPS.Infrastructure.Repositories;

/// <summary>
/// Leave repository implementation
/// </summary>
public class LeaveRepository : Repository<Leave>, ILeaveRepository
{
    public LeaveRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Leave>> GetByEmployeeIdAsync(int employeeId)
    {
        return await _dbSet
            .Include(l => l.Employee)
            .Include(l => l.Approver)
            .Where(l => l.EmployeeId == employeeId)
            .OrderByDescending(l => l.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Leave>> GetPendingLeavesAsync()
    {
        return await _dbSet
            .Include(l => l.Employee)
                .ThenInclude(e => e.Department)
            .Include(l => l.Employee)
                .ThenInclude(e => e.Designation)
            .Where(l => l.Status == LeaveStatus.Pending)
            .OrderBy(l => l.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Leave>> GetByStatusAsync(LeaveStatus status)
    {
        return await _dbSet
            .Include(l => l.Employee)
            .Include(l => l.Approver)
            .Where(l => l.Status == status)
            .OrderByDescending(l => l.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Leave>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Include(l => l.Employee)
            .Where(l =>
                (l.StartDate >= startDate && l.StartDate <= endDate) ||
                (l.EndDate >= startDate && l.EndDate <= endDate) ||
                (l.StartDate <= startDate && l.EndDate >= endDate))
            .OrderBy(l => l.StartDate)
            .ToListAsync();
    }

    public async Task<Leave?> GetByIdWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(l => l.Employee)
                .ThenInclude(e => e.Department)
            .Include(l => l.Employee)
                .ThenInclude(e => e.Designation)
            .Include(l => l.Approver)
            .FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task<bool> HasOverlappingLeavesAsync(int employeeId, DateTime startDate, DateTime endDate, int? excludeLeaveId = null)
    {
        var query = _dbSet.Where(l =>
            l.EmployeeId == employeeId &&
            l.Status != LeaveStatus.Rejected &&
            l.Status != LeaveStatus.Cancelled &&
            ((l.StartDate >= startDate && l.StartDate <= endDate) ||
             (l.EndDate >= startDate && l.EndDate <= endDate) ||
             (l.StartDate <= startDate && l.EndDate >= endDate)));

        if (excludeLeaveId.HasValue)
        {
            query = query.Where(l => l.Id != excludeLeaveId.Value);
        }

        return await query.AnyAsync();
    }

    public async Task<Dictionary<LeaveType, int>> GetLeaveBalanceAsync(int employeeId, int year)
    {
        var leaves = await _dbSet
            .Where(l =>
                l.EmployeeId == employeeId &&
                l.Status == LeaveStatus.Approved &&
                l.StartDate.Year == year)
            .ToListAsync();

        var balance = new Dictionary<LeaveType, int>();

        foreach (LeaveType leaveType in Enum.GetValues(typeof(LeaveType)))
        {
            var totalDays = leaves
                .Where(l => l.LeaveType == leaveType)
                .Sum(l => l.TotalDays);

            balance[leaveType] = totalDays;
        }

        return balance;
    }

    public async Task<bool> ApproveLeavetAsync(int leaveId, int approvedBy, string? remarks = null)
    {
        var leave = await _dbSet.FindAsync(leaveId);
        if (leave == null || leave.Status != LeaveStatus.Pending)
        {
            return false;
        }

        leave.Status = LeaveStatus.Approved;
        leave.ApprovedBy = approvedBy;
        leave.ApprovedDate = DateTime.UtcNow;
        leave.ApprovalRemarks = remarks;
        leave.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RejectLeaveAsync(int leaveId, int approvedBy, string? remarks = null)
    {
        var leave = await _dbSet.FindAsync(leaveId);
        if (leave == null || leave.Status != LeaveStatus.Pending)
        {
            return false;
        }

        leave.Status = LeaveStatus.Rejected;
        leave.ApprovedBy = approvedBy;
        leave.ApprovedDate = DateTime.UtcNow;
        leave.ApprovalRemarks = remarks;
        leave.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }
}