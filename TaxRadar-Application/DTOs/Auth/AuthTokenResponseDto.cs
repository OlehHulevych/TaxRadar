namespace TaxRadar_Application.DTOs.Auth;

public sealed record AuthTokenResponseDto(string AccessToken, string RefreshToken);
