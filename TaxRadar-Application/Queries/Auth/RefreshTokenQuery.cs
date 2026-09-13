using MediatR;
using TaxRadar_Application.DTOs.Auth;

namespace TaxRadar_Application.Queries.Auth;

public record RefreshTokenQuery(String RefreshToken):IRequest<AuthTokenResponseDto>;