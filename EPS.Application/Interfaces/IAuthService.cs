using EPS.Application.DTOs;

namespace EPS.Application.Interfaces;

/// <summary>
/// Authentication service interface for JWT token generation and user management
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Authenticate user and generate JWT token
    /// </summary>
    Task<TokenDto> LoginAsync(LoginDto dto);

    /// <summary>
    /// Register new user
    /// </summary>
    Task<TokenDto> RegisterAsync(RegisterDto dto);

    /// <summary>
    /// Refresh JWT token using refresh token
    /// </summary>
    Task<TokenDto> RefreshTokenAsync(RefreshTokenDto dto);

    /// <summary>
    /// Revoke refresh token (logout)
    /// </summary>
    Task<bool> RevokeTokenAsync(string userId);

    /// <summary>
    /// Get user by ID
    /// </summary>
    Task<UserDto?> GetUserByIdAsync(string userId);
}