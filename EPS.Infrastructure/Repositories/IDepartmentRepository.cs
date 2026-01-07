using EPS.Domain.Entities;

namespace EPS.Infrastructure.Repositories;

/// <summary>
/// Department-specific repository interface
/// </summary>
public interface IDepartmentRepository : IRepository<Department>
{
    /// <summary>
    /// Get department by code
    /// </summary>
    Task<Department?> GetByCodeAsync(string code);

    /// <summary>
    /// Get all active departments
    /// </summary>
    Task<IEnumerable<Department>> GetActiveDepartmentsAsync();

    /// <summary>
    /// Get department with employee count
    /// </summary>
    Task<IEnumerable<(Department Department, int EmployeeCount)>> GetDepartmentsWithCountAsync();

    /// <summary>
    /// Get department with all employees
    /// </summary>
    Task<Department?> GetByIdWithEmployeesAsync(int id);
}