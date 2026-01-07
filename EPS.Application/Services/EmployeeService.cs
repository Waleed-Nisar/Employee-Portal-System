using AutoMapper;
using EPS.Application.DTOs;
using EPS.Application.Interfaces;
using EPS.Domain.Entities;
using EPS.Domain.Enums;
using EPS.Infrastructure.Repositories;

namespace EPS.Application.Services;

/// <summary>
/// Employee service implementation with business logic
/// </summary>
public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IMapper _mapper;

    public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper)
    {
        _employeeRepository = employeeRepository;
        _mapper = mapper;
    }

    public async Task<EmployeeDto?> GetByIdAsync(int id)
    {
        var employee = await _employeeRepository.GetByIdWithDetailsAsync(id);
        return employee == null ? null : _mapper.Map<EmployeeDto>(employee);
    }

    public async Task<EmployeeDto?> GetByEmployeeIdAsync(string employeeId)
    {
        var employee = await _employeeRepository.GetByEmployeeIdAsync(employeeId);
        return employee == null ? null : _mapper.Map<EmployeeDto>(employee);
    }

    public async Task<EmployeeDto?> GetByEmailAsync(string email)
    {
        var employee = await _employeeRepository.GetByEmailAsync(email);
        return employee == null ? null : _mapper.Map<EmployeeDto>(employee);
    }

    public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
    {
        var employees = await _employeeRepository.GetAllWithDetailsAsync();
        return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
    }

    public async Task<(IEnumerable<EmployeeDto> Employees, int TotalCount)> GetPaginatedAsync(
        int page, int pageSize, string? searchTerm = null, int? departmentId = null, EmployeeStatus? status = null)
    {
        var (employees, totalCount) = await _employeeRepository.GetPaginatedAsync(page, pageSize, searchTerm, departmentId, status);
        var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        return (employeeDtos, totalCount);
    }

    public async Task<IEnumerable<EmployeeDto>> GetByDepartmentAsync(int departmentId)
    {
        var employees = await _employeeRepository.GetByDepartmentAsync(departmentId);
        return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
    }

    public async Task<IEnumerable<EmployeeDto>> SearchAsync(string searchTerm)
    {
        var employees = await _employeeRepository.SearchAsync(searchTerm);
        return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
    }

    public async Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto, string createdBy)
    {
        // Validate email uniqueness
        var existingEmployee = await _employeeRepository.GetByEmailAsync(dto.Email);
        if (existingEmployee != null)
        {
            throw new InvalidOperationException($"Employee with email {dto.Email} already exists");
        }

        // Generate Employee ID
        var employeeId = await _employeeRepository.GenerateEmployeeIdAsync();

        var employee = _mapper.Map<Employee>(dto);
        employee.EmployeeId = employeeId;
        employee.CreatedBy = createdBy;
        employee.UpdatedBy = createdBy;
        employee.CreatedAt = DateTime.UtcNow;
        employee.UpdatedAt = DateTime.UtcNow;

        await _employeeRepository.AddAsync(employee);
        await _employeeRepository.SaveChangesAsync();

        return _mapper.Map<EmployeeDto>(employee);
    }

    public async Task<EmployeeDto> UpdateAsync(UpdateEmployeeDto dto, string updatedBy)
    {
        var employee = await _employeeRepository.GetByIdAsync(dto.Id);
        if (employee == null)
        {
            throw new InvalidOperationException($"Employee with ID {dto.Id} not found");
        }

        // Validate email uniqueness (excluding current employee)
        var existingEmployee = await _employeeRepository.GetByEmailAsync(dto.Email);
        if (existingEmployee != null && existingEmployee.Id != dto.Id)
        {
            throw new InvalidOperationException($"Employee with email {dto.Email} already exists");
        }

        _mapper.Map(dto, employee);
        employee.UpdatedBy = updatedBy;
        employee.UpdatedAt = DateTime.UtcNow;

        _employeeRepository.Update(employee);
        await _employeeRepository.SaveChangesAsync();

        return _mapper.Map<EmployeeDto>(employee);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        if (!await CanDeleteAsync(id))
        {
            throw new InvalidOperationException("Cannot delete employee with active leaves");
        }

        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee == null)
        {
            return false;
        }

        _employeeRepository.Delete(employee);
        await _employeeRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> CanDeleteAsync(int id)
    {
        return !await _employeeRepository.HasActiveLeavesAsync(id);
    }
}