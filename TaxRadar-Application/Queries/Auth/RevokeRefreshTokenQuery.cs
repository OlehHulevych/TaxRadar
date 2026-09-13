using MediatR;

namespace TaxRadar_Application.Queries.Auth;

public record RevokeRefreshTokenQuery(string RefreshToken):IRequest;