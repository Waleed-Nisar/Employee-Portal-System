using AutoMapper;
using EPS.Application.DTOs;
using EPS.Application.Interfaces;
using EPS.Domain.Entities;
using EPS.Domain.Enums;
using EPS.Infrastructure.Repositories;

namespace EPS.Application.Services;

/// <summary>
/// Department service implementation
/// </summary>
public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IMapper _mapper;

    public DepartmentService(IDepartmentRepository departmentRepository, IMapper mapper)
    {
        _departmentRepository = departmentRepository;
        _mapper = mapper;
    }

    public async Task<DepartmentDto?> GetByIdAsync(int id)
    {
        var department = await _departmentRepository.GetByIdWithEmployeesAsync(id);
        return department == null ? null : _mapper.Map<DepartmentDto>(department);
    }

    public async Task<IEnumerable<DepartmentDto>> GetAllAsync()
    {
        var departments = await _departmentRepository.GetDepartmentsWithCountAsync();
        return departments.Select(d => {
            var dto = _mapper.Map<DepartmentDto>(d.Department);
            dto.EmployeeCount = d.EmployeeCount;
            return dto;
        });
    }

    public async Task<IEnumerable<DepartmentDto>> GetActiveDepartmentsAsync()
    {
        var departments = await _departmentRepository.GetActiveDepartmentsAsync();
        return _mapper.Map<IEnumerable<DepartmentDto>>(departments);
    }

    public async Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto)
    {
        var department = _mapper.Map<Department>(dto);
        department.CreatedAt = DateTime.UtcNow;
        department.UpdatedAt = DateTime.UtcNow;

        await _departmentRepository.AddAsync(department);
        await _departmentRepository.SaveChangesAsync();

        return _mapper.Map<DepartmentDto>(department);
    }

    public async Task<DepartmentDto> UpdateAsync(int id, CreateDepartmentDto dto)
    {
        var department = await _departmentRepository.GetByIdAsync(id);
        if (department == null)
        {
            throw new InvalidOperationException($"Department with ID {id} not found");
        }

        _mapper.Map(dto, department);
        department.UpdatedAt = DateTime.UtcNow;

        _departmentRepository.Update(department);
        await _departmentRepository.SaveChangesAsync();

        return _mapper.Map<DepartmentDto>(department);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var department = await _departmentRepository.GetByIdAsync(id);
        if (department == null)
        {
            return false;
        }

        _departmentRepository.Delete(department);
        await _departmentRepository.SaveChangesAsync();

        return true;
    }
}
