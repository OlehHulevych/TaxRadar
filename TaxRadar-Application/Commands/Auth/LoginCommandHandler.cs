using MediatR;
using Tax_Radar_Domain.Entities;
using TaxRadar_Application.DTOs.Auth;
using TaxRadar_Application.Exceptions;
using TaxRadar_Application.Interfaces;
using TaxRadar_Application.Queries.Auth;

namespace TaxRadar_Application.Commands.Auth;

public class LoginCommandHandler(IUserRepository repository,IRefreshTokenRepository refreshTokenRepository, IJwtTokenService jwtTokenService, IPasswordHasher passwordHasher):IRequestHandler<LoginQuery, AuthTokenResponseDto>
{
    public async Task<AuthTokenResponseDto> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var user = await repository.GetUserByEmail(request.Email,cancellationToken);
        if (user==null) throw new NotFoundException(nameof(User),request.Email );
        var isVerified = passwordHasher.Verify(request.Password, user.PasswordHash);
        if (!isVerified) throw new ArgumentException("Password is incorrect");
        var accessToken = jwtTokenService.GenerateToken(user.Id, user.Email.Value);
        var refreshToken = jwtTokenService.GenerateRefreshToken();
        RefreshToken userRefreshToken = new RefreshToken(user.Id, refreshToken,DateTimeOffset.UtcNow.AddDays(30));
        await refreshTokenRepository.AddAsync(userRefreshToken, cancellationToken);
        return new AuthTokenResponseDto(accessToken, refreshToken);

    }
}