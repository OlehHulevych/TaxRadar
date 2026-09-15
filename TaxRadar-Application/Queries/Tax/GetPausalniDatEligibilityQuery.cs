using MediatR;
using Tax_Radar_Domain.Models;

namespace TaxRadar_Application.Queries.Tax;

public record GetPausalniDanEligibilityQuery(decimal Income, int Year):IRequest<PausalniDanEligibility>;