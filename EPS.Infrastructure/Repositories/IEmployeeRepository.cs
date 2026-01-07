using EPS.Domain.Entities;
using EPS.Domain.Enums;

namespace EPS.Infrastructure.Repositories;

/// <summary>
/// Employee-specific repository interface with custom queries
/// </summary>
public interface IEmployeeRepository : IRepository<Employee>
{
    /// <summary>
    /// Get employee by unique employee ID (EMP-XXXX)
    /// </summary>
    Task<Employee?> GetByEmployeeIdAsync(string employeeId);

    /// <summary>
    /// Get employee by email
    /// </summary>
    Task<Employee?> GetByEmailAsync(string email);

    /// <summary>
    /// Get employee by user ID (Identity)
    /// </summary>
    Task<Employee?> GetByUserIdAsync(string userId);

    /// <summary>
    /// Get all employees with department and designation
    /// </summary>
    Task<IEnumerable<Employee>> GetAllWithDetailsAsync();

    /// <summary>
    /// Get employee with all related data
    /// </summary>
    Task<Employee?> GetByIdWithDetailsAsync(int id);

    /// <summary>
    /// Get employees by department
    /// </summary>
    Task<IEnumerable<Employee>> GetByDepartmentAsync(int departmentId);

    /// <summary>
    /// Get employees by manager
    /// </summary>
    Task<IEnumerable<Employee>> GetSubordinatesAsync(int managerId);

    /// <summary>
    /// Search employees by name or employee ID
    /// </summary>
    Task<IEnumerable<Employee>> SearchAsync(string searchTerm);

    /// <summary>
    /// Get employees by status
    /// </summary>
    Task<IEnumerable<Employee>> GetByStatusAsync(EmployeeStatus status);

    /// <summary>
    /// Get paginated employees with filters
    /// </summary>
    Task<(IEnumerable<Employee> Employees, int TotalCount)> GetPaginatedAsync(
        int page,
        int pageSize,
        string? searchTerm = null,
        int? departmentId = null,
        EmployeeStatus? status = null);

    /// <summary>
    /// Generate next employee ID
    /// </summary>
    Task<string> GenerateEmployeeIdAsync();

    /// <summary>
    /// Check if employee has active leaves
    /// </summary>
    Task<bool> HasActiveLeavesAsync(int employeeId);
}