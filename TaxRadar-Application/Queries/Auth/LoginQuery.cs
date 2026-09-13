using MediatR;
using TaxRadar_Application.DTOs.Auth;

namespace TaxRadar_Application.Queries.Auth;

public sealed record LoginQuery(string Email, string Password):IRequest<AuthTokenResponseDto>;
