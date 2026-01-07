using EPS.Domain.Entities;
using EPS.Domain.Enums;
using EPS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EPS.Infrastructure.Repositories;

/// <summary>
/// Employee repository implementation with custom queries
/// </summary>
public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Employee?> GetByEmployeeIdAsync(string employeeId)
    {
        return await _dbSet
            .Include(e => e.Department)
            .Include(e => e.Designation)
            .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);
    }

    public async Task<Employee?> GetByEmailAsync(string email)
    {
        return await _dbSet
            .Include(e => e.Department)
            .Include(e => e.Designation)
            .FirstOrDefaultAsync(e => e.Email == email);
    }

    public async Task<Employee?> GetByUserIdAsync(string userId)
    {
        return await _dbSet
            .Include(e => e.Department)
            .Include(e => e.Designation)
            .FirstOrDefaultAsync(e => e.UserId == userId);
    }

    public async Task<IEnumerable<Employee>> GetAllWithDetailsAsync()
    {
        return await _dbSet
            .Include(e => e.Department)
            .Include(e => e.Designation)
            .Include(e => e.Manager)
            .OrderBy(e => e.EmployeeId)
            .ToListAsync();
    }

    public async Task<Employee?> GetByIdWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(e => e.Department)
            .Include(e => e.Designation)
            .Include(e => e.Manager)
            .Include(e => e.Subordinates)
            .Include(e => e.Leaves.OrderByDescending(l => l.CreatedAt).Take(10))
            .Include(e => e.Attendances.OrderByDescending(a => a.Date).Take(30))
            .Include(e => e.Documents)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<Employee>> GetByDepartmentAsync(int departmentId)
    {
        return await _dbSet
            .Include(e => e.Designation)
            .Where(e => e.DepartmentId == departmentId)
            .OrderBy(e => e.FirstName)
            .ToListAsync();
    }

    public async Task<IEnumerable<Employee>> GetSubordinatesAsync(int managerId)
    {
        return await _dbSet
            .Include(e => e.Department)
            .Include(e => e.Designation)
            .Where(e => e.ManagerId == managerId)
            .OrderBy(e => e.FirstName)
            .ToListAsync();
    }

    public async Task<IEnumerable<Employee>> SearchAsync(string searchTerm)
    {
        var term = searchTerm.ToLower();
        return await _dbSet
            .Include(e => e.Department)
            .Include(e => e.Designation)
            .Where(e =>
                e.EmployeeId.ToLower().Contains(term) ||
                e.FirstName.ToLower().Contains(term) ||
                e.LastName.ToLower().Contains(term) ||
                e.Email.ToLower().Contains(term))
            .OrderBy(e => e.FirstName)
            .ToListAsync();
    }

    public async Task<IEnumerable<Employee>> GetByStatusAsync(EmployeeStatus status)
    {
        return await _dbSet
            .Include(e => e.Department)
            .Include(e => e.Designation)
            .Where(e => e.Status == status)
            .OrderBy(e => e.FirstName)
            .ToListAsync();
    }

    public async Task<(IEnumerable<Employee> Employees, int TotalCount)> GetPaginatedAsync(
        int page,
        int pageSize,
        string? searchTerm = null,
        int? departmentId = null,
        EmployeeStatus? status = null)
    {
        var query = _dbSet
            .Include(e => e.Department)
            .Include(e => e.Designation)
            .AsQueryable();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.ToLower();
            query = query.Where(e =>
                e.EmployeeId.ToLower().Contains(term) ||
                e.FirstName.ToLower().Contains(term) ||
                e.LastName.ToLower().Contains(term) ||
                e.Email.ToLower().Contains(term));
        }

        if (departmentId.HasValue)
        {
            query = query.Where(e => e.DepartmentId == departmentId.Value);
        }

        if (status.HasValue)
        {
            query = query.Where(e => e.Status == status.Value);
        }

        var totalCount = await query.CountAsync();

        var employees = await query
            .OrderBy(e => e.EmployeeId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (employees, totalCount);
    }

    public async Task<string> GenerateEmployeeIdAsync()
    {
        var lastEmployee = await _dbSet
            .OrderByDescending(e => e.Id)
            .FirstOrDefaultAsync();

        if (lastEmployee == null)
        {
            return "EMP-0001";
        }

        // Extract number from last employee ID (EMP-XXXX)
        var lastNumber = int.Parse(lastEmployee.EmployeeId.Split('-')[1]);
        var newNumber = lastNumber + 1;

        return $"EMP-{newNumber:D4}";
    }

    public async Task<bool> HasActiveLeavesAsync(int employeeId)
    {
        return await _context.Leaves
            .AnyAsync(l =>
                l.EmployeeId == employeeId &&
                l.Status == LeaveStatus.Approved &&
                l.EndDate >= DateTime.Today);
    }
}