using MediatR;
using Tax_Radar_Domain.Models;

namespace TaxRadar_Application.Queries.Tax;

public record GetVatRegistrationStatusQuery(Guid UserId):IRequest<VatRegistrationStatus>;