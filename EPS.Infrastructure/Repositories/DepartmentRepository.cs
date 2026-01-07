using EPS.Domain.Entities;
using EPS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EPS.Infrastructure.Repositories;

/// <summary>
/// Department repository implementation
/// </summary>
public class DepartmentRepository : Repository<Department>, IDepartmentRepository
{
    public DepartmentRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Department?> GetByCodeAsync(string code)
    {
        return await _dbSet
            .Include(d => d.HeadEmployee)
            .FirstOrDefaultAsync(d => d.Code == code);
    }

    public async Task<IEnumerable<Department>> GetActiveDepartmentsAsync()
    {
        return await _dbSet
            .Include(d => d.HeadEmployee)
            .Where(d => d.IsActive)
            .OrderBy(d => d.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<(Department Department, int EmployeeCount)>> GetDepartmentsWithCountAsync()
    {
        return await _dbSet
            .Include(d => d.HeadEmployee)
            .Select(d => new
            {
                Department = d,
                EmployeeCount = d.Employees.Count
            })
            .OrderBy(x => x.Department.Name)
            .ToListAsync()
            .ContinueWith(task => task.Result.Select(x => (x.Department, x.EmployeeCount)));
    }

    public async Task<Department?> GetByIdWithEmployeesAsync(int id)
    {
        return await _dbSet
            .Include(d => d.HeadEmployee)
            .Include(d => d.Employees)
                .ThenInclude(e => e.Designation)
            .FirstOrDefaultAsync(d => d.Id == id);
    }
}