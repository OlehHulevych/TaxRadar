using MediatR;
using Microsoft.Extensions.Configuration;
using Tax_Radar_Domain.Entities;
using TaxRadar_Application.DTOs.Auth;
using TaxRadar_Application.Exceptions;
using TaxRadar_Application.Interfaces;
using TaxRadar_Application.Queries.Auth;

namespace TaxRadar_Application.Commands.Auth;

public class LoginCommandHandler(IUserRepository repository,IRefreshTokenRepository refreshTokenRepository, IJwtTokenService jwtTokenService, IPasswordHasher passwordHasher, IConfiguration configuration):IRequestHandler<LoginQuery, AuthTokenResponseDto>
{
    public async Task<AuthTokenResponseDto> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var user = await repository.GetUserByEmail(request.Email,cancellationToken);
        if (user==null) throw new BadRequestException("Invalid email");
        var isVerified = passwordHasher.Verify(request.Password, user.PasswordHash);
        if (!isVerified) throw new BadRequestException("Password is incorrect");
        var accessToken = jwtTokenService.GenerateToken(user.Id, user.Email.Value);
        var refreshToken = jwtTokenService.GenerateRefreshToken();
        var refreshTokenHash = jwtTokenService.HashRefreshToken(refreshToken);
        RefreshToken userRefreshToken = new RefreshToken(user.Id, refreshTokenHash,DateTimeOffset.UtcNow.AddDays(Convert.ToDouble(configuration["Jwt:RefreshTokenExpiryDays"])));
        await refreshTokenRepository.AddAsync(userRefreshToken, cancellationToken);
        return new AuthTokenResponseDto(accessToken, refreshToken);

    }
}