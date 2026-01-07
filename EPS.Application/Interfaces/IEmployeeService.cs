using EPS.Application.DTOs;
using EPS.Domain.Enums;

namespace EPS.Application.Interfaces;

/// <summary>
/// Employee service interface for business logic
/// </summary>
public interface IEmployeeService
{
    Task<EmployeeDto?> GetByIdAsync(int id);
    Task<EmployeeDto?> GetByEmployeeIdAsync(string employeeId);
    Task<EmployeeDto?> GetByEmailAsync(string email);
    Task<IEnumerable<EmployeeDto>> GetAllAsync();
    Task<(IEnumerable<EmployeeDto> Employees, int TotalCount)> GetPaginatedAsync(
        int page, int pageSize, string? searchTerm = null, int? departmentId = null, EmployeeStatus? status = null);
    Task<IEnumerable<EmployeeDto>> GetByDepartmentAsync(int departmentId);
    Task<IEnumerable<EmployeeDto>> SearchAsync(string searchTerm);
    Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto, string createdBy);
    Task<EmployeeDto> UpdateAsync(UpdateEmployeeDto dto, string updatedBy);
    Task<bool> DeleteAsync(int id);
    Task<bool> CanDeleteAsync(int id);
}