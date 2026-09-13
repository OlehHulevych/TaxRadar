using MediatR;
using Tax_Radar_Domain.Entities;
using TaxRadar_Application.Exceptions;
using TaxRadar_Application.Interfaces;
using TaxRadar_Application.Queries.Auth;

namespace TaxRadar_Application.Commands.Auth;

public class RevokeRefreshTokenHandler(IRefreshTokenRepository repository):IRequestHandler<RevokeRefreshTokenQuery>
{
    public async Task Handle(RevokeRefreshTokenQuery request, CancellationToken cancellationToken)
    {
        var refreshToken = await repository.GetRefreshTokenByHash(request.RefreshToken, cancellationToken);
        if (refreshToken == null) throw new NotFoundException("Refresh token is required");
        refreshToken.Revoke();
        await repository.SaveChangesAsync(cancellationToken);
    }
}