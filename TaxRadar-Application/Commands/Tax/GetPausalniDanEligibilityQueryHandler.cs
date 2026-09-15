using MediatR;
using Tax_Radar_Domain.Models;
using TaxRadar_Application.Interfaces;
using TaxRadar_Application.Queries.Tax;
using TaxRadar_Application.Services;

namespace TaxRadar_Application.Commands.Tax;

public class GetPausalniDanEligibilityQueryHandler(ITaxRatesProvider taxRatesProvider):IRequestHandler<GetPausalniDanEligibilityQuery, PausalniDanEligibility>
{
    public async Task<PausalniDanEligibility> Handle(GetPausalniDanEligibilityQuery request, CancellationToken cancellationToken)
    {
        var rates = await taxRatesProvider.GetRatesForYearAsync(request.Year,cancellationToken);
        return BandSelectionService.BandEvaluation(request.Income, rates);
    }
}