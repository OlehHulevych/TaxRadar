using MediatR;
using Tax_Radar_Domain.Entities;
using TaxRadar_Application.DTOs.Auth;
using TaxRadar_Application.Exceptions;
using TaxRadar_Application.Interfaces;
using TaxRadar_Application.Queries.Auth;

namespace TaxRadar_Application.Commands.Auth;

public class RefreshTokenCommandHandler( IJwtTokenService jwtTokenService, IRefreshTokenRepository refreshTokenRepository, IUserRepository userRepository):IRequestHandler<RefreshTokenQuery, AuthTokenResponseDto>
{
    public async Task<AuthTokenResponseDto> Handle(RefreshTokenQuery request, CancellationToken cancellationToken)
    {
        var userRefreshToken =
            await refreshTokenRepository.GetRefreshTokenByHash(request.RefreshToken, cancellationToken);
        if (userRefreshToken==null || userRefreshToken.ExpiresAt < DateTimeOffset.UtcNow) throw new ArgumentException("The refresh token is expired");
        var user = await userRepository.GetByIdAsync(userRefreshToken.UserId, cancellationToken);
        if (user == null) throw new NotFoundException(nameof(User), userRefreshToken.UserId);
        var accessToken =  jwtTokenService.GenerateToken(user.Id, user.Email.Value);
        string newRefreshToken = jwtTokenService.GenerateRefreshToken();
        RefreshToken newUserRefreshToken = new RefreshToken(userRefreshToken.UserId, newRefreshToken,DateTimeOffset.UtcNow.AddDays(7));
        await refreshTokenRepository.AddAsync(newUserRefreshToken, cancellationToken);
        return new AuthTokenResponseDto(accessToken, newRefreshToken);


    }
}