using EPS.Application.DTOs;
using EPS.Domain.Enums;

namespace EPS.Application.Interfaces;

/// <summary>
/// Leave service interface for business logic
/// </summary>
public interface ILeaveService
{
    Task<LeaveDto?> GetByIdAsync(int id);
    Task<IEnumerable<LeaveDto>> GetAllAsync();
    Task<IEnumerable<LeaveDto>> GetByEmployeeIdAsync(int employeeId);
    Task<IEnumerable<LeaveDto>> GetPendingLeavesAsync();
    Task<IEnumerable<LeaveDto>> GetByStatusAsync(LeaveStatus status);
    Task<LeaveDto> RequestLeaveAsync(LeaveRequestDto dto);
    Task<LeaveDto> ApproveLeaveAsync(int leaveId, int approvedBy, string? remarks = null);
    Task<LeaveDto> RejectLeaveAsync(int leaveId, int approvedBy, string? remarks = null);
    Task<bool> CancelLeaveAsync(int leaveId, int employeeId);
    Task<Dictionary<LeaveType, int>> GetLeaveBalanceAsync(int employeeId, int year);
}