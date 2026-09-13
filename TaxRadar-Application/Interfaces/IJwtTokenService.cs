using TaxRadar_Application.DTOs.Auth;

namespace TaxRadar_Application.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(Guid userId, string email);
    string GenerateRefreshToken();

}
