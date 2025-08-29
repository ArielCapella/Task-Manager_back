namespace TaskManager.Api.DTOs;

public record RegisterDto(string Email, string Password);
public record LoginDto(string Email, string Password);

public record AuthResponse(string Token, DateTime ExpiresAtUtc, string UserId, string Email);