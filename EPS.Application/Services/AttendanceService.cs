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
public class AttendanceService : IAttendanceService
{
    private readonly IAttendanceRepository _attendanceRepository;
    private readonly IMapper _mapper;

    public AttendanceService(IAttendanceRepository attendanceRepository, IMapper mapper)
    {
        _attendanceRepository = attendanceRepository;
        _mapper = mapper;
    }

    public async Task<AttendanceDto?> GetByIdAsync(int id)
    {
        var attendance = await _attendanceRepository.GetByIdAsync(id);
        return attendance == null ? null : _mapper.Map<AttendanceDto>(attendance);
    }

    public async Task<IEnumerable<AttendanceDto>> GetByEmployeeIdAsync(int employeeId)
    {
        var attendances = await _attendanceRepository.GetByEmployeeIdAsync(employeeId);
        return _mapper.Map<IEnumerable<AttendanceDto>>(attendances);
    }

    public async Task<IEnumerable<AttendanceDto>> GetByDateRangeAsync(int employeeId, DateTime startDate, DateTime endDate)
    {
        var attendances = await _attendanceRepository.GetByDateRangeAsync(employeeId, startDate, endDate);
        return _mapper.Map<IEnumerable<AttendanceDto>>(attendances);
    }

    public async Task<AttendanceDto> MarkAttendanceAsync(MarkAttendanceDto dto)
    {
        // Validate date is not in future
        if (dto.Date.Date > DateTime.Today)
        {
            throw new InvalidOperationException("Cannot mark attendance for future dates");
        }

        var attendance = await _attendanceRepository.MarkAttendanceAsync(
            dto.EmployeeId,
            dto.Date,
            dto.CheckInTime,
            dto.Status,
            dto.Remarks);

        if (dto.CheckOutTime.HasValue)
        {
            attendance.CheckOutTime = dto.CheckOutTime;
            await _attendanceRepository.SaveChangesAsync();
        }

        return _mapper.Map<AttendanceDto>(attendance);
    }

    public async Task<bool> MarkCheckOutAsync(int employeeId, DateTime date, DateTime checkOutTime)
    {
        return await _attendanceRepository.MarkCheckOutAsync(employeeId, date, checkOutTime);
    }

    public async Task<Dictionary<AttendanceStatus, int>> GetAttendanceSummaryAsync(int employeeId, DateTime startDate, DateTime endDate)
    {
        return await _attendanceRepository.GetAttendanceSummaryAsync(employeeId, startDate, endDate);
    }
}