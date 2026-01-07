using AutoMapper;
using EPS.Application.DTOs;
using EPS.Application.Interfaces;
using EPS.Domain.Entities;
using EPS.Domain.Enums;
using EPS.Infrastructure.Repositories;

namespace EPS.Application.Services;

/// <summary>
/// Leave service implementation with business logic
/// </summary>
public class LeaveService : ILeaveService
{
    private readonly ILeaveRepository _leaveRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IMapper _mapper;

    public LeaveService(ILeaveRepository leaveRepository, IEmployeeRepository employeeRepository, IMapper mapper)
    {
        _leaveRepository = leaveRepository;
        _employeeRepository = employeeRepository;
        _mapper = mapper;
    }

    public async Task<LeaveDto?> GetByIdAsync(int id)
    {
        var leave = await _leaveRepository.GetByIdWithDetailsAsync(id);
        return leave == null ? null : _mapper.Map<LeaveDto>(leave);
    }

    public async Task<IEnumerable<LeaveDto>> GetAllAsync()
    {
        var leaves = await _leaveRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<LeaveDto>>(leaves);
    }

    public async Task<IEnumerable<LeaveDto>> GetByEmployeeIdAsync(int employeeId)
    {
        var leaves = await _leaveRepository.GetByEmployeeIdAsync(employeeId);
        return _mapper.Map<IEnumerable<LeaveDto>>(leaves);
    }

    public async Task<IEnumerable<LeaveDto>> GetPendingLeavesAsync()
    {
        var leaves = await _leaveRepository.GetPendingLeavesAsync();
        return _mapper.Map<IEnumerable<LeaveDto>>(leaves);
    }

    public async Task<IEnumerable<LeaveDto>> GetByStatusAsync(LeaveStatus status)
    {
        var leaves = await _leaveRepository.GetByStatusAsync(status);
        return _mapper.Map<IEnumerable<LeaveDto>>(leaves);
    }

    public async Task<LeaveDto> RequestLeaveAsync(LeaveRequestDto dto)
    {
        // Validate employee exists
        var employee = await _employeeRepository.GetByIdAsync(dto.EmployeeId);
        if (employee == null)
        {
            throw new InvalidOperationException($"Employee with ID {dto.EmployeeId} not found");
        }

        // Validate employee is active
        if (employee.Status != EmployeeStatus.Active)
        {
            throw new InvalidOperationException("Only active employees can request leaves");
        }

        // Validate dates
        if (dto.StartDate > dto.EndDate)
        {
            throw new InvalidOperationException("Start date cannot be after end date");
        }

        if (dto.StartDate < DateTime.Today)
        {
            throw new InvalidOperationException("Cannot request leave for past dates");
        }

        // Check for overlapping leaves
        var hasOverlap = await _leaveRepository.HasOverlappingLeavesAsync(dto.EmployeeId, dto.StartDate, dto.EndDate);
        if (hasOverlap)
        {
            throw new InvalidOperationException("Leave request overlaps with existing leave");
        }

        var leave = _mapper.Map<Leave>(dto);
        leave.Status = LeaveStatus.Pending;
        leave.CreatedAt = DateTime.UtcNow;
        leave.UpdatedAt = DateTime.UtcNow;

        await _leaveRepository.AddAsync(leave);
        await _leaveRepository.SaveChangesAsync();

        return _mapper.Map<LeaveDto>(leave);
    }

    public async Task<LeaveDto> ApproveLeaveAsync(int leaveId, int approvedBy, string? remarks = null)
    {
        var success = await _leaveRepository.ApproveLeavetAsync(leaveId, approvedBy, remarks);
        if (!success)
        {
            throw new InvalidOperationException("Failed to approve leave. Leave may not exist or is not pending");
        }

        var leave = await _leaveRepository.GetByIdWithDetailsAsync(leaveId);
        return _mapper.Map<LeaveDto>(leave!);
    }

    public async Task<LeaveDto> RejectLeaveAsync(int leaveId, int approvedBy, string? remarks = null)
    {
        var success = await _leaveRepository.RejectLeaveAsync(leaveId, approvedBy, remarks);
        if (!success)
        {
            throw new InvalidOperationException("Failed to reject leave. Leave may not exist or is not pending");
        }

        var leave = await _leaveRepository.GetByIdWithDetailsAsync(leaveId);
        return _mapper.Map<LeaveDto>(leave!);
    }

    public async Task<bool> CancelLeaveAsync(int leaveId, int employeeId)
    {
        var leave = await _leaveRepository.GetByIdAsync(leaveId);
        if (leave == null)
        {
            return false;
        }

        if (leave.EmployeeId != employeeId)
        {
            throw new UnauthorizedAccessException("You can only cancel your own leaves");
        }

        if (leave.Status != LeaveStatus.Pending && leave.Status != LeaveStatus.Approved)
        {
            throw new InvalidOperationException("Only pending or approved leaves can be cancelled");
        }

        leave.Status = LeaveStatus.Cancelled;
        leave.UpdatedAt = DateTime.UtcNow;

        _leaveRepository.Update(leave);
        await _leaveRepository.SaveChangesAsync();

        return true;
    }

    public async Task<Dictionary<LeaveType, int>> GetLeaveBalanceAsync(int employeeId, int year)
    {
        return await _leaveRepository.GetLeaveBalanceAsync(employeeId, year);
    }
}