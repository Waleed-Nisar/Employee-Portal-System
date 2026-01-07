using EPS.Application.DTOs;
using EPS.Domain.Enums;

namespace EPS.Application.Interfaces;

/// <summary>
/// Department service interface
/// </summary>
public interface IDepartmentService
{
    Task<DepartmentDto?> GetByIdAsync(int id);
    Task<IEnumerable<DepartmentDto>> GetAllAsync();
    Task<IEnumerable<DepartmentDto>> GetActiveDepartmentsAsync();
    Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto);
    Task<DepartmentDto> UpdateAsync(int id, CreateDepartmentDto dto);
    Task<bool> DeleteAsync(int id);
}