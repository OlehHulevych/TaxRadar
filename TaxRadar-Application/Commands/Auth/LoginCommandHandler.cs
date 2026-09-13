using MediatR;
using TaxRadar_Application.DTOs.Auth;
using TaxRadar_Application.Interfaces;
using TaxRadar_Application.Queries.Auth;

namespace TaxRadar_Application.Commands.Auth;

public class LoginCommandHandler(IUserRepository repository, IJwtTokenService jwtTokenService, IPasswordHasher passwordHasher):IRequestHandler<LoginQuery, AuthTokenResponseDto>
{
    public async Task<AuthTokenResponseDto> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var isExist = await repository.CheckIfUserExistByEmail(request.Email,cancellationToken);
        if (!isExist) throw new ArgumentException("User with this email doesn't exist");
        var isVerified = passwordHasher.Verify(request.Password);

    }
}