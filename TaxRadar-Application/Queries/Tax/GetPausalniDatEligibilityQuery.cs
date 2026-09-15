using MediatR;
using Tax_Radar_Domain.Models;

namespace TaxRadar_Application.Queries.Tax;

public record GetPausalniDatEligibilityQuery(decimal Income, int Year):IRequest<PausalniDanEligibility>;